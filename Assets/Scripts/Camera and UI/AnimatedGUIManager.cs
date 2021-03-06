using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Houses logic, functions, etc. for the pause menu, game over, etc
/// </summary>
public class AnimatedGUIManager : Singleton<AnimatedGUIManager>
{
    public GameObject pauseMenu;

    bool visible;   //determines visibility of certain menu items
	public Animator menuAnimation;

    /// <summary>
    /// Event transmitted from GUI Manager when pause button is pressed
    /// </summary>
    public event Action On_PauseButton_Sent;

    /// <summary>
    /// Event transmitted from GUI Manager when pause is exited and game is set to resume
    /// </summary>
    public event Action On_ResumeButton_Sent;

    /// <summary>
    /// Event transmitted from GUI Manager upon the restarting of the level/ game loop
    /// </summary>
    public event Action On_RestartButtonPauseMenu_Sent;

    protected virtual void Start()
    {
        //subscribes the event created by the 
        GameManager.instance.On_GameOverState_Sent += OnGameOverState;
        GameManager.instance.On_PauseState_Sent += On_PauseState_Received;
        //gameObject.SetActive(false);
    }

    private void On_PauseState_Received()
    {
        gameObject.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public virtual void Continue()
    {
        Debug.Log("RESUME");
        visible = false;
        On_ResumeButton_Sent();
    }

    //Subscriber that only is triggered by the initiation of the gameOver state change 
    public virtual void OnGameOverState()
    {
        gameObject.SetActive(false);
    }

    /*Restart logic needs to reside in the Game Manager you only need to worry 
    about type void methods that uses a subscribed event as input or a trigger
    Mind you this is still a WIP gimme a minute... <Kev Note*/
    public virtual void Restart()
    {
        visible = false;
        if (On_RestartButtonPauseMenu_Sent != null)
        {
            Debug.Log("Sending Restart");
            On_RestartButtonPauseMenu_Sent();
            Debug.Log("Restarted");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        /*checks the gameState and the button presses to make sure things work 
         * dependent of the current game state of the GameManager <Kev Note*/

        if (ButtonPressed("Start") && GameManager.instance.GetState() == GameState.inGame && On_PauseButton_Sent != null)
        {
            //Debug.Log("Start was pressed.");
            On_PauseButton_Sent();
            visible = true;
			menuAnimation.SetBool ("isPaused", true);
        }
        else if (ButtonPressed("Start") && GameManager.instance.GetState() == GameState.pause && On_PauseButton_Sent != null)
        {
            Continue();
            visible = false;
			menuAnimation.SetBool ("isPaused", false);
        }

        pauseMenu.SetActive(visible);
    }

    protected virtual bool ButtonPressed(string button)
    {
        bool pressed;
        return pressed = (Input.GetButtonDown(button)) ? true : false;
    }
}
#region TODO list, refactoring etc
/************TODO Refactoring********************************************************************//*
 1-
 2-
 3-
 4-
 *************************************************************************************************/
#endregion