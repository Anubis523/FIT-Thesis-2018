﻿using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// New and Improved consolidated Player Controller that handles movements, jumps, attacks and whatnot.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public PlayerData data;
    PlayerHealth myHealth;

    //rigidboday and animator children of the player character 
    Animator myAnim;
    Rigidbody myRB;
    public static bool facingRight;
    //respawn position
    Vector3 respawnPos;
    //respawn rotation
    Quaternion rot;

    /// <summary>
    /// keeps position to be referred to outside
    /// </summary>
    public static Vector3 myPos;
    /// <summary>
    /// Delegates used to speed up runtime of checking the parameters and buttons of a resulting
    /// Animation/Action
    /// </summary>
    public delegate bool ActionTaken(String button, String state, bool flag, Animator myAnim);

    /// <summary>
    /// Combo Action taken is simply a a two-input version of Action Taken
    /// </summary>
    public delegate bool ComboActionTaken(String button1, String button2, String state, bool flag, Animator myAnim);

    //have the delegates equal two specific generic functions that produce bools based on input and animator parameters
    ActionTaken takeAction = ActionTook;
    ComboActionTaken comboAction = ComboActionTake;

    //establishes defaults and initiates variables/placeholders
    public void Awake()
    {
        myAnim = this.GetComponent<Animator>();
        myRB = this.GetComponent<Rigidbody>();
        facingRight = true;
        myAnim.SetBool("grounded", false);
        respawnPos = myRB.transform.position;
        rot = myRB.transform.rotation;
        myHealth = this.gameObject.GetComponent<PlayerHealth>();
    }


    public void Update()
    {
        myPos = myRB.transform.position;

    }

    // Update is called once per physics action
    void FixedUpdate()
    {

        bool undead = takeAction("Respawn", "dead", true, myAnim);
        //if false death-state is on all other actions cease
        if (!myAnim.GetBool("dead"))
        {
            //float move = Input.GetAxis("Horizontal");
            //myAnim.SetFloat("speed", Mathf.Abs(move));
            //myRB.velocity = new Vector3(move * data.runSpeed, myRB.velocity.y, 0);

            //float otherMove = Input.GetAxis("JoystickHorizontal");
            //myAnim.SetFloat("speed", Mathf.Abs(otherMove));
            ////Debug.Log("Joystick Horizontal at " + otherMove);
            //myRB.velocity = new Vector3(otherMove * data.runSpeed, myRB.velocity.y, 0);

            ////Changes direction if either move velocity and facing direction are at odds
            //if (!facingRight)
            //{
            //    if (move > 0 || otherMove > 0) { Flip(); }
            //}
            //else if (facingRight)
            //{
            //    if (move < 0 || otherMove < 0) { Flip(); }
            //}

            //ATTENTION!! it half works it won't walk or turn left wierd huh??
            float move = Mover(Input.GetAxis("Horizontal"), Input.GetAxis("JoystickHorizontal"));
            myAnim.SetFloat("speed", Mathf.Abs(move));
            myRB.velocity = new Vector3(move * data.runSpeed, myRB.velocity.y, 0);
            if (move > 0 && !facingRight) { Flip(); }
            else if (move < 0 && facingRight) { Flip(); }


            bool puncher = takeAction("Punch", "grounded", true, myAnim);

            if (puncher)
            {
                StartCoroutine(HitStopperPunch());
            }
            bool slammer = comboAction("Punch", "Slam", "airborne", true, myAnim);

            if (slammer)
            {
                myAnim.SetBool("airborne", false);
                myAnim.SetBool("slam", true);
            }
            else if (Input.GetAxisRaw("JoystickVertical") == 1 && Input.GetButton("Punch"))
            {
                myAnim.SetBool("airborne", false);
                myAnim.SetBool("slam", true);
            }

        }

        else if (undead)
        {
            reSpawn();
            myHealth.currentHP = PlayerHealth.maxHP;
        }
    }

    //funnels the digital/analog horizontal axis inputs
    float Mover (float digital, float analog)
    {
        float mover = 0;
        if (digital > 0) { mover = digital;}
        else if (analog > 0) { mover = analog; }
        return mover;
    }
   
    /// <summary>
    /// Returns bool based on the button press, and animState ==flag
    /// </summary>

    public static bool ActionTook(String button, String state, bool flag, Animator myAnim)
    {
        bool acted;
        acted = (Input.GetButtonDown(button) && myAnim.GetBool(state) == flag) ? true : false;
        return acted;
    }

    /// <summary>
    /// Returns bool based on dual buttons pressed and AnimState ==flag
    /// </summary>
    public static bool ComboActionTake(String button1, String button2, String state, bool flag, Animator myAnim)
    {
        bool acted;
        acted = (Input.GetButton(button1) && Input.GetButton(button2) && myAnim.GetBool(state) == flag);
        return acted;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }

    //false respawn, resets transposition
    public virtual void reSpawn()
    {
        transform.SetPositionAndRotation(respawnPos, rot);
        myAnim.SetBool("dead", false);
        myAnim.SetBool("grounded", true);
        facingRight = true;
    }

    public void die()
    {
        transform.Rotate(0, 0, 90);
        myAnim.SetBool("dead", true);
    }

    //hitStop coroutine for punch to hold and then stop animation
    IEnumerator HitStopperPunch()
    {
        myAnim.SetBool("punching", true);
        //find better way to hitStop on the punch its jaggy atm
        yield return new WaitForSeconds(myAnim.GetCurrentAnimatorStateInfo(0).length / 5f);
        //stops movement while punch is animated restarts on end
        myAnim.SetBool("punching", false);
    }
    public float airAttackWaitTime = .3f;

    //declares slam bool false upon ground contact resetting its anim state
    void OnCollisionEnter(Collision collision)
    {
        myAnim.SetBool("slam", false);
        myAnim.SetBool("airborne", false);
        myAnim.SetBool("grounded", true);

        if (collision.gameObject.tag == "Ground")
        {
            myAnim.SetBool("grounded", true);
            myAnim.SetBool("airborne", false);
        }

        else if (collision.gameObject.tag == "Death Object")
        {
            die();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (Mathf.Abs(myRB.velocity.y) > 0 || collision.gameObject.tag=="Ground"
            )
        {
            myAnim.SetBool("grounded", false);
            myAnim.SetBool("airborne", true);
        }
    }
}


