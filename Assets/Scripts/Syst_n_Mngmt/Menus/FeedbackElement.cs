using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackElement : MonoBehaviour
{
    private Vector3 initialSize;
    private Color initialColor;

    private bool selected;
    [Range(1.0f,2.5f)]
    public float UpsizeVal = 1.15f;
    public TextMeshProUGUI textmshpr;
    public bool recolorText;
    public Color changeColor;
    private void Awake()
    {
        initialSize = transform.localScale;
        if (textmshpr) initialColor = textmshpr.color;
    }
    private void Update()
    {
        if (selected == true)
        {
            ChangeSize(UpsizeVal);
            if (recolorText == true) ChangeColr(changeColor);
            //ChangeColr(new Color(initialColor.r - 0.3f, initialColor.g - 0.3f, initialColor.b - 0.3f, initialColor.a));
        }
        else
        {
            ChangeSize(1);
            if (recolorText == true) ChangeColr(initialColor);
        }

    }
    public void MouseOver() => selected = true;

    public void MouseExit() => selected = false;

    void ChangeSize(float multiplyValue)
    {

        if (Mathf.Abs(Vector3.Distance(initialSize * multiplyValue, transform.localScale)) > 0.05)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialSize * multiplyValue, 0.3f);
        }

    }

    void ChangeColr(Color newColor)
    {
        if (textmshpr.color != newColor)
        {
            textmshpr.color = newColor;
        }

    }

    public void InvokeSound(string keySound)
    {
        if(AudioManager.instance)
        {
            AudioManager.instance.PlaySound(keySound);
        }
    }

}
