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


    public GameObject controlBinderPrefab;


    public GameObject playerCharacter; //added in since it seems the objects passed via the events are actually clones so cant access real action maps...

    public int currentMenuPosition = 1; //for keyboard navigation tracking

    public AudioMixer audioMixer;

    private PlayerInput menuInputs = null;
    private PlayerInput playerInputs = null;

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
        //Debug.Log(selectedButton);
        //Image buttonBackground = selectedButton.GetComponent<Image>();
        //Debug.Log(buttonBackground.color);
        //buttonBackground.color = new Color(255, 255, 255, 1); //keep updated to whatever it actually is OR keep a holder var somewhere
        //Debug.Log(buttonBackground.color);
    }
    public void OnButtonHoverOut(BaseEventData eventData)
    {
        //Debug.Log("Hover Out");
        GameObject selectedButton = eventData.selectedObject;
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


    //https://hextantstudios.com/unity-custom-settings/
    //https://support.unity.com/hc/en-us/articles/115000177803-How-can-I-modify-Project-Settings-via-scripting-


    public void UpdateMainVolume(float newValue)
    {
        //Debug.Log($"New setting: {newValue} ");
        const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
        SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
        SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

        m_Volume.floatValue = newValue;
        audioManager.ApplyModifiedProperties();
    }


    public float GetMainVolume()
    {
        const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
        SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
        SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

        return m_Volume.floatValue;
    }


    public void OnUpdateMainVolume(float newValue)
    {
        //TODO: consider having this operate on the mixer instead of the main project volume?  project volume is probably better due to core integration or something
        //Debug.Log($"New setting: {newValue} ");
        UpdateMainVolume(newValue);
    }


    public void OnUpdateFXVolume(float newValue) { SetVolume("FXVolume", newValue); }
    public void OnUpdateMusicVolume(float newValue) { SetVolume("MusicVolume", newValue); }
    public void OnUpdateVoicesVolume(float newValue) { SetVolume("VoicesVolume", newValue); }

    //https://api.unity.com/v1/oauth2/authorize?client_id=unity_learn&locale=en_US&redirect_uri=https%3A%2F%2Flearn.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Ftutorial%252Faudio-mixing%253Fuv%253D2020.1%2526projectId%253D5f4e4ee3edbc2a001f1211df&response_type=code&scope=identity+offline&state=f25e033d-349e-4a36-a483-5d5af2597eb7
    //https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    public void SetVolume(string type, float newValue)
    {
        if (newValue > 0) //when log doesnt break and it technically still makes noise
        {
            float convertedVolume = Mathf.Log10(newValue) * 20;
            audioMixer.SetFloat(type, convertedVolume);
            Debug.Log($"New volume: {convertedVolume} ");
        }
        else //should mute instead (seems we cant access that option directly via script--just set to absolute min volume instead?
        {
            audioMixer.SetFloat(type, -80f);
        }
     
    }


    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@0.1/api/UnityEngine.Experimental.Input.InputActionRebindingExtensions.RebindingOperation.html
    public void RebindAction(InputAction actionToRebind)
    {
        Debug.Log($"Attempting to rebind an action... {actionToRebind}");
        //var rebind = new InputActionRebindingExtensions.RebindingOperation()
        //.WithAction(myAction)
        //.WithBindingGroup("Gamepad")
        //.WithCancelingThrough("<Keyboard>/escape");

        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Pointer>/position") // Don"t bind to mouse positionS
            .WithControlsExcluding("<Pointer>/delta") // Don"t bind to mouse movement deltas
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete( operation => {
                Debug.Log($"Rebound '{actionToRebind}' to '{operation.selectedControl}'");
                operation.Dispose();
            })
            .Start();
    }


    //function that the rebind button triggers (it will have the string be statically in there, generated when the instances are made) 
    public void OnRebindClick(string actionName)
    {
        Debug.Log($"Click on rebind of : {actionName}");
        InputAction targetAction = this.menuInputs.currentActionMap.FindAction(actionName);
        if(targetAction == null) targetAction = this.playerInputs.currentActionMap.FindAction(actionName);
        if (targetAction == null) Debug.LogWarning($"No action found for string name: {actionName}");
        else RebindAction(targetAction);
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
        this.AudioOptionsMenu = gameObject.transform.Find("AudioOptionsMenu").gameObject;
        this.ControlsOptionsMenu = gameObject.transform.Find("ControlsOptionsMenu").gameObject;
        this.OtherOptionsMenu = gameObject.transform.Find("OtherOptionsMenu").gameObject;

        //disable/hide the menus 
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

        //Debug.Log(this.menuInputs);
        //Debug.Log(this.playerInputs);


        //let unity get the valid detected screen resolutions (only works from exe not in editor allegedly) 
        this.resolutions =  Screen.resolutions;
        this.resolutionsDropdown = VideoOptionsMenu.transform.Find("ResolutionsDropdown").gameObject.GetComponent<TMP_Dropdown>();
        SetResolutionOptions();
  


        //setup initial volumes 
        //TODO: have a location that saves the value between sessions that we grab from;
        float mainVolume = GetMainVolume();
        this.AudioOptionsMenu.transform.Find("GlobalVolume").gameObject.transform.Find("Slider").GetComponent<Slider>().value = mainVolume;



        //setup controls display
        Transform instanceParentObject = this.ControlsOptionsMenu.transform.Find("BindingsScroller").transform.Find("Viewport").transform.Find("Content").transform;
        float parentWidth = instanceParentObject.GetComponent<RectTransform>().rect.width;
        float parentHeight = instanceParentObject.GetComponent<RectTransform>().rect.height;
        //Debug.Log($"Parent Rect sizeDelta {instanceParentObject.GetComponent<RectTransform>().sizeDelta}");
        //Debug.Log($"Parent Rect position {instanceParentObject.GetComponent<RectTransform>().position}");
        Debug.Log($"Parent Rect rect {instanceParentObject.GetComponent<RectTransform>().rect}");

        RectTransform prefabRT = this.controlBinderPrefab.GetComponent<RectTransform>();
        this.controlBinderPrefab.transform.localScale = prefabRT.localScale =  Vector3.one;//just in case
   

        //RectTransform rt = (RectTransform)this.controlBinderPrefab.transform;
        //Debug.Log($"Element Test method  rect  {rt.sizeDelta}");
        //float width = GetComponent<SpriteRenderer>().bounds.size.x;
        //Debug.Log($"Element Test method  rect  {width}");

        //for now we have to set the heights and widths manually because I havent been able to figure out how to get the prefab to have non-zero width and height 
        float elementHeight = 50f;// prefabRT.rect.height;
        float elementWidth = 380f;//prefabRT.rect.height;
        prefabRT.rect.Set(0, 0, elementWidth, elementHeight);

        Debug.Log($"Element Rect rect  {prefabRT.rect}");
        float placerStartX = 0 + (elementWidth/2);// -(parentWidth/2);//start at left? right now parent is placed using top, left and elements are placed via center 
        float placerStartY = 0 - elementHeight;//start at top, then move the height down
        float placerIntervalY = -elementHeight - 20; //go down
        int currentInterval = 0;//track where we are

        //maybe helpful? https://answers.unity.com/questions/1748577/ui-issue-when-instantiating-ui-prefabs-at-runtime.html

        foreach (InputAction action in this.playerInputs.actions)
        {
            //TODO: get working with the scroller

            //create a controlbinder as a child of the scroller
            GameObject controlBinder = Instantiate(this.controlBinderPrefab, instanceParentObject) as GameObject;
            //GameObject controlBinder = Instantiate(this.controlBinderPrefab) as GameObject;
            //controlBinder.transform.SetParent(instanceParentObject, false);
            //controlBinder.transform.localPosition = new Vector3 (0, placerStart + (placerIntervalY * currentInterval), 0); //use .Set if this doesnt work ?

            //TODO: also make sure the recttransform part of the canvas renderer is behaving correctly?
            RectTransform rectTransform = controlBinder.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            rect.height = elementHeight;
            rect.width = elementWidth;
            rectTransform.anchorMax = new Vector2(.5f, .5f);
            rectTransform.anchorMin = new Vector2(.5f, .5f);
            rectTransform.pivot = new Vector2(.5f, .5f);
            rectTransform.localPosition = new Vector3(placerStartX, placerStartY + (placerIntervalY * currentInterval), 0);

            rectTransform.rect.Set(0, 0, elementWidth, elementHeight);

            //test of  https://answers.unity.com/questions/1748577/ui-issue-when-instantiating-ui-prefabs-at-runtime.html 
            //RectTransform rectTest = controlBinder.transform as RectTransform;
            //rectTest.localScale = Vector3.one;

            Debug.Log($"Final Rect Transform of {action.name} is {rectTransform.anchorMax}, {rectTransform.anchorMin}, {rectTransform.pivot}, {rectTransform.sizeDelta}");

            //Get and update the label 
            GameObject labelObject = controlBinder.transform.Find("Label").gameObject;
            labelObject.GetComponent<TMP_Text>().SetText(action.name);

            //Get and update the button
            GameObject buttonObject = controlBinder.transform.Find("Binding").gameObject; 
            buttonObject.transform.Find("Bind").gameObject.GetComponent<TMP_Text>().SetText(action.GetBindingDisplayString() );

            //add the appropriate listener to the button 
            buttonObject.GetComponent<Button>().onClick.AddListener( () => OnRebindClick(action.name) ); //delegate { OnRebindClick(action.name); } [both of these work]

            //Debug.Log($"Final Rect Transform of {action.name} is {rectTransform}");

            //Debug.Log($"New local Position Vector of {action.name} should be {new Vector3(0, placerStart + (placerIntervalY * currentInterval), 0)}");
            Debug.Log($"Created ControlBinder {action.name} at {controlBinder.transform.localPosition}");

            currentInterval++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
