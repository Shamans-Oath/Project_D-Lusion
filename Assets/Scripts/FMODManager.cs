using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;

public class FMODManager : MonoBehaviour
{
    public static FMODManager instance;

    public List<EventBank> eventBanks = new List<EventBank>();

    public FMODUnity.StudioEventEmitter emitter;
    void Awake()
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEvent(string name, int index)
    {
        for(int i = 0; i < eventBanks.Count; i++)
        {
            if (eventBanks[i].BankName == name)
            {
                emitter.EventReference = eventBanks[i].eventPaths[index];
            }
        }
    }

    public void PlayEvent()
    {
        emitter.Play();
    }

    public void StopEvent() 
    {
        emitter.Stop();
    }
}

[System.Serializable]
public class EventBank
{
    public string BankName;
    public EventReference[] eventPaths;
}