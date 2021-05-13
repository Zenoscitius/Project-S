using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //for Path


public static class DataManager
{
    //save the settings as JSON to a file at a location TBD
    public static bool SaveJsonDataToFile(string filePath, string settingsJson)
    {
        Debug.Log("SaveJsonDataToFile");
        File.WriteAllText(filePath, settingsJson);

        return true; 
    }


    //load the settings as JSON from a file at a location TBD
    public static string LoadJsonDataFromFile(string filePath)
    {
        Debug.Log("LoadJsonDataFromFile");
        //JSON.Parse(jsonString);
        //JsonUtility.FromJson(this);
        if (File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);

            return JsonUtility.FromJson<string>(fileContents);

        }
        else return "";
        //JsonUtility.FromJsonOverwrite(json, myObject);
    }


}
