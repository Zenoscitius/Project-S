using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //register keycontrols; TODO: Make this dynamic based upon user settings;
        //TODO: determine what Unity input system is most appropriate to use 
        //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Settings.html
        //https://docs.unity3d.com/2020.1/Documentation/Manual/class-InputManager.html

    }

    // Update is called once per frame
    /*    void Update()
        {

        }*/


    public void OpenConfirmation(string type)
    {
        if(type == "" || type == null)
        {

        } else if(type == "exit") {

        } else if(type == "newGame" ) {

        }

    }


    /* Opens up the in-game options menu --should be more or less the same as the main menu, but different formatting etc
     * 
     * 
     */
    public void OpenInGameMenu()
    {

    }


    public void ExitGame()
    {
        //TODO: add confirmation system/or one off
        Application.Quit();
    }



    public void OpenOptionsMenu()
    {

    }

    public void NewGame()
    {
        //ask confirmation -- TODO: determine how new and old data interact

        //call the stuff involving in making a new game save session
    }

    public void SaveGame()
    {

    }



}
