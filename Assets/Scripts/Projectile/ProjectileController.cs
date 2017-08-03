﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Projectile controller class checks for input to throw
/// projectile
/// </summary>
public class ProjectileController : Controller {

	[SerializeField]
	private Projectile model;

	protected virtual void Awake () {
		//check to see if model variable is empty
		if (!model) {
			model = this.gameObject.GetComponent<Projectile>();
		}
		//check to see if projectile is there
	}

	// Update is called once per frame
	protected virtual void Update () {
        //on hold of throw button 
        if (Input.GetButton("Grab"))
        {
            if (!model.hasAmmo())
                Debug.Log("Empty Clip");
            else
                Debug.Log("Holding");
        }
        else if (Input.GetButtonUp("Grab") && !model.hasAmmo())
        {
               Debug.Log("Empty Clip, Can");
        }
        else if (Input.GetButtonUp("Grab") && model.hasAmmo())
        {
            Debug.Log("Throwing");
            model.shootLoad();
        }
        }

}
    
