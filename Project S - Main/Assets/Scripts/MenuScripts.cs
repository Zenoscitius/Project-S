using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;

public class MenuScripts : AudioController
{
    protected PlayerInput menuInputs = null;
    protected PlayerInput playerInputs = null;
    public GameObject controlBinderPrefab;
    public GameObject popupOverlay; //should be a UI object like a Canvas 
    //public GameObject popupOverlay;

    //https://answers.unity.com/questions/1277650/how-can-i-pass-a-method-like-a-parameter-unity3d-c.html
    //public delegate void ConfirmDelegateFxn(); // This defines what type of method you're going to call.
    //public ConfirmDelegateFxn m_methodToCall; // This is the variable holding the method you're going to cal

    protected void CreateUIControlBinder(Transform instanceParentObject, string actionName, string buttonLabel )
    {
        //create a controlbinder as a child of the scroller
        GameObject controlBinder = Instantiate(this.controlBinderPrefab, instanceParentObject) as GameObject;

        //TODO: also make sure the recttransform part of the canvas renderer is behaving correctly?
        RectTransform rectTransform = controlBinder.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;

        rectTransform.anchorMax = new Vector2(.5f, .5f);
        rectTransform.anchorMin = new Vector2(.5f, .5f);
        rectTransform.pivot = new Vector2(.5f, .5f);

        //Get and update the label 
        GameObject labelObject = controlBinder.transform.Find("Label").gameObject;
        labelObject.GetComponent<TMP_Text>().SetText(actionName);

        //Get and update the button
        GameObject buttonObject = controlBinder.transform.Find("Binding").gameObject;
        buttonObject.transform.Find("Bind").gameObject.GetComponent<TMP_Text>().SetText(buttonLabel);

        //TODO: add buttons for the 2 allowed sets (M+K vs Gamepad default, but separation not enforced--DMC does this)
        //TODO: add split for the directionals 

        //add the appropriate listener to the button 
        buttonObject.GetComponent<Button>().onClick.AddListener(() => OnRebindClick(actionName)); //delegate { OnRebindClick(action.name); } [both of these work]
    }

    //function that the rebind button triggers (it will have the string be statically in there, generated when the instances are made) 
    //TODO: allow rebindng of composite ones 
    public void OnRebindClick(string actionName, int bindingIndex = 0)
    {
        Debug.Log($"Click on rebind of : {actionName}");
        InputAction targetAction = this.menuInputs.currentActionMap.FindAction(actionName);
        if (targetAction == null) targetAction = this.playerInputs.currentActionMap.FindAction(actionName);
        if (targetAction == null) Debug.LogWarning($"No action found for string name: {actionName}");
        else UserSettings.Instance.RebindAction(targetAction);

        UpdateBindingDisplay();
    }



    /// <summary>
    /// Trigger a refresh of the currently displayed binding.
    /// </summary>
    public void UpdateBindingDisplay()
    {
        //var displayString = string.Empty;
        //var deviceLayoutName = default(string);
        //var controlPath = default(string);

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

    //TODO: turn into a class of its own
    //https://gamedevbeginner.com/coroutines-in-unity-when-and-how-to-use-them/
    //https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
    //popup to confirm/reject actions 
    //System.Func<double> function
    //System.Action function
    public void ActionChoicePopup(string actionText, System.Action confirmFunction, System.Action cancelFunction, float timer = 0f)
    {
        this.popupOverlay.SetActive(true);
        //parse inputs
        if (actionText.Trim() == "") actionText = "Are you sure?";
        //if (confirmFunction)       //( (confirmFunction) != null) confirmFunction = '';

        //cancel after a timer
        //TODO: see if this matters https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity3.html
        GameObject timerTextObj = null;
        if (timer != 0f)
        {
            this.popupOverlay.transform.Find("TimerText").gameObject.SetActive(true);
            timerTextObj = this.popupOverlay.transform.Find("TimerText").gameObject;
        }
        else this.popupOverlay.transform.Find("TimerText").gameObject.SetActive(false);

        IEnumerator cancelCoroutine = ActionChoiceWait(cancelFunction, timer, timerTextObj);
        if (timer != 0f)
        {
            StartCoroutine(cancelCoroutine); //start up that timer
            //Invoke(cancelFunction, timer); //old version where pass string 
        }

        //get subobjects
        GameObject actionTextObj = this.popupOverlay.transform.Find("ActionText").gameObject;
        GameObject confirmButtonObj = this.popupOverlay.transform.Find("ConfirmButton").gameObject;
        GameObject cancelButtonObj = this.popupOverlay.transform.Find("CancelButton").gameObject;

        //assign subobjects
        actionTextObj.gameObject.GetComponent<TMP_Text>().SetText(actionText);
        confirmButtonObj.GetComponent<Button>().onClick.AddListener( () => {
            confirmFunction();
            StopCoroutine(cancelCoroutine); //make sure selecting a choice removes the timer choice
            this.popupOverlay.SetActive(false);
        }); 
        cancelButtonObj.GetComponent<Button>().onClick.AddListener( () => { 
            cancelFunction();
            StopCoroutine(cancelCoroutine); //make sure selecting a choice removes the timer choice
            this.popupOverlay.SetActive(false);
        }); 

        //buttonObject.GetComponent<Button>().onClick.AddListener(() => OnRebindClick(actionName)); //delegate { OnRebindClick(action.name); } [both of these work]
    }

    //todo: have something to stop coroutine?
    //public void CloseActionChoicePopup(System.Action cancelFunction, IEnumerator coroutine)
    //{
    //    cancelFunction();
    //    StopCoroutine(coroutine); //make sure selecting a choice removes the timer choice
    //    this.popupOverlay.SetActive(false);
    //}
    private IEnumerator ActionChoiceWait(System.Action cancelFunction, float timer, GameObject timerTextObj = null)
    {
        //unscaled time so being paused doesnt matter

        //custom timer that lets you do other actions inside the wait
        float counter = 0f;
        bool counterStarted = false;
        //TODO: consider using waitUntil/waitWhile
        while (counter < timer)
        {
            if (counterStarted) counter += Time.fixedDeltaTime; //dont want to count the delta before timer started
            else counterStarted = true; //turn timer on

            Debug.Log("Current WaitTime: " + counter);

            //update any visual timers
            if(timerTextObj != null)
            {
                string timerText = string.Format("{0}", Mathf.CeilToInt(timer - counter) );
                timerTextObj.GetComponent<TMP_Text>().SetText(timerText);
            }

            yield return null; //Don't freeze Unity
        }

        //builtin unity timer
        //if (timer > 0f) yield return new WaitForSecondsRealtime(timer); //non-thread-blocking wait before runnign cancel 
        cancelFunction();
        this.popupOverlay.SetActive(false);
    }
    

    //note this will not work in the editor
    public void Exitgame()
    {
        #if !UNITY_EDITOR
                        Application.Quit();
        #else
                EditorApplication.ExitPlaymode();
        #endif
        //TODO: show confirm prompt; maybe should be callable without being paused?
        //if (gamePaused)
        //{

        //set the resolution and save
        System.Action confirmFunction = (() =>
        {
            Debug.Log("confirm pressed");
            //SetResolution(resArrayIndex);
            #if !UNITY_EDITOR
                                  Application.Quit();
            #else
                        EditorApplication.ExitPlaymode();
            #endif
        });

        //revert the resolution [shouldnt need to save]
        System.Action cancelFunction = (() =>
        {
            Debug.Log("cancel pressed");
            //SetResolution(currentIndex, false);
        });

        ActionChoicePopup("Are you sure you want to exit the game?", confirmFunction, cancelFunction);

        //TODO: autosave the game?  what should be the exit behavior in regards to current playstate (if any) 
        //}
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("<color=purple>Menu Scripts</color>");
        //have disabled initially
        this.popupOverlay.SetActive(false);

    }
    //// Update is called once per frame
    //void Update()
    //{

    //}

}
