using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;
//using static UserSettings; 

public class EscapeMenuScripts : MenuScripts
{
    public static bool gamePaused = false;

    private Resolution[] resolutions;
    private int curSelectedResIndex; 

    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    //https://www.youtube.com/watch?v=ElqAoS2FEDo
 
    private GameObject EscapeMenu; //visual part of the menu; NOT the wrapper
    private GameObject OptionsMenu; //
    private GameObject VideoOptionsMenu;
    private GameObject AudioOptionsMenu;
    private GameObject ControlsOptionsMenu;
    private GameObject OtherOptionsMenu;

    private GameObject currentActiveMenu; //which of the above menus is presently active, null if none

    public TMP_Dropdown resolutionsDropdown;

    //public GameObject controlBinderPrefab;

    public GameObject playerCharacter; //added in since it seems the objects passed via the events are actually clones so cant access real action maps...

    public int currentMenuPosition = 1; //for keyboard navigation tracking

    //private PlayerInput menuInputs = null;
    //private PlayerInput playerInputs = null;

    //private  navigationTree;

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Escape Menu' action
    //We want to open up the pause menu, pause the game, and switch control from the player over to the Menu exclusively 
    //for more about pausing https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause
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




    //https://www.red-gate.com/simple-talk/dotnet/c-programming/how-to-create-a-settings-menu-in-unity/
    public void OnButtonHoverIn(BaseEventData eventData)
    {
        //Debug.Log("Hover In");
        //GameObject selectedButton = eventData.selectedObject;
        //Debug.Log(selectedButton);
        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(255, 255, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);
    }
    public void OnButtonHoverOut(BaseEventData eventData)
    {
        //Debug.Log("Hover Out");
        //GameObject selectedButton = eventData.selectedObject;
        //Debug.Log(selectedButton);
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
        curSelectedResIndex = validDisplayOptions.Count - (curSelectedResIndex + 1); //correct the index

        //fix the resolution list display 

        this.resolutionsDropdown.ClearOptions();
        this.resolutionsDropdown.AddOptions(validDisplayOptions);
        this.resolutionsDropdown.value = curSelectedResIndex;
        this.resolutionsDropdown.RefreshShownValue(); //just in case?
    }

    public void SetResolution(int resArrayIndex, bool shouldSave = true)
    {
        Debug.Log("Resolution Selected at Index: " + resArrayIndex);
        curSelectedResIndex = resArrayIndex;
        Resolution targetRes = resolutions[resArrayIndex];
        //TODO: save the user setting change to the settings file 
        //TODO: have a popup that autoreverts if not confirmed 

        //update the actual settings
        if (shouldSave)
        {
            UserSettings.Instance.UpdateResolution(targetRes);
            //UserSettings.Instance.currentResolution = Screen.currentResolution;
            UserSettings.Instance.SaveUserSettingsToFile();
        }

    }

    public void OnResolutionSelect(int eventData)
    {
        Debug.Log("Resolution Selected: " + eventData);
        int currentIndex = this.resolutionsDropdown.value;
        int resArrayIndex = resolutions.Length - (eventData + 1); //correct the visual array index vs actual index

        //set the new resolution for now, but dont save
        SetResolution(resArrayIndex, false);

        //set the resolution and save
        System.Action confirmFunction = (() => {
            Debug.Log("confirm pressed");
            SetResolution(resArrayIndex);
        });

        //revert the resolution [shouldnt need to save]
        System.Action cancelFunction = ( () => {
            Debug.Log("cancel pressed");
            SetResolution(currentIndex, false);
        });

        ActionChoicePopup("Confirm new Resolution", confirmFunction, cancelFunction, 15f );

        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(255, 255, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);

    }

    public void OnUpdateVsync(bool newValue)
    {
        UserSettings.Instance.saveData.screenData.isVsynced = newValue;
        UserSettings.Instance.SaveUserSettingsToFile();
    }
    public void OnUpdateWindowed(bool newValue)
    {
        UserSettings.Instance.saveData.screenData.isFullscreen = !newValue;
        UserSettings.Instance.SaveUserSettingsToFile();
    }
    public void OnUpdateResizable(bool newValue)
    {
        UserSettings.Instance.saveData.screenData.isResizable = newValue && !UserSettings.Instance.saveData.screenData.isFullscreen;
        UserSettings.Instance.SaveUserSettingsToFile();
    }

    public void UpdateBooleanOption(bool newValue)
    {
        
    }


    //https://hextantstudios.com/unity-custom-settings/
    //https://support.unity.com/hc/en-us/articles/115000177803-How-can-I-modify-Project-Settings-via-scripting-

    public void OnUpdateMainVolume(float newValue)
    {
        //TODO: consider having this operate on the mixer instead of the main project volume?  project volume is probably better due to core integration or something
        UserSettings.Instance.SetVolume("main", newValue, true);
    }
    public void OnUpdateFXVolume(float newValue) { UserSettings.Instance.SetVolume("FXVolume", newValue, true); }
    public void OnUpdateMusicVolume(float newValue) { UserSettings.Instance.SetVolume("MusicVolume", newValue, true); }
    public void OnUpdateVoicesVolume(float newValue) { UserSettings.Instance.SetVolume("VoicesVolume", newValue, true); }

    //https://api.unity.com/v1/oauth2/authorize?client_id=unity_learn&locale=en_US&redirect_uri=https%3A%2F%2Flearn.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Ftutorial%252Faudio-mixing%253Fuv%253D2020.1%2526projectId%253D5f4e4ee3edbc2a001f1211df&response_type=code&scope=identity+offline&state=f25e033d-349e-4a36-a483-5d5af2597eb7
    //https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();//call MenuScripts.Start();

        Debug.Log("<color=purple>Escape Menu Scripts</color>");

        Debug.Log($"temp init spot for userSettings {UserSettings.Instance}");
        //Debug.Log(UserSettings.Instance);

        //TODO setup a data structure that holds the hierarchy information so its more generalized
        //Otherwise turns these back into public vars that you use the inspector to assign 
        //https://docs.unity3d.com/ScriptReference/Transform.Find.html , if we make the structure hierarchical, we will need to add the /'s
        this.EscapeMenu = gameObject.transform.Find("EscapeMenu").gameObject;
        this.OptionsMenu = gameObject.transform.Find("OptionsMenu").gameObject;
        this.VideoOptionsMenu = gameObject.transform.Find("VideoOptionsMenu").gameObject;
        this.AudioOptionsMenu = gameObject.transform.Find("AudioOptionsMenu").gameObject;
        this.ControlsOptionsMenu = gameObject.transform.Find("ControlsOptionsMenu").gameObject;
        this.OtherOptionsMenu = gameObject.transform.Find("OtherOptionsMenu").gameObject;

        // disable/hide the menus initially;  if they are null here there be a problem
        this.EscapeMenu.SetActive(false);
        this.OptionsMenu.SetActive(false);
        this.VideoOptionsMenu.SetActive(false);
        this.AudioOptionsMenu.SetActive(false);
        this.ControlsOptionsMenu.SetActive(false);
        this.OtherOptionsMenu.SetActive(false);

        //setup inputs
        this.menuInputs = GetComponent<PlayerInput>();//grab reference to menu input controller
        this.playerInputs = playerCharacter.GetComponent<PlayerInput>(); //grab reference to player input controller
        this.menuInputs.currentActionMap.Disable(); //we need it to start disable atm since player is what has initial control (until we make more changes probably)


        //let unity get the valid detected screen resolutions (only works from exe not in editor allegedly) 
        this.resolutions = Screen.resolutions;
        UserSettings.Instance.UpdateResolution();
        this.resolutionsDropdown = VideoOptionsMenu.transform.Find("ResolutionsDropdown").gameObject.GetComponent<TMP_Dropdown>();
        SetResolutionOptions();


        //setup initial volumes 
        //TODO: have a location that saves the value between sessions that we grab from;
        //TODO: move functions to settings and/or datamanager
        float mainVolume = UserSettings.Instance.GetVolume("main");
        this.AudioOptionsMenu.transform.Find("GlobalVolume").gameObject.transform.Find("Slider").GetComponent<Slider>().SetValueWithoutNotify(mainVolume);
        float MusicVolume = UserSettings.Instance.GetVolume("MusicVolume");
        this.AudioOptionsMenu.transform.Find("MusicVolume").gameObject.transform.Find("Slider").GetComponent<Slider>().SetValueWithoutNotify(MusicVolume);
        float FXVolume = UserSettings.Instance.GetVolume("FXVolume");
        this.AudioOptionsMenu.transform.Find("FXVolume").gameObject.transform.Find("Slider").GetComponent<Slider>().SetValueWithoutNotify(FXVolume);
        float VoicesVolume = UserSettings.Instance.GetVolume("VoicesVolume");
        this.AudioOptionsMenu.transform.Find("VoiceVolume").gameObject.transform.Find("Slider").GetComponent<Slider>().SetValueWithoutNotify(VoicesVolume);

        //Debug.Log(this.menuInputs);
        //Debug.Log(this.playerInputs);

        //setup controls display
        Transform instanceParentObject = this.ControlsOptionsMenu.transform.Find("BindingsScroller").transform.Find("Viewport").transform.Find("Content").transform;

        //maybe helpful? https://answers.unity.com/questions/1748577/ui-issue-when-instantiating-ui-prefabs-at-runtime.html
        //TODO: revisit these to make some of the sizes reactive to the text inside of them  https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/HOWTO-UIFitContentSize.html

        foreach (InputAction action in this.playerInputs.actions)
        {
            //DumpToConsole(action);
            //Debug.Log($"Action bindings {JsonUtility.ToJson(action.bindings, true)}");
            //Debug.Log($"Action controls {JsonUtility.ToJson(action.controls, true)}");
            //Debug.Log($"Action interactions {JsonUtility.ToJson(action.interactions, true)}");
            //Debug.Log($"Action object {action}");
            //TODO: get working with the scroller

            InputBinding currentComposite = action.bindings[0];
            foreach (InputBinding actionBinding in action.bindings)
            {
                if (actionBinding.isComposite)
                {
                    //Debug.Log($"this binding is composite {actionBinding}");
                    currentComposite = actionBinding;
                }
                else if(actionBinding.isPartOfComposite )
                {
                    //Debug.Log($"this binding is part of the a composite {actionBinding}");
                    CreateUIControlBinder(instanceParentObject, action.GetBindingDisplayString() + actionBinding.name, actionBinding.ToDisplayString());
                }
            }
 
               //todo:make a function 
            
            //create a controlbinder as a child of the scroller
            GameObject controlBinder = Instantiate(this.controlBinderPrefab, instanceParentObject) as GameObject;

            //TODO: also make sure the recttransform part of the canvas renderer is behaving correctly?
            RectTransform rectTransform = controlBinder.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            rectTransform.anchorMax = new Vector2(.5f, .5f);
            rectTransform.anchorMin = new Vector2(.5f, .5f);
            rectTransform.pivot = new Vector2(.5f, .5f);
            //rectTransform.localPosition = new Vector3(placerStartX, placerStartY + (placerIntervalY * currentInterval), 0);
            //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, elementWidth);
            //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, elementHeight);
            //rectTransform.ForceUpdateRectTransforms();
            //rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge, 5f, elementWidth); 
            //rectTransform.rect.Set(0, 0, elementWidth, elementHeight);

            //Get and update the label 
            GameObject labelObject = controlBinder.transform.Find("Label").gameObject;
            labelObject.GetComponent<TMP_Text>().SetText(action.name);

            //Get and update the button
            GameObject buttonObject = controlBinder.transform.Find("Binding").gameObject; 
            buttonObject.transform.Find("Bind").gameObject.GetComponent<TMP_Text>().SetText(action.GetBindingDisplayString(0) );

            //TODO: add buttons for the 2 allowed sets (M+K vs Gamepad default, but separation not enforced--DMC does this)
            //TODO: add split for the directionals 

            //add the appropriate listener to the button 
            buttonObject.GetComponent<Button>().onClick.AddListener( () => OnRebindClick(action.name) ); //delegate { OnRebindClick(action.name); } [both of these work]
            
            //currentInterval++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
