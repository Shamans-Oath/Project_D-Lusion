using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] sounds;


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

    // Start is called before the first frame update
    void Start()
    {

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

    public void ChangeVolume(string sound, float volume, float changeSpd)
    {
        Sound s = System.Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.Log("The sound " + name + " couldn't be found");
            return;
        }

        var currentVolume = s.volume;
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
}
