using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


public class StartMenuScripts : MonoBehaviour
{
    // Start is called before the first frame update
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

    private VisualElement startMenuUI;
    //public CharacterController characterController;
    private VisualTreeAsset startMenuUIAsset;

    public void OnEnable()
    {
        Debug.Log("UI Script OnEnable()");
        //startMenuUIAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Structure/UI-Home.uxml");
        //startMenuUI = startMenuUIAsset.Instantiate();

        startMenuUI = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log("startMenuUI:" + startMenuUI);

        //playerStateImage = root.Q<VisualElement>("PlayerStateImage");

        /*So it is clearly traversing the tree properly, but it is saying its screenspace box basically doesnt exist; 
         * Unclear if I am doing something wrong OR if the preview build doesnt have the ability to do runtime bindings or something
         * 
         * 
         * UPDATE: Appears to be that the cloned tree isnt treated as the same tree, so it doesnt count; Direct access seems to be the solution, despite being recommended against?
         * */
        //VisualElement startMenuUI = startMenuUIAsset;
        //Debug.Log("startMenuUI: " + startMenuUI);
        //Debug.Log("# children: " + startMenuUI.childCount ) ;
        //Debug.Log("stylesheets: " + startMenuUI.styleSheets); 
        // Debug.Log("resolvedStyle: " + startMenuUI.resolvedStyle);
    }


    void Start()
    //void OnEnable()
    {
        //https://docs.unity3d.com/2020.1/Documentation/Manual/UIE-LoadingUXMLcsharp.html

        Debug.Log("UI Script Start()" );
    
        Button optionsButton = startMenuUI.Q<Button>("OptionsButton");

        if (optionsButton != null)
        {
            //https://learn.unity.com/tutorial/uielements-first-steps/?tab=overview#5cd19ef9edbc2a1156524842
      
            Debug.Log("options button found: " + optionsButton);

            optionsButton.RegisterCallback<ClickEvent>(ButtonHandler, "mainMenuOptions"); //works, mousedownevent doesnt for some reason;

            //this method also works from: https://loglog.games/2020/09/27/unity-ui-toolkit-first-steps/
            optionsButton.clickable = new Clickable(() => {
               Debug.Log("OPTIONS BUTTON CLICK DETECTED!!!!");

               //Debug.Log(optionsButton.text);
               //optionsButton.text = "cat!";
               //Debug.Log(optionsButton.text);
           });

            //optionsButton.clicked += () => {
            //    Debug.Log("OPTIONS BUTTON CLICK DETECTED!!!!"); 
            //};

            //Debug.Log(optionsButton.pickingMode);
            //optionsButton.visible = false;
            //optionsButton.RemoveFromHierarchy();
            //startMenuUI.MarkDirtyRepaint();

            //optionsButton.RegisterCallback<MouseEnterEvent>(ButtonHoverHandler, TrickleDown.Trickledown);

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

    void ButtonHandler(ClickEvent evt, string name)
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
