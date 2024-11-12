using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FightModule : MonoBehaviour
{
    public EncounterScriptable Info;
    public Transform[] spawnPoints;
    public Transform[] subModule;
    public bool combatInProgress;
    public UnityEvent endEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadFight();
        }*/
    }

    public void LoadFight()
    {
        if(!combatInProgress)
        {
            combatInProgress = true;
            SpawnManager.instance.currentModule = this;
            SpawnManager.instance.currentEncounter = Info;
            SpawnManager.instance.currentWave = 0;
            SpawnManager.instance.waveCooldown = Info.timeBetweenWaves;
            SpawnManager.instance.ReadEncounter(0);
            SpawnManager.instance.isActive = true;
            StartCoroutine(SpawnManager.instance.LoadEncounter(0));
        }        
    }

    public void OnCompleteEvent()
    {
        endEvent.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LoadFight();
        }
    }
}
