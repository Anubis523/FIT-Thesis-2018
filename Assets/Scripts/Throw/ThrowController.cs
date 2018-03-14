﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
/// <summary>
///	Takes input for ThrowModel 
/// </summary>
public class ThrowController : Controller
{
   ThrowModel canon;

    protected virtual void Start()
    {
        if (canon == null)
        {
            canon = this.GetComponent<ThrowModel>();
        }
    }

    protected virtual void FixedUpdate()
    {
        /*Uses Throw to trigger a branching nest of conditionals that will either throw a projectile
         at an angle or straight, or returns a debug log*/
         if (GameManager.instance.GetState() == GameState.inGame)
        {
            if (Input.GetButtonDown("Throw") && !Input.GetButton("Aim") && Ammo.instance.bullets > 0)
            {
                try
                {
                    canon.ThrowStraight();
                }
                catch (IndexOutOfRangeException e)
                {
                    throw new Exception("Out of Ammo. "+e.Message);
                }
            }

            else if (Input.GetButtonDown("Throw") && Input.GetButton("Aim") && Ammo.instance.bullets > 0)
            {
                canon.ThrowAngle();
            }
        }
    }
}