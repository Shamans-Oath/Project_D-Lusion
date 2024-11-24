using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeHandler : MonoBehaviour
{
    public string VolumeKey;
    public Slider VolumeSlider;
    public TextMeshProUGUI value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sliderRatio = Mathf.InverseLerp(0, 1, VolumeSlider.value);
        int sliderValue = (int)Mathf.Lerp(0, 100, sliderRatio);

        if(value != null )
        value.text = sliderValue.ToString();
    }

    public void UpdateVolume(float value)
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(VolumeKey, value);
        }
    }
}
