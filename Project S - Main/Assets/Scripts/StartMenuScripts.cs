using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class StartMenuScripts : MonoBehaviour
{
    // Start is called before the first frame update
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    void Start()
    //void OnEnable()
    {
        //https://docs.unity3d.com/2020.1/Documentation/Manual/UIE-LoadingUXMLcsharp.html

        Debug.Log("Test UI Script Console Log" );

        /*So it is clearly traversing the tree properly, but it is saying its screenspace box basically doesnt exist; 
         * Unclear if I am doing something wrong OR if the preview build doesnt have the ability to do runtime bindings or something
         * 
         * 
         * 
         * */

        VisualTreeAsset startMenuUIAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Structure/UI-Home.uxml");

        VisualElement startMenuUI = startMenuUIAsset.Instantiate();
        //VisualElement startMenuUI = startMenuUIAsset;

        Debug.Log("startMenuUI: " + startMenuUI);
        //Debug.Log("# children: " + startMenuUI.childCount ) ;
        //Debug.Log("stylesheets: " + startMenuUI.styleSheets); 
        Debug.Log("resolvedStyle: " + startMenuUI.resolvedStyle);

        var optionsButton = startMenuUI.Q<Button>("OptionsButton");

        if (optionsButton != null)
        {
            //https://learn.unity.com/tutorial/uielements-first-steps/?tab=overview#5cd19ef9edbc2a1156524842
      
            Debug.Log("options button found: " + optionsButton);

            //Debug.Log(optionsButton.text);
            optionsButton.text = "cat!";
            //Debug.Log(optionsButton.text);

            optionsButton.clicked += () => {
                Debug.Log("OPTIONS BUTTON CLICK DETECTED!!!!"); 
            };
        //Debug.Log(optionsButton.pickingMode);
        //optionsButton.visible = false;
        //optionsButton.RemoveFromHierarchy();
        //startMenuUI.MarkDirtyRepaint();

        //optionsButton.RegisterCallback<MouseEnterEvent>(ButtonHoverHandler, TrickleDown.Trickledown);
        //optionsButton.RegisterCallback<MouseDownEvent>(ButtonHandler);
            //optionsButton.RegisterCallback<ClickEvent>(ButtonHandler);

            //optionsButton.MarkDirtyRepaint();

        }
    }

    // Update is called once per frame
    void Update()
    {
        //optionsButton.RegisterCallback<MouseEnterEvent>(ButtonHoverHandler, TrickleDown.NoTrickleDown);
        //optionsButton.RegisterCallback<MouseDownEvent>(ButtonHandler);
    }

    void ButtonHandler(MouseDownEvent evt)
    //void ButtonHandler(ClickEvent evt)
    {
        Debug.Log("========Test UI Script BUTTON PRESS Log===========");
            //evt.StopPropagation();
    }

    void ButtonHoverHandler(MouseEnterEvent evt)
    {
        Debug.Log("========Test UI Script BUTTON HOVER Log===========");
        //evt.StopPropagation();
    }


    void TestHandler(string primitiveTypeName)
    {
        Debug.Log("========Test UI Script CLICKED PROP Log===========");
        //evt.StopPropagation();
    }


}
