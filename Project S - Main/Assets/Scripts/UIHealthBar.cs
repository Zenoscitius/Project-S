using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*UI Portion of the Healthbar. IS NOT the data source.*/
public class UIHealthBar : ScriptableObject
{
    //public static UIHealthBar instance { get; private set; }
    //void Awake() { //instance = this; }

    public Image mask;
    float originalSize;
    float value;
    bool visible = true;

    public UIHealthBar(Image mask, bool visible)
    {
        this.mask = mask;
        this.visible = visible;
    }

    void Start()
    {
        value = 1f;
        this.originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        this.value = originalSize * value;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.value);
        //if(this.value == 1f) this.visible = true;
        //else this.visible = true;
    }

    public float GetValue(float value)
    {
        return this.value;
    }

    public bool GetVisibility()
    {
        return this.visible;
    }
    public void SetVisibility(bool visible)
    {
        this.visible = visible;
    }
}