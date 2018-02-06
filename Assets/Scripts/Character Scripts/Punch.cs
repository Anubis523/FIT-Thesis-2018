﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used to create the punch based on the animation state parameters
/// </summary>
[RequireComponent(typeof (CapsuleCollider))]
public class Punch : MonoBehaviour {

    [SerializeField]
    CapsuleCollider punchCapsule;

    [SerializeField]
    Animator myAnim;

    [Tooltip("The damage the punch inflicts upon an enemy or destructible obstacle")]
    [Range(0, 20)]
    public int dam = 10;
    
    private int Damage
    {
        get
        {
            return Damage;
        }
        set
        {
            Damage = dam;
        }
    }

    // Use this for initialization
    void Start () {
		if (punchCapsule == null)
        {
            punchCapsule = this.gameObject.GetComponent<CapsuleCollider>();
        }

        if (myAnim == null)
        {
            myAnim = gameObject.GetComponentInParent<Animator>();
        }
	}
     
	// Update is called once per frame
	void Update () {
        punchCapsule.enabled = myAnim.GetBool("punching"); //bool is determined by when/during the punch animation is active/on
        if (punchCapsule.enabled)
        {
            PunchAttack();
        }
    }

    void PunchAttack()
    {
        Collider [] cols = Physics.OverlapCapsule(punchCapsule.bounds.center, punchCapsule.bounds.extents, punchCapsule.radius,LayerMask.GetMask("Enemy"));
        foreach (Collider col in cols)
        {
            if (col.tag == "HurtBox")
            {
                Debug.Log("Enemy hit");
                col.gameObject.GetComponentInParent<Enemy>().TakeDamage(dam);
            }
        }
    }
}
