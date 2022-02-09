using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using System.IO; //for Path
using UnityEngine.Audio;
using UnityEditor;


//For Debugging and in-engine inline text changes
//https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html

//store the current settings for inputs and other values for access by other code

//[System.Serializable]
public class UserSettings : MonoBehaviour, ISerializationCallbackReceiver  //cant declare complex members in  classes apparently
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
    //  - rename class to your own classname
    //  - rename CS (C#) file too
    //
    // DO NOT PUT THIS IN ANY SCENE; this code auto-instantiates itself once
    // (the first time you try and access the instance==probably will be when the game starts) 
    //https://pastebin.com/SuvBWCpJ
    public static int count = 0;
    private static UserSettings _Instance;
    public static UserSettings Instance
    {
        get
        {
            if (!_Instance) //&& count == 0
            {
                Debug.Log("<color=green>Setting Up UserSettings _Instance</color>");
                _Instance = new GameObject().AddComponent<UserSettings>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    
    //Apparently these are just "easy json" basically and dont have inherent tie ins that are useful for us? 
    //https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
    //https://docs.unity3d.com/Manual/class-PlayerSettings.html

    //TODO: maybe change the name and expand beyond just resolution-- such as other screen data
    [System.Serializable]
    public struct ScreenData
    {
        public bool isFullscreen;
        public bool isResizable;
        public bool isVsynced;
        public int width;
        public int height;
        public int refreshRate;

        //public ScreenData(bool isFullscreen, bool isResizable, bool isVsynced)
        //{
        //    this.isFullscreen = isFullscreen;// Screen.fullScreen;
        //    this.isResizable = isResizable; //!Screen.fullScreen;
        //    this.isVsynced = isVsynced;
        //}
    }

    [System.Serializable]
    public struct AudioData
    {
        public float MainVolume;
        public float FXVolume;
        public float MusicVolume;
        public float VoicesVolume;
    }


    /* a) for loops on List<T> are a bit more than 2 times cheaper than foreach loops on List<T>, 
     * b) Looping on array is around 2 times cheaper than looping on List<T>, 
     * c) looping on array using for is 5 times cheaper than looping on List<T> using foreach (which most of us do).*/
    //https://stackoverflow.com/questions/365615/in-net-which-loop-runs-faster-for-or-foreach/365658#365658

    [System.Serializable]
    public struct ControlsData
    {
        public List<ControlPairing> controlPairingList;
        public string rebindJson; 
    }


    [System.Serializable]
    public struct SaveData
    {
        public ControlsData controlsData;
        public AudioData audioData;
        public ScreenData screenData;
    }

    //TODO: https://docs.unity3d.com/Manual/script-Serialization-Custom.html
    public void OnBeforeSerialize()
    {
        Debug.Log("<color=green>OnBeforeSerialize:</color> ");
        // Unity is about to read the serializedNodes field's contents.
        // The correct data must now be written into that field "just in time".
        //if (serializedNodes == null) serializedNodes = new List<SerializableNode>();
        //if (root == null) root = new Node();
        //serializedNodes.Clear();
        //AddNodeToSerializedNodes(root);
        // Now Unity is free to serialize this field, and we should get back the expected 
        // data when it is deserialized later.
    }
    public void OnAfterDeserialize()
    {
        Debug.Log("<color=green>OnAfterDeserialize:</color> ");
        //Unity has just written new data into the serializedNodes field.
        //let's populate our actual runtime data with those new values.
        //if (serializedNodes.Count > 0)
        //{
        //    ReadNodeFromSerializedNodes(0, out root);
        //}
        //else
        //    root = new Node();
    }

    //TODO: try out https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347#description for easier/better serialization




    //enumerate the actual data stored 
    public SaveData saveData;
    //public ScreenData screenData;
    //public AudioData audioData;
    //public ControlsData controlsData;

    public AudioMixer audioMixer; //TODO: do a private vs public pass
    private Resolution currentResolution;//  Screen.currentResolution;?
    public InputActionAsset playerInputActions;
    public InputActionAsset menuInputActions;
    //public PlayerPrefs unitySettings = new PlayerPrefs();

    //TODO: determine if this will serialize to json nicely for rebinding 
    //public string inputType; //controller, keyboard+m
    //public InputActionMap controlBindings;


    private string settingsFileName = "settings.dat";
    private bool triggerSaveOnChange = false;

    //SwitchCurrentActionMap();

    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@0.1/api/UnityEngine.Experimental.Input.InputActionRebindingExtensions.RebindingOperation.html
    public void RebindAction(InputAction actionToRebind, InputBinding bindingToRebind, string inputControlPath = null)
    {
        //InputAction actionToRebind = bindingToRebind.action;
        Debug.Log($"Attempting to rebind an action... {actionToRebind}");
        Debug.Log($"with current binding... {bindingToRebind}");
        //var rebind = new InputActionRebindingExtensions.RebindingOperation()
        //.WithAction(myAction)
        //.WithBindingGroup("Gamepad")
        //.WithCancelingThrough("<Keyboard>/escape");

        //this should apply to override path, which is not data-persistent 
        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Pointer>/position") // Don"t bind to mouse positionS
            .WithControlsExcluding("<Pointer>/delta") // Don"t bind to mouse movement deltas
            .WithCancelingThrough("<Keyboard>/escape") // let escape cancel the binding...may want to change so always have to bind in case we let escape menu rebinding?
            .OnMatchWaitForAnother(0.25f)
            .WithBindingMask(bindingToRebind)
            //.WithControlsHavingToMatchPath(inputControlPath) for restricting use to control paths
            .OnComplete(operation =>
            {
                Debug.Log($"Rebound '{actionToRebind}' to '{operation.selectedControl}'");
                operation.Dispose();//free memory otherwise it is a leak

                //manage json data for it
                UpdateSavedBindings();
      
            });
  
            rebindOperation.Start();
    }


    //updates the internal datastructure for storing the controls information
    //TODO
    private void UpdateSavedBindings(bool triggerSaveOnChange = false)
    {
        //manage json data for it
        foreach (InputActionMap actionMap in this.playerInputActions.actionMaps)
        {

            //https://forum.unity.com/threads/how-to-save-input-action-bindings.799311/

            string rebinds = actionMap.SaveBindingOverridesAsJson();
            this.saveData.controlsData.rebindJson = rebinds;
            //actionMap.LoadBindingOverridesFromJson(rebinds);
            Debug.Log($"<color=yellow>Updated BindingOverrides Json</color> {rebinds}");
            /*{"bindings":[
                * {"action":"PlayerControls/Light Attack",
                * "id":"46a9318b-4b50-4cb2-9681-bfcee2c1c710",
                * "path":"<Mouse>/rightButton",
                * "interactions":"",
                * "processors":""},
                * {"action":"PlayerControls/Light Attack",
                * "id":"8b2e0902-f089-40d6-bda5-3b9a1b1fe81d",
                * "path":"<Mouse>/rightButton",
                * "interactions":"","processors":""}]}
                */
            //loop through the actions of each map
            foreach (InputAction action in actionMap.actions)
            {

            }

        }

        if (triggerSaveOnChange || this.triggerSaveOnChange) SaveUserSettingsToFile();
    }

    //update to pass resolution
    public void UpdateResolution(Resolution newResolution, bool triggerSaveOnChange = false)
    {
        //TODO: maybe make sure that this is a valid resolution before pushing it? 

        bool resolutionIsValid = false;
        foreach (Resolution validResolution in Screen.resolutions)
        {
            if (validResolution.width == newResolution.width && validResolution.height == newResolution.height
                && validResolution.refreshRate == newResolution.refreshRate) resolutionIsValid = true;
        }

        if (!resolutionIsValid)
        {
            Debug.LogWarning($"UpdateResolution tried to set invalid resolution: {newResolution}");
            return;
        }

        this.saveData.screenData.height = newResolution.height;
        this.saveData.screenData.width = newResolution.width;
        this.saveData.screenData.refreshRate = newResolution.refreshRate;
        Screen.SetResolution(newResolution.width, newResolution.height, this.saveData.screenData.isFullscreen, newResolution.refreshRate);
        //this.currentResolution = newResolution;
        if (triggerSaveOnChange || this.triggerSaveOnChange) SaveUserSettingsToFile();
    }

    //push saved screendata to live
    public void UpdateResolution()
    {
        Screen.SetResolution(this.saveData.screenData.width, this.saveData.screenData.height, this.saveData.screenData.isFullscreen, this.saveData.screenData.refreshRate);
        if (triggerSaveOnChange || this.triggerSaveOnChange) SaveUserSettingsToFile();
    }



    //https://hextantstudios.com/unity-custom-settings/
    //https://support.unity.com/hc/en-us/articles/115000177803-How-can-I-modify-Project-Settings-via-scripting-

    //https://api.unity.com/v1/oauth2/authorize?client_id=unity_learn&locale=en_US&redirect_uri=https%3A%2F%2Flearn.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Ftutorial%252Faudio-mixing%253Fuv%253D2020.1%2526projectId%253D5f4e4ee3edbc2a001f1211df&response_type=code&scope=identity+offline&state=f25e033d-349e-4a36-a483-5d5af2597eb7
    //https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    //set the 0-100 value of the volume
    public void SetVolume(string type, float newValue, bool triggerSaveOnChange = false)
    {
        CheckAudioMixer();//make sure the mixer is loaded...for some reason if you dont it claims that it isnt even when it is

        Debug.Log($"New Audio Setting: {type} @ {newValue} ");
        if (type == "main" || type == "Main" || type == "MainVolume")
        {
            const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
            SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
            SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

            //update the actual value and the tracking
            saveData.audioData.MainVolume = newValue; //UserSettings.Instance.
            m_Volume.floatValue = newValue;
            audioManager.ApplyModifiedProperties();

        }
        else
        {
            if (type == "FXVolume") saveData.audioData.FXVolume = newValue; //UserSettings.Instance.
            else if (type == "VoicesVolume") saveData.audioData.VoicesVolume = newValue; //UserSettings.Instance.
            else if (type == "MusicVolume") saveData.audioData.MusicVolume = newValue; //UserSettings.Instance.

            if (newValue > 0) //when log doesnt break and it technically still makes noise
            {
                float convertedVolume = Mathf.Log10(newValue) * 20;
                //this.saveData.audioData.GetType().GetProperty(type).SetValue(audioData, convertedVolume);
                this.audioMixer.SetFloat(type, convertedVolume);
                Debug.Log($"New volume: {convertedVolume} ");
            }
            else //should mute instead (seems we cant access that option directly via script--just set to absolute min volume instead?
            {
                this.audioMixer.SetFloat(type, -80f);
            }
        }
        //TODO: trigger save
        if(triggerSaveOnChange || this.triggerSaveOnChange) SaveUserSettingsToFile();
    }

    //return the 0-100 value of the volume
    public float GetVolume(string type)
    {
        //Debug.Log($"Get Audio Setting: {type} @ {newValue} ");
        CheckAudioMixer();//make sure the mixer is loaded...for some reason if you dont it claims that it isnt even when it is

        if (type == "main" || type == "Main" || type == "MainVolume")
        {
            const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
            SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
            SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

            return m_Volume.floatValue;
        }
        else
        {
            this.audioMixer.GetFloat(type, out float rawVolume);
            if (rawVolume == -80f) return 0;
            else
            {
                float convertedVolume = Mathf.Pow(10f, (rawVolume / 20)); //reverse the conversion 
                return convertedVolume;
            }
        }
    }



    //save the settings as JSON to a file at a location TBD (dont do any data work here, should always save as-is)
    public void SaveUserSettingsToFile()
    {
        string settingsJson = JsonUtility.ToJson(this.saveData);
        Debug.Log($"SaveUserSettingsToFile: {settingsJson}");
        DataManager.SaveJsonDataToFile(settingsFileName, settingsJson);
    }

    //load the settings as JSON from a file at a location TBD
    public void LoadUserSettingsFromFile()
    {
        Debug.Log(">LoadUserSettingsFromFile()");
        //JSON.Parse(jsonString);
        //JsonUtility.FromJson(this);
        Debug.Log($"<color=yellow> playerInputActions json: {playerInputActions.ToJson()} </color>");
        string jsonData = DataManager.LoadJsonDataFromFile(settingsFileName);
        Debug.Log($"<color=yellow> raw json: {jsonData}  </color>");
        //Debug.Log($"loaded user settings Json Data: {DataManager.ConvertObjToJson(jsonData)}");
        if (jsonData.Length > 3)//if we actually have data...
        {
            Debug.Log("<color=green>User Settings file detected! </color>");
            //JsonUtility.FromJsonOverwrite(jsonData, this); //EditorJsonUtility
            JsonUtility.FromJsonOverwrite(jsonData, this.saveData); //EditorJsonUtility
            Debug.Log($"<color=yellow>this.saveData from jsonoverwrite: {this.saveData}  </color>");

            PushAllDataToActive();
        }
        else
        {
            Debug.Log("<color=red>NO User Settings file!  Engage defaults</color>");
            InitializeDefaultData();
        }
        Debug.Log($"loaded settings results [direct]: screenData=>{DataManager.ConvertObjToJson(this.saveData.screenData)}, " +
            $" audiodata=>{DataManager.ConvertObjToJson(this.saveData.audioData)}, " +
            $" controlsData=>{DataManager.ConvertObjToJson(this.saveData.controlsData)}");

        //Debug.Log($"<color=green>playerInputActions json: {playerInputActions.ToJson()} </color>");
        //Debug.Log($"loaded settings results [helper]: {DataManager.ConvertObjToJson(this)}");
        //var maps = InputActionMap.FromJson(json);
    }

    //pushes all the internal data to the actionable gamestate data without triggering a file save
    private void PushAllDataToActive(){
        Debug.Log("<color=blue>Pushing data to active</color>");
        UpdateResolution();
        //SetVolume("MainVolume", this.saveData.audioData.MainVolume); //already done by unity since using audioManger
        SetVolume("FXVolume", this.saveData.audioData.FXVolume);
        SetVolume("MusicVolume", this.saveData.audioData.MusicVolume);
        SetVolume("VoicesVolume", this.saveData.audioData.VoicesVolume);
  

        //update the overridePath's of the controls 
        foreach (ControlPairing controlPairing in this.saveData.controlsData.controlPairingList) {

            //get the action in the action map 

            //controlPairing.inputActionName

            //loop through the maps 
            foreach (InputActionMap actionMap in this.playerInputActions.actionMaps)
            {
                //Debug.Log($"<color=yellow>actionMap</color> {DataManager.ConvertObjTozJson(actionMap)}");
                //https://forum.unity.com/threads/how-to-save-input-action-bindings.799311/



                //loop through the actions of each map
                foreach (InputAction action in actionMap.actions)
                {
                    //string rebinds = action.SaveBindingOverridesAsJson();
                    if (action.name == controlPairing.inputActionName)
                    {
                        //action.ApplyBindingOverride();
                    }

                }

            }
        }


    }

    //sets all default data
    private void InitializeDefaultData()
    {
        Debug.Log("<color=blue>Init default data</color>");
        //todo: store initial states for this? 
        this.saveData.screenData.isFullscreen = Screen.fullScreen;
        this.saveData.screenData.isResizable = !Screen.fullScreen;
        this.saveData.screenData.isVsynced = false;
        this.saveData.screenData.refreshRate = Screen.currentResolution.refreshRate;
        this.saveData.screenData.height = Screen.currentResolution.height;
        this.saveData.screenData.width = Screen.currentResolution.width;
        this.saveData.audioData.MainVolume = 50f;
        this.saveData.audioData.FXVolume = 100f;
        this.saveData.audioData.MusicVolume = 100f;
        this.saveData.audioData.VoicesVolume = 100f;

        //no need to do default controls since theyre defined already, but we do want to remove the overrides
        //loop through the maps 
        foreach (InputActionMap actionMap in this.playerInputActions.actionMaps)
        {
            //Debug.Log($"<color=yellow>actionMap</color> {DataManager.ConvertObjToJson(actionMap)}");

            actionMap.RemoveAllBindingOverrides();

            //loop through the actions of each map
            foreach (InputAction action in actionMap.actions)
            {
                //action.RemoveAllBindingOverrides();

                //Debug.Log($"<color=yellow>input action </color> {DataManager.ConvertObjToJson(action)}");
                //Debug.Log($"<color=yellow>input action </color> {(action)}");
                //this.controlsData.controlPairingList.Add(new ControlPairing(action.name));
            }

        }

        //push the datas to active usage
        PushAllDataToActive();
    }

    private void CheckAudioMixer()
    {
        if(this.audioMixer == null)
        {
            this.audioMixer = Resources.Load<AudioMixer>("ResonanceAudioMixer") as AudioMixer;
            if (this.audioMixer != null) Debug.Log($"<color=yellow>Assigned audio mixer?</color> {DataManager.ConvertObjToJson(audioMixer)}");
            else Debug.Log($"<color=red>Audio mixer not loaded....</color>");
        }
        else Debug.Log($"<color=green>Audio mixer already loaded</color> {DataManager.ConvertObjToJson(audioMixer)}");
    }

    private void Awake()
    {

        if (count != 0)
        {
            Debug.Log("<color=red>User Settings instance trying to be awoken again....</color> ");
            return;
        }
        count++;

        Debug.Log("<color=green>User Settings instance up and running from awake woot!</color> ");

        //grab the audiomixer 
        //this.audioMixer = Resources.Load("ResonanceAudioMixer") as AudioMixer;
        this.audioMixer = Resources.Load<AudioMixer>("ResonanceAudioMixer") as AudioMixer;
        if (this.audioMixer != null) Debug.Log($"<color=yellow>Assigned audio mixer?</color> {DataManager.ConvertObjToJson(audioMixer)}");
        else Debug.Log($"<color=red>Audio mixer not loaded....</color>");

        //grab the controls 
        //this.controlBindings =
        this.playerInputActions = Resources.Load<InputActionAsset>("PlayerActions");// as InputActionMap;
        Debug.Log($"<color=yellow>Assigned player control bindings?</color> {DataManager.ConvertObjToJson(playerInputActions)}");

        //TODO
        //playerInputActions.DontDestroyOnLoad = true;

        //loop through the maps 
        foreach (InputActionMap actionMap in this.playerInputActions.actionMaps)
        {
            //Debug.Log($"<color=yellow>actionMap</color> {DataManager.ConvertObjToJson(actionMap)}");

            //loop through the actions of each map
            foreach (InputAction action in actionMap.actions)
            {

                //Debug.Log($"<color=yellow>input action </color> {DataManager.ConvertObjToJson(action)}");
                Debug.Log($"<color=yellow>input action binding id</color> {(action.GetBindingIndex())}");
                //this.controlsData.controlPairingList.Add(new ControlPairing(action.name));
            }
                
        }

        //this.menuInputActions = Resources.Load<InputActionAsset>("MenuActions");// as InputActionMap;
        //Debug.Log($"<color=yellow>Assigned menu control bindings?</color> {DataManager.ConvertObjToJson(menuInputActions)}");

        //Debug.Log(PlayerPrefs)    
        LoadUserSettingsFromFile();

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    private void Start()
    {
        //start doesnt get triggered
        Debug.Log("User Settings instance up and running from start.  This shouldnt ever actually trigger");
    }
}
