﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enum used to determine the direction the turret is facing
/// </summary>

//public enum Facing { Left = -1, Right = 1};

public class EnemyTurretGo : MonoBehaviour
{
 
    //rigid body of the parent enemy element to be used to determine direction of shot
    Rigidbody myRb;
    //commented out bullets array used for the grabbable and non-grabbable ammo types to be tested later

    //the empty game object used to launch bullets from
    public GameObject gunBarrel;
    public GameObject bullet;
    public GameObject poolManager;

    PoolItem poolBullets;   //the pooled bullets

    int directionModifier = 1;

    //the number of shots that can be fired per second
    int fullClip = 0;

    //not in use atm, for the fire rate in seconds or 5 times a second .2 * 1.000 seconds
    [Range(0, 2)]
    public float fireRate = 0.2f;

    [Range(0, 100)]
    public float fireForce = 30f;

    [Tooltip("The percentage of shots per fire cycle that are grabbable")]
    [Range(0, 100)]
    public int percentage = 50;

    float percentFloat = .5f;

    // Use this for initialization
    protected virtual void Start()
    {
        percentFloat = percentage * 0.01f;  //converts percentage whole number to float form for weighted generation of bullets

        if (myRb != null)
        {
            return;
        }
        else
        {
            myRb = gameObject.GetComponent<Rigidbody>(); //to be used to determine direction which should be determined in fixed update
        }

        fullClip = Mathf.RoundToInt(1 / fireRate);

        poolBullets = new PoolItem(fullClip, bullet);
        ChangeBullets();
    }

    private void ChangeBullets()    //changes the kind of bullets the turret fires based on a percentage
    {
        int changedBullets = Mathf.FloorToInt(percentFloat * poolBullets.Count());
        List<int> previousIndexes = new List<int>();
        int randomIndex;

        for (int i = changedBullets; i >= 0; i--)
        {
            do
            {
                randomIndex = UnityEngine.Random.Range(0, poolBullets.Count());
            } while (previousIndexes.Contains(randomIndex));

            poolBullets.GetAtIndex(randomIndex).GetComponent<EnemyBulletTest>().SetGrabbable(true);
            previousIndexes.Add(randomIndex);

        }
    }

    private void Update()
    {
        percentFloat = percentage * 0.01f;  //converts percentage whole number to float form for weighted generation of bullets

        if (poolBullets.Count() <= 0)
        {
            StartCoroutine(WaitToReload());
            poolBullets.ReUse();
        }
    }

    void FireFix() //using pooled objects to fire projectiles
    {
        GameObject temp = poolBullets.Get(gunBarrel.transform.position);
        temp.GetComponent<Rigidbody>().velocity = Vector3.right * directionModifier * fireForce;
    }

    IEnumerator FireRoute()
    {
        for (int i = 0; i < poolBullets.Count(); i++)
        {
            FireFix();
            yield return new WaitForSecondsRealtime(.5f);
        }

        yield return null;
    }

    IEnumerator WaitToReload()
    {
        yield return new WaitForSecondsRealtime(.75f);
    }

    IEnumerator StopFire()
    {
        for (int i = 0; i < poolBullets.Count(); i++)
        {
            poolBullets.ReUse();
        }
        yield return null;
    }

    private void FixedUpdate()
    {
        //this if-else block deteremines a directional modifier for the launch of projectiles
        if (myRb.velocity.x > 0)
        {
            directionModifier = 1;
        }
        else if (myRb.velocity.x < 0)
        {
            directionModifier = -1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject + " collider detected");
        if (other.tag == "Player")
        {
            if (!poolBullets.IsEmpty())
            {
                StartCoroutine(FireRoute());    //fires when player is detected
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(StopFire()); //reloads when not detected
        }
    }

    /****************************************TODO************************************************/
    /*
     *3.5) add a isEmpty method to the pool item class to set reloads 
      4) make all this conditional upon if the enemy detects a certain tagged object of colllider
      5) make some particle traces for the bullets
      6) figure out the grabbable projectiles
      7) figure out how to share ammo between enemies based on proximity and activity levels
      */
    /****************************************TODO************************************************/
}
