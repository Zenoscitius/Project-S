using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;

//store the current settings for inputs and other values for access by other code
public class UserSettings : MonoBehaviour //cant declare complex members in static classes apparently
{
    //https://www.youtube.com/watch?v=xF2zUOfPyg8
    //https://blogs.unity3d.com/2020/11/26/learn-the-input-system-with-updated-tutorials-and-our-sample-project-warriors/?utm_source=youtube&utm_medium=social&utm_campaign=event_global_generalpromo_2020-11-26_unite-now-input-system-blog
    //https://forum.unity.com/threads/is-there-a-good-rebinding-tutorial-for-new-input-system.1025254/
    //https://answers.unity.com/questions/1718428/rebinding-in-new-input-system.html
    //https://unity.com/features/input-system
    //[old?] https://forum.unity.com/threads/rebinding-keys-isnt-reflected-in-existing-controlschemes.829191/
    //[OLD_INPUT_SYSTEM]https://docs.unity3d.com/Manual/class-InputManager.html

    public static bool isFullscreen = true;

    public static string inputType; //controller, keyboard+m
    public static Input lightAttackInput;

    //SwitchCurrentActionMap();

    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation.html
    void RemapButtonClicked(InputAction actionToRebind)
    {
        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                        // To avoid accidental input from mouse motion
                    .WithControlsExcluding("<Pointer>/position") // Don't bind to mouse position
                    .WithControlsExcluding("<Pointer>/delta") // Don't bind to mouse movement deltas
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

}
