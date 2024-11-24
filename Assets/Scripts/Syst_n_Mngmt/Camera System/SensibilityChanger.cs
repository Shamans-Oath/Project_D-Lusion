using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SensibilityChanger : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI value;

    private void OnEnable()
    {
        if(Camera_System.instance != null)
        slider.value = Camera_System.instance.ReadSens();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sensRatio = Mathf.InverseLerp(Camera_System.instance.minSens, Camera_System.instance.maxSens, Camera_System.instance.cmp_playerCamera.m_XAxis.m_MaxSpeed);

        int sensValue = (int)Mathf.Lerp(0, 100, sensRatio);

        value.text = sensValue.ToString();
    }

    public void UpdateSens(float value)
    {
        if (Camera_System.instance != null)
        Camera_System.instance.UpdateSens(value);
    }
}
