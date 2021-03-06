using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AnimatedGameOverGUI : Singleton<AnimatedGameOverGUI> {

    public GameObject gameOverScreen;
	public Animator menuAnimation;

    /// <summary>
    /// Event transmitted from Game Over GUI Manager upon the restarting of the level/ game loop
    /// </summary>
    public event Action On_Restart_Sent;
    /// <summary>
    /// Event transmitted from the Animated Game Over upon quit
    /// </summary>
    public event Action On_QuitOut_Sent;

    // Use this for initialization
    void Start () {
        //subscribes the event created by the 
        gameObject.SetActive(false);
        GameManager.instance.On_GameOverState_Sent += OnGameOverState;
        On_Restart_Sent += GameManager.instance.OnRestartButton;
    }

    public virtual void Quit()
    {
        //gameOverScreen.SetActive(false);
		SceneManager.LoadScene ("20180428_Start Screen", LoadSceneMode.Single);
        if (On_QuitOut_Sent != null)
        {
			menuAnimation.SetBool ("isPaused", false);
            Debug.Log("Quiting...");
            On_QuitOut_Sent();
        }

        gameObject.SetActive(false);
    }

    public virtual void Restart()
    {
        gameOverScreen.SetActive(false);

        if (On_Restart_Sent != null)
        {
			menuAnimation.SetBool ("isPaused", false);
            Debug.Log("Sending Restart GO");
            On_Restart_Sent();
            Debug.Log("Restarted GO");

        }
        gameObject.SetActive(false);
    }

    //Subscriber that only is triggered by the initiation of the gameOver state change 
    public virtual void OnGameOverState()
    {
        menuAnimation.SetBool("isPaused", true);
        gameObject.SetActive(true);
        gameOverScreen.SetActive(true);
    }
}
