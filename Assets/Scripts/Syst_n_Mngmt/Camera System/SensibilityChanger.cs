using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensibilityChanger : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        slider.value = Camera_System.instance.ReadSens();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSens(float value)
    {
        Camera_System.instance.UpdateSens(value);
    }
}
