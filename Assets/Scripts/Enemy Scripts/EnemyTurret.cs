﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enum used to determine the direction the turret is facing
/// </summary>
//public enum Facing { Left = -1, Right = 1};

public class EnemyTurretGo : MonoBehaviour {
    #region Variables Field

    //sets default direction turret faces
    [SerializeField]
    GameObject bullet;

    //in reference to current enemy
    Enemy myEnemy;
    EnemyBehavior thisBehavior;
    //used ast multiplier to affect direction projectile is shot in
    int direction;

    //bool set true when enemy behavior is turret as well
    bool turretOn = false;

    [Tooltip("Time between turns of turret")]
    [Range(0.3f , 3f)]
    public float turretInterval;

    [Tooltip("Number of shots fired between turret turn intervals")]
    [Range(1 , 6)]
    public int shotsPerInterval;

    [Tooltip("Strength of force with which the projectile is shot")]
    [Range(1f, 3000f)]
    public float throwForce;

    #endregion
    /// <summary>
    /// The constructor for the EnemyTurret
    /// </summary>
    public EnemyTurretGo(float switchInterval, int shotsPer, float shotForce)
    {
        this.turretInterval = switchInterval;
        this.shotsPerInterval = shotsPer;
        this.throwForce = shotForce;
    }

    // Use this for initialization
    void Start ()
    {
        thisBehavior = gameObject.GetComponent<Enemy>().enBehavior;
        myEnemy = this.gameObject.GetComponent<Enemy>();
        StartCoroutine(TurretPatrol());
    }

    //if the subscribing method that has event passed on that carries the behavior enum that then starts or stops the coroutine and changes the turretOn bool
    public void PatrolBehave(EnemyBehavior obj)
    {
        if (obj != EnemyBehavior.Turret)
        {
            turretOn = false;
           // StopCoroutine(TurretPatrol());
        } 
        else
        {
            turretOn = true;
           // StartCoroutine(TurretPatrol());
        }
    }

    /// <summary>
    /// Coroutine that governs the logic that fires from the turret at regular intervals a nominal number of times
    /// </summary>
    /// <returns></returns>
    IEnumerator TurretPatrol ()
    {
        Debug.Log("Turret Patrol starts and turretOn is "+turretOn);
        float intervals = shotsPerInterval / turretInterval;

        while (true)
        {
            for (int i = shotsPerInterval; i > 0; i++)
            {
                //Debug.Log("Entering the For Loop");
                Fire();
                yield return new WaitForSecondsRealtime(intervals);
            }
            Vector3 reScale = this.gameObject.transform.localScale;
            reScale.x *= -1;
            this.gameObject.transform.localScale = reScale;
        }
    }

    //fires projectile
    protected virtual void Fire()
    {
        // GameObject shot = Instantiate(bullet, myEnemy.transform.position, Quaternion.identity);
        // shot.GetComponent<Rigidbody>().AddForce(Vector3.right * direction * throwForce);
        Debug.Log("Pew Pew");
    }
}
