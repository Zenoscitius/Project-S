using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using System.IO; //for Path

//store the current settings for inputs and other values for access by other code

[System.Serializable]
public class UserSettings : MonoBehaviour //cant declare complex members in  classes apparently
{
    //https://www.youtube.com/watch?v=xF2zUOfPyg8
    //https://blogs.unity3d.com/2020/11/26/learn-the-input-system-with-updated-tutorials-and-our-sample-project-warriors/?utm_source=youtube&utm_medium=social&utm_campaign=event_global_generalpromo_2020-11-26_unite-now-input-system-blog
    //https://forum.unity.com/threads/is-there-a-good-rebinding-tutorial-for-new-input-system.1025254/
    //https://answers.unity.com/questions/1718428/rebinding-in-new-input-system.html
    //https://unity.com/features/input-system
    //[old?] https://forum.unity.com/threads/rebinding-keys-isnt-reflected-in-existing-controlschemes.829191/
    //[OLD_INPUT_SYSTEM]https://docs.unity3d.com/Manual/class-InputManager.html


    
   
    // To use: access with UserSettings.Instance
    //
    // To set up:
    //  - Copy this file (duplicate it)
    //  - rename class SingletonSimple to your own classname
    //  - rename CS file too
    //
    // DO NOT PUT THIS IN ANY SCENE; this code auto-instantiates itself once
    // (the first time you try and access the instance==probably will be when the game starts) 
    //https://pastebin.com/SuvBWCpJ
    private static UserSettings _Instance;
    public static UserSettings Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<UserSettings>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }


    //TODO: try out https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347#description for easier/better serialization

    //Apparently these are just "easy json" basically and dont have inherent tie ins that are useful for us? 
    //https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
    //https://docs.unity3d.com/Manual/class-PlayerSettings.html

    public bool isFullscreen = true;// Screen.fullScreen;
    public bool isResizable = false; //!Screen.fullScreen;
    public bool isVsynced = false;
    public SaveResolution saveResolution;
    Resolution currentResolution;//  Screen.currentResolution;?


    //TODO: maybe change the name and expand beyond just resolution-- such as other screen data
    [System.Serializable]
    public struct SaveResolution
    {
        public int height;
        public int width;
        public int refreshRate;

        //public bool isFullscreen;
        //public bool isResizable;
        //public bool isVsynced;

    }
   
    //public PlayerPrefs unitySettings = new PlayerPrefs();

    //TODO: determine if this will serialize to json nicely for rebinding 
    public  string inputType; //controller, keyboard+m
    public  InputActionMap controlBindings;

    private string settingsFileName = "settings.dat";
    private string settingsFilePath;
    //SwitchCurrentActionMap();

    private string SettingsToJson()
    {
        string json = "{";

        json += "'isFullscreen':'" + isFullscreen.ToString() + "',";
        json += "'isResizable':'" + isResizable.ToString() + "',";
        json += "'isVsynced':'" + isVsynced.ToString() + "',";

        json += "}";

        return json;
    }


    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation.html

    void RemapButtonClicked(InputAction actionToRebind)
    {
        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                // To avoid accidental input from mouse motion
                .WithControlsExcluding("<Pointer>/position") // Don"t bind to mouse position
                .WithControlsExcluding("<Pointer>/delta") // Don"t bind to mouse movement deltas
                .OnMatchWaitForAnother(0.1f)
                .Start();
    }

    /// <summary>
    /// Trigger a refresh of the currently displayed binding.
    /// </summary>
    public void UpdateBindingDisplay()
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        // Get display string from action.
        //var action = m_Action?.action;
        //if (action != null)
        //{
        //    var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
        //    if (bindingIndex != -1)
        //        displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
        //}

        //// Set on label (if any).
        //if (m_BindingText != null)
        //    m_BindingText.text = displayString;

        //// Give listeners a chance to configure UI in response.
        //m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
    }

       


    public void UpdateResolution(Resolution newResolution)
    {
        this.saveResolution.height = newResolution.height;
        this.saveResolution.width = newResolution.width;
        this.saveResolution.refreshRate = newResolution.refreshRate;
        currentResolution = newResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen, currentResolution.refreshRate);
    }
    public void UpdateResolution()
    {
        currentResolution.width = this.saveResolution.width;
        currentResolution.height = this.saveResolution.height;
        currentResolution.refreshRate = this.saveResolution.refreshRate;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen, currentResolution.refreshRate);
    }


    //save the settings as JSON to a file at a location TBD
    public void SaveUserSettingsToFile()
    {
        string settingsJson = JsonUtility.ToJson(this);
        Debug.Log($"SaveUserSettingsToFile: {settingsJson}");

        DataManager.SaveJsonDataToFile(settingsFileName, settingsJson);
    }


    //load the settings as JSON from a file at a location TBD
    public void LoadUserSettingsFromFile()
    {
        //Debug.Log("LoadUserSettingsFromFile");
        //JSON.Parse(jsonString);
        //JsonUtility.FromJson(this);

        //if(File.Exists(settingsFileName))
        //{
            string jsonData = DataManager.LoadJsonDataFromFile(settingsFileName);

            Debug.Log($"loaded user settings Json Data: {jsonData}");

            if (jsonData.Length > 3)
            {
                JsonUtility.FromJsonOverwrite(jsonData, this);
                UpdateResolution();
            }
        //}
       
    }

    private void Awake()
    {
        //Debug.Log("User Settings instance up and running from awake woot!");
        //Debug.Log(PlayerPrefs)
        settingsFilePath = Path.Combine(Application.persistentDataPath, settingsFileName);
        bool settingsFileExists = File.Exists(settingsFilePath);

        if (settingsFileExists)
        {
            //Debug.Log("User Settings file exists!");
            LoadUserSettingsFromFile();
        }
        else
        {
            Debug.Log("NO User Settings file !");
            this.isFullscreen = Screen.fullScreen;
            this.isResizable = !Screen.fullScreen;
            this.isVsynced = false;
            this.currentResolution = Screen.currentResolution;
        }

    }

    private void Start()
    {
        //Debug.Log("User Settings instance up and running from start woot!");

    }
}
