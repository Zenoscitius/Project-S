using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using System.IO; //for Path
using UnityEngine.Audio;
using UnityEditor;

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
    private Resolution currentResolution;//  Screen.currentResolution;?
    public AudioMixer audioMixer;

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

    
    //not presently used
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
            .OnComplete(operation => {
                Debug.Log($"Rebound '{actionToRebind}' to '{operation.selectedControl}'");
                operation.Dispose();
            })
            .Start();
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

    //https://api.unity.com/v1/oauth2/authorize?client_id=unity_learn&locale=en_US&redirect_uri=https%3A%2F%2Flearn.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Ftutorial%252Faudio-mixing%253Fuv%253D2020.1%2526projectId%253D5f4e4ee3edbc2a001f1211df&response_type=code&scope=identity+offline&state=f25e033d-349e-4a36-a483-5d5af2597eb7
    //https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    public void SetVolume(string type, float newValue)
    {
        if (type == "main")
        {
            const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
            SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
            SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

            m_Volume.floatValue = newValue;
            audioManager.ApplyModifiedProperties();
        }
        else
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
  
    }

    public float GetVolume(string type)
    {
        if (type == "main")
        {
            const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
            SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
            SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

            return m_Volume.floatValue;
        }
        else
        {
            //if (newValue > 0) //when log doesnt break and it technically still makes noise
            //{
            //    float convertedVolume = Mathf.Log10(newValue) * 20;
            //    audioMixer.SetFloat(type, convertedVolume);
            //    Debug.Log($"New volume: {convertedVolume} ");
            //}
            //else //should mute instead (seems we cant access that option directly via script--just set to absolute min volume instead?
            //{
            //    audioMixer.SetFloat(type, -80f);
            //}
            return 50;
        }

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
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen, newResolution.refreshRate);
        this.currentResolution = newResolution;
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

        string jsonData = DataManager.LoadJsonDataFromFile(settingsFileName);

        Debug.Log($"loaded user settings Json Data: {DataManager.ConvertObjToJson(jsonData)}");
        Debug.Log($"loaded settngs results: {DataManager.ConvertObjToJson(this)}");
        if (jsonData.Length > 3)
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
            
            UpdateResolution();
        }
       
      
    }

    private void Awake()
    {
        Debug.Log("<color=green>User Settings instance up and running from awake woot!</color> ");
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
            Debug.Log("<color=red>NO User Settings file !</color>");
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
