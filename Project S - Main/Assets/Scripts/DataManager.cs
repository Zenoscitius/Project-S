using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //for Path
using System;

public static class DataManager
{
    //save the settings as JSON to a file at a location TBD
    public static bool SaveJsonDataToFile(string filePath, string settingsJson)
    {
        Debug.Log($"SaveJsonDataToFile: {filePath}");
        string fullPath = "";
        if (filePath.Length > 0) fullPath = Path.Combine(Application.persistentDataPath, filePath);
        else
        {
            Debug.LogWarning($"cant use empty path {filePath} for data {settingsJson}");
            return false;
        }

        try
        {
            File.WriteAllText(fullPath, settingsJson);
            return true;
        }
        catch (Exception err)
        {
            Debug.LogError($"Failed to write file at {fullPath}: {err}");
        }
        return false;

    }


    //load the settings as JSON from a file at a location TBD
    public static string LoadJsonDataFromFile(string filePath)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, filePath);

        Debug.Log($"LoadJsonDataFromFile: {filePath}");
        //JSON.Parse(jsonString);
        //JsonUtility.FromJson(this);
        if (File.Exists(fullPath))
        {
            try
            {
                string fileContents = File.ReadAllText(fullPath);
                //Debug.Log($"file exists! {fileContents}");
                //calling FromJson here makes it stop being a regular string ti seems?
                return fileContents;// JsonUtility.FromJson<string>(fileContents);
            }
            catch (Exception err)
            {
                Debug.LogError($"Failed to read file at {fullPath}: {err}");
                return "";
            }
        }
        else
        {
            Debug.LogError($"file DOESNT EXIST! {filePath}");
            return "";
        }
        //JsonUtility.FromJsonOverwrite(json, myObject);
    }


}
