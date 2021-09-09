using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class AudioController : MonoBehaviour
{

    //actual volume controller
    public AudioMixer audioMixer;



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
            m_Volume.floatValue = UserSettings.Instance.audioData.MainVolume = newValue;
            audioManager.ApplyModifiedProperties();

        }
        else
        {
            if (type == "FXVolume") UserSettings.Instance.audioData.FXVolume = newValue;
            else if (type == "VoicesVolume") UserSettings.Instance.audioData.VoicesVolume = newValue;
            else if (type == "MusicVolume") UserSettings.Instance.audioData.MusicVolume = newValue;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
