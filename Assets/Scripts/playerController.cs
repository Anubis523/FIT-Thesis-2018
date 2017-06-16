using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	//variables for movement
	public float runSpeed;
	public float drag;

	Rigidbody myRB;
	Animator myAnim;
	bool facingRight;
	int dir;

	//for jumping player starts suspended above ground by default is not on ground
	public float jumpHeight;

	// Use this for initialization
	void Start () {
		myRB = GetComponent <Rigidbody>();
		myAnim = GetComponent <Animator>();

		//these particular settings brute force initial conditions as being airborne and not on the ground
		myAnim.SetBool ("grounded", false);
		facingRight = true;
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
		if (Input.GetKey("j")) {
			Jump ();		
		}


		//lateral movement
		float move = Input.GetAxis("Horizontal");
		myAnim.SetFloat ("speed", Mathf.Abs (move));
		if (myAnim.GetBool ("grounded") == true) 
			myRB.velocity = new Vector3 (move * runSpeed, myRB.velocity.y, 0);
		
		//simulates lack of mobility when falling can move just at a reduced speed laterally
		else 
			myRB.velocity = new Vector3 (move * drag, myRB.velocity.y, 0);
		


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
		transform.Rotate (0.0f, 90f*dir, 0.0f);}

	void Jump (){
		if (myAnim.GetBool("grounded")==true) {
			myAnim.SetBool ("grounded", false);
			myRB.velocity = myRB.velocity + new Vector3 (0, jumpHeight, 0);
		}

		}
	void OnCollisionEnter (Collision collision){
		myAnim.SetBool ("grounded", true);
	}

	void OnCollisionExit (Collision collision){
		myAnim.SetBool ("grounded", false);
	}
	//random commenting to make gitHub work
	void Punch ()	{}

	void Slam ()	{}
}