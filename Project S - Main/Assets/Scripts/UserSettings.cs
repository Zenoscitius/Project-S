﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using System.IO; //for Path
using UnityEngine.Audio;
using UnityEditor;

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

    //TODO: https://docs.unity3d.com/Manual/script-Serialization-Custom.html
    public void OnBeforeSerialize()
    {
        Debug.Log("<color=green>OnBeforeSerialize</color>");
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
        Debug.Log("<color=green>OnAfterDeserialize</color>");
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

    [System.Serializable]
    public struct ControlsData
    {

    }



    public ScreenData screenData;
    public AudioData audioData;
    private Resolution currentResolution;//  Screen.currentResolution;?
    public AudioMixer audioMixer; //TODO: do a private vs public pass


    //public PlayerPrefs unitySettings = new PlayerPrefs();

    //TODO: determine if this will serialize to json nicely for rebinding 
    public  string inputType; //controller, keyboard+m
    public  InputActionMap controlBindings;

    private string settingsFileName = "settings.dat";

    //SwitchCurrentActionMap();

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

    //https://api.unity.com/v1/oauth2/authorize?client_id=unity_learn&locale=en_US&redirect_uri=https%3A%2F%2Flearn.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Ftutorial%252Faudio-mixing%253Fuv%253D2020.1%2526projectId%253D5f4e4ee3edbc2a001f1211df&response_type=code&scope=identity+offline&state=f25e033d-349e-4a36-a483-5d5af2597eb7
    //https://gamedevbeginner.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    //set the 0-100 value of the volume
    public void SetVolume(string type, float newValue)
    {
        //Debug.Log($"New Audio Setting: {type} @ {newValue} ");
        if (type == "main" || type == "MainVolume")
        {
            const string AudioSettingsAssetPath = "ProjectSettings/AudioManager.asset";
            SerializedObject audioManager = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AudioSettingsAssetPath)[0]);
            SerializedProperty m_Volume = audioManager.FindProperty("m_Volume");

            //update the actual value and the tracking
            m_Volume.floatValue = this.audioData.MainVolume = newValue;
            audioManager.ApplyModifiedProperties();

        }
        else
        {
            if (type == "FXVolume") this.audioData.FXVolume = newValue;
            else if (type == "VoicesVolume") this.audioData.VoicesVolume = newValue;
            else if (type == "MusicVolume") this.audioData.MusicVolume = newValue;

            if (newValue > 0) //when log doesnt break and it technically still makes noise
            {
                float convertedVolume = Mathf.Log10(newValue) * 20;
                //this.audioData.GetType().GetProperty(type).SetValue(audioData, convertedVolume);
                audioMixer.SetFloat(type, convertedVolume);
                Debug.Log($"New volume: {convertedVolume} ");   
            }
            else //should mute instead (seems we cant access that option directly via script--just set to absolute min volume instead?
            {
                audioMixer.SetFloat(type, -80f);
            }
        }
        //TODO: trigger save
        //SaveUserSettingsToFile();
    }

    //return the 0-100 value of the volume
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
    
            audioMixer.GetFloat(type, out float rawVolume);
            if (rawVolume == -80f) return 0;
            else
            {
                float convertedVolume = Mathf.Pow((rawVolume / 20), 10f); //reverse the conversion 
                return convertedVolume;
            }
        }
    }

    //update to pass resolution
    public void UpdateResolution(Resolution newResolution)
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
 
        this.screenData.height = newResolution.height;
        this.screenData.width = newResolution.width;
        this.screenData.refreshRate = newResolution.refreshRate;
        Screen.SetResolution(newResolution.width, newResolution.height, this.screenData.isFullscreen, newResolution.refreshRate);
        //this.currentResolution = newResolution;
    }

    //push saved screendata to live
    public void UpdateResolution()
    {
        Screen.SetResolution(this.screenData.width, this.screenData.height, this.screenData.isFullscreen, this.screenData.refreshRate);
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

        //Debug.Log($"loaded user settings Json Data: {DataManager.ConvertObjToJson(jsonData)}");
        if (jsonData.Length > 3)//if we actually have data...
        {
            JsonUtility.FromJsonOverwrite(jsonData, this); //EditorJsonUtility
            UpdateResolution();
        }
        else
        {
            Debug.Log("<color=red>NO User Settings file!  Engage defaults</color>");
            InitializeDefaultData();
        }
        Debug.Log($"loaded settings results [direct]: {DataManager.ConvertObjToJson(this.screenData)}, " +
            $" {DataManager.ConvertObjToJson(this.audioData)}");
        Debug.Log($"loaded settings results [helper]: {DataManager.ConvertObjToJson(this)}");


    }

    //sets all default data
    private void InitializeDefaultData()
    {
        this.screenData.isFullscreen = Screen.fullScreen;
        this.screenData.isResizable = !Screen.fullScreen;
        this.screenData.isVsynced = false;
        this.screenData.refreshRate = Screen.currentResolution.refreshRate;
        this.screenData.height = Screen.currentResolution.height;
        this.screenData.width = Screen.currentResolution.width;
        this.audioData.MainVolume = 50;
        this.audioData.FXVolume = 100;
        this.audioData.MusicVolume = 100;
        this.audioData.VoicesVolume = 100;

        //no need to do default controls since theyre defined already

        //push the datas to active usage
        UpdateResolution();
        SetVolume("MainVolume", this.audioData.MainVolume);
        SetVolume("FXVolume", this.audioData.FXVolume);
        SetVolume("MusicVolume", this.audioData.MusicVolume);
        SetVolume("VoicesVolume", this.audioData.VoicesVolume);
    }

    private void Awake()
    {
        Debug.Log("<color=green>User Settings instance up and running from awake woot!</color> ");
        //Debug.Log(PlayerPrefs)
   
        LoadUserSettingsFromFile();
    }

    private void Start()
    {
        //Debug.Log("User Settings instance up and running from start woot!");
    }
}
