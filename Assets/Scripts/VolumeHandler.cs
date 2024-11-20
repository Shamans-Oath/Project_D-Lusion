using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHandler : MonoBehaviour
{
    public string VolumeKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVolume(float value)
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(VolumeKey, value);
        }
    }
}
