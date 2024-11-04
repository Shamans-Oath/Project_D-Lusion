using Features;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public SoundReference[] sounds;
    Furry cmp_Furry;


    // Start is called before the first frame update
    void Start()
    {
        cmp_Furry = GetComponent<Furry>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallAudioManager(string name)
    {
        SoundReference s;

        if (cmp_Furry != null && cmp_Furry.furryCount > cmp_Furry.furryMax * 0.7f)
        {
            s = Array.Find(sounds, item => item.name == name + "Furro");
        }
        else
        {
            s = Array.Find(sounds, item => item.name == name);
        }
        

        if (AudioManager.instance)
        {
            AudioManager.instance.PlaySound(s.reference);
        }
            
    }
}

[System.Serializable]
public class SoundReference
{
    public string name;
    public string reference;
}
