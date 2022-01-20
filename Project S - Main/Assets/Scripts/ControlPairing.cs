using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for json storage purposes
[System.Serializable]
public class ControlPairing : MonoBehaviour
{
    public string inputActionName;
    public string mainBindingOverride;
    public string secondaryBindingOverride;
    //TODO: how represent the subbinding ones....?

    public ControlPairing(string inputActionName, string mainBindingOverride, string secondaryBindingOverride)
    {
        //todo: add error handling
        this.inputActionName = inputActionName;
        this.mainBindingOverride = mainBindingOverride;
        this.secondaryBindingOverride = secondaryBindingOverride;
        //this.actionMapName = secondaryBinding;
    }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public override string ToString()
    //{
    //    return "";
    //}
}
