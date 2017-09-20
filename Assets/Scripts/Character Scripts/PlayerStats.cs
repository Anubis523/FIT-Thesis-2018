﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Class that stores and manages player stats with some additional functions
/// </summary>
public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField]
    StatsData stats;

    //camera needed to obscure/mask the player on separate layer by itself to simulate the transparency oscillation of the invincibility frames of older games
    [SerializeField]
    Camera camera;

    PlayerStats instance;
    /*number of intervals to be calculated by duration and wait time total*/
    int intervals = 0;

    int currentHP;
    bool invincible;
    public static int cash, hp;

    PlayerController player;
    int wallet, energy;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        player = this.GetComponent<PlayerController>();
        currentHP = stats.maxHP;
        hp = currentHP;
        invincible = false;
        wallet = 0;
        energy = stats.maxEnergy;
    }

    void Update()
    {
        //Debug.Log("Invincible state is " + invincible);
        cash = wallet;
        hp = currentHP;
        //debugger 
        Debug.Log("Stats are as follows: " + "HP is " + currentHP + "/" + stats.maxHP
            + " Scrap :" + wallet + " Energy is: " + energy + " / " + stats.maxEnergy);
        //makes sure the pickups addition to stat does not exceed the stat max itself
        if (currentHP > stats.maxHP) { currentHP = stats.maxHP; }
        if (wallet > stats.maxMoney) { wallet = stats.maxMoney; }
        if (energy > stats.maxEnergy) { energy = stats.maxEnergy; }
        Mathf.Clamp(currentHP, 0, stats.maxHP);

        if (invincible)
        {
            intervals = Mathf.RoundToInt((stats.duration * (1 / stats.waitTime)) / 2);

            StartCoroutine(IFramez());
            //Debug.Log("Now Mortal Again!!");
        }

    }

    public void resetState()
    {
        currentHP = stats.maxHP;
    }

    //culls the layer that holds player from game render view (layer 9)
    void CullOn()
    {
        camera.cullingMask &= ~(1 << 9);
    }
    //reveals the layer that holds the player in game render view
    void CullOff()
    {
        camera.cullingMask |= (1 << 9);
    }

    IEnumerator IFramez()
    {
        for (int i = 0; i < intervals; i++)
        {
            CullOn();
            //Debug.Log("Culling");
            yield return new WaitForSeconds(stats.waitTime / 2);
            CullOff();
            yield return new WaitForSeconds(stats.waitTime / 2);
            invincible = false;
            // Debug.Log("Not Culling");
        }
        //yield return null;
    }

    //handles damage taken by player character and its effects
    public void takeDamage(int dmg)
    {
        if (!invincible)
        {
            currentHP -= dmg;
            if (currentHP <= 0)
            {
                player.die();
            }
            else
            {
                invincible = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && !invincible)
        {
            takeDamage(col.gameObject.GetComponent<Enemy>().damage);
            Debug.Log("I've been hit!");
        }

        //logic that makes sure pickup affects the numbers of the stats
        if (col.gameObject.tag == "PickUp")
        {
            int amount  = 0;

            if (col.GetComponent<PickUp>().pickup == PickupType.Health)
            {
                amount = Mathf.RoundToInt(.25f * (stats.maxHP));
                currentHP = (currentHP + amount > stats.maxHP) ? stats.maxHP : currentHP + amount;
            }

            else if (col.GetComponent<PickUp>().pickup == PickupType.Money)
            {
                amount = col.GetComponent<PickUp>().purse;
                wallet += amount;
            }

            Debug.Log("The type of pick up is " + col.GetComponent<PickUp>().pickup);
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death Object")
        {
            currentHP = 0;
        }
    }
}


