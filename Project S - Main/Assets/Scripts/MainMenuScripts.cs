using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScripts : MonoBehaviour
{
    public List<GameObject> Children;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("what is this??: " + this);
        //Debug.LogFormat(this, "UTF-8",{ });

      // var MainMenuButtons = GetComponent<>()(transform);//this.GetComponents<GameObject>();


        foreach (Transform child in this.transform)
        {
            Children.Add(child.gameObject);
            Debug.Log("button: " + child.gameObject);
        }

        //Debug.Log(Children.Length);

       /* foreach(GameObject button in MainMenuButtons)
        {
            Debug.Log("button: " + button);

            if (button)
            {

            }

        }*/
    }

    // Update is called once per frame
    void Update()
    {

}

   /* int CountChildren(Transform a)
    {
        int childCount = 0;
        foreach (Transform b in a)
        {
            Debug.Log("Child: " + b);
            childCount++;
            childCount += CountChildren(b);
        }
        return childCount;
    }*/


}
