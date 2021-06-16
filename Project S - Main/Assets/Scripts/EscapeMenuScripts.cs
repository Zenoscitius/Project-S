using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
//using static UserSettings; 

public class EscapeMenuScripts : MonoBehaviour
{
    private Resolution[] resolutions;
    private int curSelectedResIndex; 

    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    //https://www.youtube.com/watch?v=ElqAoS2FEDo

    public static bool gamePaused = false;
    private GameObject EscapeMenu; //visual part of the menu; NOT the wrapper
    private GameObject OptionsMenu; //
    private GameObject VideoOptionsMenu;
    private GameObject AudioOptionsMenu;
    private GameObject ControlsOptionsMenu;
    private GameObject OtherOptionsMenu;

    private GameObject currentActiveMenu; //which of the above menus is presently active, null if none


    public TMP_Dropdown resolutionsDropdown;


    public GameObject playerCharacter; //added in since it seems the objects passed via the events are actually clones so cant access real action maps...

    public int currentMenuPosition = 1; //for keyboard navigation tracking


    private PlayerInput menuInputs = null;
    private PlayerInput playerInputs = null;

    //private  navigationTree;

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
            //if(EscapeMenu.activeSelf)
            //{
                Debug.Log("Open the Escape menu!");
                PauseGame();
            //}

        }
    }

    public void OnGoBackMenuLevel(InputAction.CallbackContext value)
    {
        if (value.started)
        { //only do it when we initially press the button 

            //playerInput.SwitchCurrentActionMap("MenuControls");   
            GoBackMenuLevel();
        }
    }

    public void GoBackMenuLevel()
    {
        if (!gamePaused) return;

        if (this.currentActiveMenu == this.EscapeMenu)
        {
            Debug.Log("Close the Escape menu!");
            ResumeGame();
        }
        else if (this.currentActiveMenu == this.OptionsMenu)
        {
            Debug.Log("Close the Options menu!");
            NavigateToMenu("EscapeMenu");
        }
        else if (this.currentActiveMenu == this.VideoOptionsMenu)
        {
            NavigateToMenu("OptionsMenu");
        }
        else if (this.currentActiveMenu == this.AudioOptionsMenu)
        {
            NavigateToMenu("OptionsMenu");
        }
        else if (this.currentActiveMenu == this.ControlsOptionsMenu)
        {
            NavigateToMenu("OptionsMenu");
        }
        else if (this.currentActiveMenu == this.OtherOptionsMenu)
        {
            NavigateToMenu("OptionsMenu");
        }
        else
        {
            Debug.Log("Unhandled case!");
        }
    }

    public void NavigateToMenu(string targetMenu)
    {
        if (!gamePaused) return;

            this.currentActiveMenu.SetActive(false);
        if (targetMenu == "EscapeMenu")
        {
            this.currentActiveMenu = this.EscapeMenu;
        }
        else if (targetMenu == "OptionsMenu")
        {
            this.currentActiveMenu = this.OptionsMenu;
        }
        else if (targetMenu == "VideoOptionsMenu")
        {
            this.currentActiveMenu = this.VideoOptionsMenu;
        }
        else if (targetMenu == "AudioOptionsMenu")
        {
            this.currentActiveMenu = this.AudioOptionsMenu;
        }
        else if (targetMenu == "ControlsOptionsMenu")
        {
            this.currentActiveMenu = this.ControlsOptionsMenu;
        }
        else if (targetMenu == "OtherOptionsMenu")
        {
            this.currentActiveMenu = this.OtherOptionsMenu;
        }
        else
        {
            Debug.Log("Unhandled case!");
        }

        this.currentActiveMenu.SetActive(true);


        //// iterate through all first level children
        //foreach (Transform child in gameObject.transform)
        //{
        //    if (child.gameObject.activeSelf)
        //    {
        //        Debug.Log($"The child {child.name} is active!");
        //    }
        //    else
        //    {
        //        Debug.Log($"The child {child.name} is inactive!");
        //    }
        //}

    }


    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gamePaused = true;
        this.EscapeMenu.SetActive(true);
        this.currentActiveMenu = this.EscapeMenu;
        Time.timeScale = 0f; //note Update() continues to run; FixedUpdate doesnt get called at all 
        this.playerInputs.currentActionMap.Disable();
        this.menuInputs.currentActionMap.Enable();
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        gamePaused = false;
        this.EscapeMenu.SetActive(false);
        this.currentActiveMenu = null;
        Time.timeScale = 1f;

        this.playerInputs.currentActionMap.Enable();
        this.menuInputs.currentActionMap.Disable();
    }
    
    public void QuitToMainMenu()
    {
        if (gamePaused)
        {
            //TODO: show confirm prompt 
            if (gamePaused) Application.Quit();
        }
    }

    public void Exitgame()
    {
        //TODO: show confirm prompt; maybe should be callable without being paused?
        if (gamePaused)
        {
            Application.Quit();
        }
    }


    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    

    public void OnButtonHoverIn(BaseEventData eventData)
    {
        //Debug.Log("Hover In");
        GameObject selectedButton = eventData.selectedObject;
        Debug.Log(selectedButton);
        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(255, 255, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);
    }
    public void OnButtonHoverOut(BaseEventData eventData)
    {
        //Debug.Log("Hover Out");
        GameObject selectedButton = eventData.selectedObject;
        Debug.Log(selectedButton);
        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(0, 218, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);
    }

    public void SetResolutionOptions()
    {
        List<string> validDisplayOptions = new List<string>();

        //TODO: decide how to handle refresh rates, framerate caps, vsync,
        for (int i=0; i < resolutions.Length; i++)
        {
            string optionString = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            validDisplayOptions.Add(optionString);

            //if this is the currently in use resolution, note so
            //if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.refreshRate)



            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                curSelectedResIndex = i;
            }
        }

        validDisplayOptions.Reverse();
        curSelectedResIndex = validDisplayOptions.Count - (curSelectedResIndex + 1); //correct index

        //fix the resolution list display 
        this.resolutionsDropdown.ClearOptions();
        this.resolutionsDropdown.AddOptions(validDisplayOptions);
        this.resolutionsDropdown.value = curSelectedResIndex;
        this.resolutionsDropdown.RefreshShownValue(); //just in case?
    }


    public void OnResolutionSelect(int eventData)
    {
        Debug.Log("Resolution Selected: " + eventData);
        int resArrayIndex = resolutions.Length - (eventData + 1); //correct the visual array index vs actual index
        SetResolution(resArrayIndex);

        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(255, 255, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);

       
    }

    public void SetResolution( int resArrayIndex)
    {
        Debug.Log("Resolution Selected at Index: " + resArrayIndex);
        curSelectedResIndex = resArrayIndex;
        Resolution targetRes = resolutions[resArrayIndex];
        //Screen.SetResolution(targetRes.width, targetRes.height, Screen.fullScreen, targetRes.refreshRate);

        //UserSettings.Instance.isFullscreen
        //TODO: save the user setting change to the settings file 
        //TODO: have a popup that autoreverts if not confirmed 

        //update the actual settings
        UserSettings.Instance.UpdateResolution(targetRes);
        //UserSettings.Instance.currentResolution = Screen.currentResolution;
        UserSettings.Instance.SaveUserSettingsToFile();
    }


    public void OnUpdateVsync(bool newValue)
    {
        UserSettings.Instance.isVsynced = newValue;
        UserSettings.Instance.SaveUserSettingsToFile();
    }

    public void OnUpdateWindowed(bool newValue)
    {
        UserSettings.Instance.isFullscreen = !newValue;
        UserSettings.Instance.SaveUserSettingsToFile();
    }
    public void OnUpdateResizable(bool newValue)
    {
        UserSettings.Instance.isResizable = newValue && !UserSettings.Instance.isFullscreen;
        UserSettings.Instance.SaveUserSettingsToFile();
    }

    public void UpdateBooleanOption(bool newValue)
    {
        
    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Escape Menu Scripts");

        Debug.Log($"temp init spot for userSettings {UserSettings.Instance}");
        //Debug.Log(UserSettings.Instance);


        //TODO setup a data structure that holds the hierarchy information so its more generalized
        //Otherwise turns these back into public vars that you use the inspector to assign 
        //https://docs.unity3d.com/ScriptReference/Transform.Find.html , if we make the structure hierarchical, we will need to add the /'s
        this.EscapeMenu = gameObject.transform.Find("EscapeMenu").gameObject;
        this.OptionsMenu = gameObject.transform.Find("OptionsMenu").gameObject;
        this.VideoOptionsMenu = gameObject.transform.Find("VideoOptionsMenu").gameObject;
        //this.AudioOptionsMenu = gameObject.transform.Find("AudioOptionsMenu").gameObject;
        //this.ControlsOptionsMenu = gameObject.transform.Find("ControlsOptionsMenu").gameObject;
        //this.OtherOptionsMenu = gameObject.transform.Find("OtherOptionsMenu").gameObject;

        //disable/hide the menus 
        this.EscapeMenu.SetActive(false);
        this.OptionsMenu.SetActive(false);
        this.VideoOptionsMenu.SetActive(false);
        //this.AudioOptionsMenu.SetActive(false);
        //this.ControlsOptionsMenu.SetActive(false);
        //this.OtherOptionsMenu.SetActive(false);

        //setup inputs
        this.menuInputs = GetComponent<PlayerInput>();//grab reference to menu input controller
        this.playerInputs = playerCharacter.GetComponent<PlayerInput>(); //grab reference to player input controller
        this.menuInputs.currentActionMap.Disable(); //we need it to start disable atm since player is what has initial control (until we make more changes probably)

        //Debug.Log(this.menuInputs);
        //Debug.Log(this.playerInputs);


        //let unity get the valid detected screen resolutions (only works from exe not in editor allegedly) 
        this.resolutions =  Screen.resolutions;
        SetResolutionOptions();
        //this.resolutionsDropdown = VideoOptionsMenu.transform.Find("ResolutionsDropdown").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
