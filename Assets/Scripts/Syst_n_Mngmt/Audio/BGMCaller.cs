using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMCaller : MonoBehaviour
{
    public string Music;
    public float Volume;

    // Start is called before the first frame update
    void Start()
    {
        if(AudioManager.instance != null)
        {
            PlayBGM();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM()
    {
        AudioManager.instance.MusicChanger(Music, Volume);
    }
}
