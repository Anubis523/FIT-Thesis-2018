using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	//variables for movement
	public float runSpeed;

    /// <summary>
    /// Physics component for this object
    /// </summary>
	private Rigidbody myRB;
    /// <summary>
    /// Animation controller component for this object
    /// </summary>
	private Animator myAnim;
    /// <summary>
    /// Is the player facing right
    /// </summary>
	private bool facingRight;
    /// <summary>
    /// Direction
    /// </summary>
	private int dir;



	// Use this for initialization
	private void Start () {
		myRB = GetComponent <Rigidbody>();
		myAnim = GetComponent <Animator>();
		facingRight = true;


	}
	
	// Update is called once per frame
	private void Update () {
		//Debug print out to verify outputs/ data
//		Debug.Log ( transform.localRotation.y +" Degrees at Y Axis");
	}

	//depends upon physics objects
	private void FixedUpdate(){
		float move = Input.GetAxis("Horizontal");
		myAnim.SetFloat ("speed", Mathf.Abs (move));

		myRB.velocity = new Vector3 (move * runSpeed, myRB.velocity.y,0);

		if (move > 0 && !facingRight) {
			dir = -1;
			Flip ();
		} else if (move < 0 && facingRight) {
			dir = 1;
			Flip ();
		}
	

	}

    /// <summary>
    /// Flip the player between facing right and facing left
    /// </summary>
	private void Flip() {
		facingRight = !facingRight;
		transform.Rotate (0.0f, 90f*dir, 0.0f);

		// Z scale flip for pos to neg or vice versa may create problems later on be carefull!!
				//** 	ALTERNATIVE FLIP - uses scale flip but can cause problems with other math-based calculations to come
		//			Vector3 theScale = transform.localScale;
		//			theScale.z *= -1;
		//			transform.localScale = theScale;
	}
}