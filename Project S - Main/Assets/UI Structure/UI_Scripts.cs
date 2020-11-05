using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


public class UI_Scripts : MonoBehaviour
{

   public static void testUIScript()
   // private void OnEnable()
    {
        //https://docs.unity3d.com/2020.1/Documentation/Manual/UIE-LoadingUXMLcsharp.html

        Debug.Log("Test UI Script Console Log");

        VisualTreeAsset startMenuUIAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Structure/UI-Home.uxml");
     
        VisualElement startMenuUI = startMenuUIAsset.CloneTree();

        //VisualElement ui = uiAsset.CloneTree(null);
        //.rootVisualElement.Add(ui);
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //   // NewGame.RegisterCallback<MouseDownEvent>(MyCallback);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log("Test UI Script Console Log (update)");
    //}
}




/*public class MyWindow : EditorWindow
{
    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        EditorWindow w = EditorWindow.GetWindow(typeof(MyWindow));

        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MyWindow.uxml");
        VisualElement ui = uiAsset.CloneTree(null);

        w.rootVisualElement.Add(ui);
    }

    void OnGUI()
    {
        // Nothing to do here, unless you need to also handle IMGUI stuff.
    }
}*/