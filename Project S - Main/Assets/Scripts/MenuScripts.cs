using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.Audio;

public class MenuScripts : MonoBehaviour
{
    protected PlayerInput menuInputs = null;
    protected PlayerInput playerInputs = null;
    public GameObject controlBinderPrefab;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    protected void CreateUIControlBinder(Transform instanceParentObject, string actionName, string buttonLabel)
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

}
