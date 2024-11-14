using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMCaller : MonoBehaviour
{
    public string Music;
    
    private void OnEnable()
    {
        if (AudioManager.instance != null)
        {
            PlayBGM();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM()
    {
        AudioManager.instance.MusicChanger(Music);
    }
}
