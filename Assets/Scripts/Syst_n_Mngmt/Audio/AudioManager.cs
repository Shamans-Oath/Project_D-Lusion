using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] sounds;

    public AudioMixerGroup masterMixerGroup;

    public string lastBGM;

    public MixerValues[] mixerValues;

    public GameData gameData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.mixerGroup;
        }

        
    }

    void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVolume("MasterVolume", gameData.genVolume);
        SetVolume("MusicVolume", gameData.mscVolume);
        SetVolume("SFXVolume", gameData.sndVolume);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void Pause(string sound)
    {
        Sound s = System.Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.Log("The sound " + name + " couldn't be found");
            return;
        }

        s.source.Pause();
    }

    public void UnPause(string sound)
    {
        Sound s = System.Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.Log("The sound " + name + " couldn't be found");
            return;
        }

        s.source.UnPause();
    }

    public void TogglePause(bool toggle) 
    {
        if (toggle == true)
        {
            foreach (var sound in sounds)
            {
                sound.source.pitch = 0;
                sound.paused = true;
            }
        }
        else
        {
            foreach (var sound in sounds)
            {
                if(sound.paused)
                {
                    sound.source.pitch = 1;
                    sound.paused = false;
                }
            }
        }
    }

    public void ChangeVolume(string sound, float volume, float changeSpd)
    {
        Sound s = System.Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.Log("The sound " + name + " couldn't be found");
            return;
        }

        var currentVolume = s.source.volume;
        if (Mathf.Approximately(currentVolume, volume))
        {
            Debug.LogWarning($"Music {s.name} is already at volume {volume}");
            return;
        }

        var sign = -Mathf.Sign(currentVolume - volume);
        StartCoroutine(VolumeChanger(s.source, volume, changeSpd * sign));
    }

    private IEnumerator VolumeChanger(AudioSource source, float volume, float changeSpd)
    {
        if (volume == 0)
        {
            volume = 0.001f;
        }
        while (!Mathf.Approximately(source.volume, volume))
        {
            source.volume += Time.deltaTime * changeSpd;

            if (Mathf.Approximately(Mathf.Sign(changeSpd), Mathf.Sign(source.volume - volume)))
            {
                source.volume = volume;
                break;
            }

            yield return null;
        }
    }

    public void ToggleMute(string volumeKey)
    {
        MixerValues m = System.Array.Find(mixerValues, item => item.volumeKey == volumeKey);

        if (m == null)
        {
            Debug.Log("The mixer " + volumeKey + " couldn't be found");
            return;
        }

        if (m.muted == false)
        {
            SetVolume(volumeKey, 0);
            m.muted = true;
        }
        else
        {
            SetVolume(volumeKey, m.value);
            m.muted = false;
        }
    }

    public void SetVolume(string volumeKey, float value)
    {
        if (value == 0)
        {
            value = 0.001f;
        }

        masterMixerGroup.audioMixer.SetFloat(volumeKey, Mathf.Log10(value) * 20);

        switch(volumeKey)
        {
            case "MasterVolume":
                gameData.genVolume = value;                
            break;
            case "MusicVolume":
                gameData.mscVolume = value;
            break;
            case "SFXVolume":
                gameData.sndVolume = value;
            break;
        }

        gameData.Save();
    }

    public float ReadVolume(string volumeKey)
    {
        float volume = 0;

        switch (volumeKey)
        {
            case "MasterVolume":
                volume = gameData.genVolume;
            break;

            case "MusicVolume":
                volume = gameData.mscVolume;
            break;

            case "SFXVolume":
                volume = gameData.sndVolume;
            break;
        }

        return volume;
    }

    public void MusicChanger(string name)
    {
        Sound s = System.Array.Find(sounds, item => item.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " couldn't be found");
            return;
        }

        if (s.name != lastBGM)
        {
            ChangeVolume(lastBGM, 0, 0.2f);
            s.source.volume = 0;
            PlaySound(name);
            ChangeVolume(name, s.volume, 0.2f);
            lastBGM = name;
        }
    }
}

[System.Serializable]
public class MixerValues
{
    public string volumeKey;
    public float value;
    public bool muted;
}
