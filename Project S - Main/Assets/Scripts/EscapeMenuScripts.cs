using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenuScripts : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject EscapeMenu; //visual part of the menu; NOT the wrapper

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Escape Menu' action
    public void OnToggleEscapeMenu(InputAction.CallbackContext value)
    {
        //return;//temporarily disable to see what the problem with the core systems are
        if (value.started)
        { //only do it when we initially press the button 

            if (gamePaused) ResumeGame();
            else PauseGame();
            Debug.Log("Toggle the Escape menu!");
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        this.EscapeMenu.SetActive(true);
        Time.timeScale = 0f; //note Update() continues to run; FixedUpdate doesnt get called at all 
    }

    public void ResumeGame()
    {
        gamePaused = false;
        this.EscapeMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadOptions()
    {
        if (gamePaused) ;
    }

    public void QuitToMenu()
    {
        if (gamePaused) ;
    }

    public void Exitgame()
    {
        if (gamePaused) Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
