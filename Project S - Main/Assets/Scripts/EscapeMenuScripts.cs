using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenuScripts : MonoBehaviour
{

    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    //https://www.youtube.com/watch?v=ElqAoS2FEDo

    public static bool gamePaused = false;
    public GameObject EscapeMenu; //visual part of the menu; NOT the wrapper
    public GameObject OptionsMenu; //

    public int currentMenuPosition = 1; //for keyboard navigation tracking

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Escape Menu' action
    public void OnToggleEscapeMenu(InputAction.CallbackContext value)
    {

        //TODO: determine where we put the action maps, and how we will switch them when this triggers 
        //(could/should all live on player character? or can we switch from the one on the player to the one on the menu?)
        Debug.Log("OnToggleEscapeMenu...");
        Debug.Log(value);
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputAction.CallbackContext.html
        //value.action;//get the InputAction
        //value.action.actionMap;//get the InputAction Map it belongs to;
        //value.action.actionMap.asset;//get the InputActionAsset that the Map belongs to;
        


    
        if (value.started)
        { //only do it when we initially press the button 

            //playerInput.SwitchCurrentActionMap("MenuControls");

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
        if (gamePaused)
        {
            this.EscapeMenu.SetActive(false);
            this.OptionsMenu.SetActive(true);
        }
    }

    public void QuitToMenu()
    {
        if (gamePaused)
        {

        }
    }

    public void Exitgame()
    {
        //TODO: show confirm prompt 
        if (gamePaused) Application.Quit();
    }


    //Video options
    public void OpenVideoOptions()
    {

    }
    public void CloseVideoOptions()
    {

    }


    //Audio Options
    public void OpenAudioOptions()
    {

    }
    public void CloseAudioOptions()
    {

    }

    //Keybinding/Controls Options
    public void OpenControlOptions()
    {

    }
    public void CloseControlOptions()
    {

    }

    //All other options
    public void OpenOtherOptions()
    {

    }
    public void CloseOtherOptions()
    {

    }


    //Options Back Button
    public void GoBackToEscape()
    {
        if (gamePaused)
        {
            this.EscapeMenu.SetActive(true);
            this.OptionsMenu.SetActive(false);
        }
    }

    public void GoBackToMainOptions(string from)
    {
        if (gamePaused)
        {
            //this.EscapeMenu.SetActive(false);
            this.OptionsMenu.SetActive(true);
        }
    }

    public void GoToSubOptions(string to)
    {
        if (gamePaused)
        {

            //this.EscapeMenu.SetActive(true);
            this.OptionsMenu.SetActive(false);

            // the object to examine the childs of
            //Transform parent;// gameObject.transform

            // iterate through all first level children
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Debug.Log($"The child {child.name} is active!");
                }
                else
                {
                    Debug.Log($"The child {child.name} is inactive!");
                }
            }  
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Escape Menu Scripts");
        //Debug.Log(gameObject.name );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
