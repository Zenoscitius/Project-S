using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMenuScripts : MonoBehaviour
{

    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    //https://www.youtube.com/watch?v=ElqAoS2FEDo

    public static bool gamePaused = false;
    public GameObject EscapeMenu; //visual part of the menu; NOT the wrapper
    public GameObject OptionsMenu; //
    public GameObject playerCharacter; //added in since it seems the objects passed via the events are actually clones so cant access real action maps...

    public int currentMenuPosition = 1; //for keyboard navigation tracking


    private PlayerInput menuInputs = null;
    private PlayerInput playerInputs = null;

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Escape Menu' action
    //We want to open up the pause menu, pause the game, and switch control from the player over to the Menu exclusively 
    public void OnToggleEscapeMenu(InputAction.CallbackContext value)
    {
        //TODO: determine where we put the action maps, and how we will switch them when this triggers 
        //(could/should all live on player character? or can we switch from the one on the player to the one on the menu?)
        //PlayerInput mainInput = GetComponent<PlayerInput>();
        Debug.Log("OnToggleEscapeMenu...");
        //Debug.Log(value.action.actionMap);
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputAction.CallbackContext.html
        //
        //value.action;//get the InputAction
        //value.action.actionMap;//get the InputAction Map it belongs to;
        //value.action.actionMap.asset;//get the InputActionAsset that the Map belongs to;

        //InputActionMap MenuControlsMap = value.action.actionMap.asset.FindActionMap("MenuControls", true);
        //InputActionMap PlayerControlsMap = value.action.actionMap.asset.FindActionMap("PlayerControls", true);
        //Debug.Log(PlayerControlsMap); //seems to be a clone
        //Debug.Log(MenuControlsMap); //seems to be a clone 
        //PlayerControlsMap.Disable();
        //MenuControlsMap.Enable();

        if (value.started)
        { //only do it when we initially press the button 

            //playerInput.SwitchCurrentActionMap("MenuControls");
            Debug.Log("Toggle the Escape menu!");
 
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();    
            }
        }
    }

    public void OnOpenEscapeMenu(InputAction.CallbackContext value)
    {
        if (value.started)
        { //only do it when we initially press the button 

            //playerInput.SwitchCurrentActionMap("MenuControls");
            Debug.Log("Open the Escape menu!");

            //if (gamePaused)
            //{
            //    ResumeGame();
            //}
            //else
            //{
                PauseGame();
            //}
        }
    }

    public void OnCloseEscapeMenu(InputAction.CallbackContext value)
    {
        if (value.started)
        { //only do it when we initially press the button 

            //playerInput.SwitchCurrentActionMap("MenuControls");
            Debug.Log("Close the Escape menu!");

            //if (gamePaused)
            //{
                ResumeGame();
            //}
            //else
            //{
            //    PauseGame();
            //}
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gamePaused = true;
        this.EscapeMenu.SetActive(true);
        Time.timeScale = 0f; //note Update() continues to run; FixedUpdate doesnt get called at all 

        this.playerInputs.currentActionMap.Disable();
        this.menuInputs.currentActionMap.Enable();
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        gamePaused = false;
        this.EscapeMenu.SetActive(false);
        Time.timeScale = 1f;


        this.playerInputs.currentActionMap.Enable();
        this.menuInputs.currentActionMap.Disable();
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


    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/

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


    public void OnButtonHoverIn(BaseEventData eventData)
    {
        Debug.Log("Hover In");
        
        Debug.Log(eventData);
        //Image buttonBackground = eventData.selectedObject.GetComponent<Image>();
        //buttonBackground.color = new Color(255, 255, 255, 1);
    }
    public void OnButtonHoverOut(BaseEventData eventData)
    {
        Debug.Log("Hover Out");
        Debug.Log(eventData.selectedObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Escape Menu Scripts");
        //Debug.Log(gameObject.name );
        this.menuInputs = GetComponent<PlayerInput>();//grab reference to menu input controller
        this.playerInputs = playerCharacter.GetComponent<PlayerInput>(); //grab reference to player input controller
        this.OptionsMenu.SetActive(false); //hide menus
        this.EscapeMenu.SetActive(false); //hide menus
        this.menuInputs.currentActionMap.Disable(); //we need it to start disable atm since player is what has initial control (until we make more changes probably)
        //Debug.Log(this.menuInputs);
        //Debug.Log(this.playerInputs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
