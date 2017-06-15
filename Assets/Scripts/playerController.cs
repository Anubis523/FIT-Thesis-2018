using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	//variables for movement
	public float runSpeed;

	Rigidbody myRB;
	Animator myAnim;
	bool facingRight;
	int dir;

	//for jumping player starts suspended above ground by default is not on ground
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;

	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody>();
		myAnim = GetComponent <Animator>();
		facingRight = true;
		grounded = false;


	}
	
	// Update is called once per frame gets expensive
	void Update () {
		//Debug print out to verify outputs/ data
//		if (Input.anyKey) {
//			Debug.Log (Input.inputString);
//		}
	}

	//depends upon physics objects
	void FixedUpdate(){
		//this works but the in air or airborne cycle does not work it strobes
		if (grounded && Input.GetKey("j")) {
			grounded = false;
			myAnim.SetBool ("grounded",grounded);
			myRB.velocity = myRB.velocity + new Vector3 (0,jumpHeight,0);


		}
			//need to resolve conditional for the isGrounded or not so jumps and such are more reliable
//		if (myRB.position.y < -0.01) 
//			grounded = true;
//		else     
//			grounded = false;
//
		myAnim.SetBool ("grounded", grounded);

		//lateral movement
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

	void Flip() {
		facingRight = !facingRight;
		transform.Rotate (0.0f, 90f*dir, 0.0f);

	}

	void OnCollionStay (){
		grounded = true;
	}
}