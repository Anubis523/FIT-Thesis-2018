﻿using UnityEngine;
using System;

/// <summary>
/// Enemy base class used to determine basic information and behavior of enemy class
/// </summary>
[RequireComponent(typeof (AudioSource))]
public class Enemy : MonoBehaviour
{
    #region Global Variables

    Vector3 startLocation; //used to warp the enemies back to their original positions upon restart
    /// <summary>
    /// Event that transmits from Enemy to trigger ammo stocking 
    /// </summary>
    public event Action On_BecomeAmmo_Sent; //need to assign subscriber < Kev Note

    /// <summary>
    /// Event that transmits when the enemy becomes random loot on "destruction"
    /// </summary>
    public event Action<Vector3, Quaternion> On_RandomLootDropped_Sent;

    /// <summary>
    /// Event that transmits when the enemy becomes the default loot upon "destruction"
    /// </summary>
    public event Action<Vector3, Quaternion, PickupType> On_DefaultLootDrop_Sent;
    AudioSource boomBox;
    //allows for adjustment of enemy health points
    [Range(0, 25)]
    public int HP;
    public bool randomDrop = false;
    //used to save max HP info for enemy
    public int saveHP;

    string currentAttackType = "";

    public CapsuleCollider hurtBox;
    #endregion

    // Use this for initialization
    protected virtual void Start()
    {
        boomBox = gameObject.GetComponent<AudioSource>();
        startLocation = gameObject.transform.root.gameObject.transform.position;
        saveHP = HP;
        On_RandomLootDropped_Sent += LootGenerator.instance.On_RandomLootDropped_Received;
        On_DefaultLootDrop_Sent += LootGenerator.instance.On_DefaultLootDrop_Received;
        GameManager.instance.On_RestartState_Sent += On_RestartButton_Received;
    }

    protected virtual void Update()
    {
        Mathf.Clamp(HP, 0, 25);

        if (HP <= 0)
        {
            HP = 0;
            DealDeath();
        }
    }
    /// <summary>
    /// Logic ran upon death using several functions
    /// </summary>
    private void DealDeath()
    {
        boomBox.Play(); //plays the hurt sound

        if (currentAttackType == "HitBox")
        {
            BecomePickUp();
        }
        else if (currentAttackType == "Hand")
        {
            BecomeProjectile();
        }
        else
            gameObject.transform.root.gameObject.SetActive(false);
    }

    public void EnemyTakeDamage(int dam, string attacker)  {
        HP -= dam;
        boomBox.Play();
        if (HP <= 0)
        {
            currentAttackType = attacker;
            Debug.Log(String.Format("Attacked by Player's {0}", attacker));
            DealDeath();
        }
        Debug.Log("Took damage "+ HP +" HP left");
    }

    /// <summary>
    /// Sets the position of the enemy
    /// </summary>
    /// <param name="pos"></param>
    public virtual void SetCenter(Vector3 pos)   {
        this.transform.position = pos;
    }

    /// <summary>
    /// Changes the tag of the enemy and transforming the body into a projectile.
    /// </summary>
    public virtual void BecomeProjectile()
    {
        On_BecomeAmmo_Sent += Ammo.instance.Load;
        //transmits to Ammo handling manager as a subject of subscription
        if (On_BecomeAmmo_Sent != null)
        {
            On_BecomeAmmo_Sent();
            gameObject.transform.root.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// creates a pickUp item upon destruction if and only if
    ///the enemy did not become ammo to be shot, they are not
    ///mutually inclusive
    /// </summary>
    protected virtual void BecomePickUp()
    {
        Debug.Log("Became PickUP");
        if (randomDrop)
        {
            On_RandomLootDropped_Sent(transform.position, transform.rotation);
        }
        else if (!randomDrop)
        {
            On_DefaultLootDrop_Sent(transform.position, transform.rotation, PickupType.Health);
        }
        gameObject.transform.root.gameObject.SetActive(false);
    }
    /// <summary>
    /// Resets the location of the enemies upon reset of the 
    /// </summary>
    protected virtual void On_RestartButton_Received()
    {
        gameObject.transform.root.gameObject.SetActive(true);
        gameObject.transform.root.gameObject.transform.position = startLocation;
        HP = saveHP;

    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        
            if (obj.tag == "Projectile")
            {
            Debug.Log("Projectile HIT!!");
                EnemyTakeDamage(obj.GetComponent<Projectile>().damage, obj.tag);
            }
    }
}
#region TODO list, refactoring etc
/************TODO Refactoring********************************************************************//*
 1-
 2-
 3-
 4-
 *************************************************************************************************/
#endregion