using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FightModule : MonoBehaviour
{
    public EncounterScriptable Info;
    public Transform[] spawnPoints;
    public Transform[] subModule;

    [field: SerializeField]
    public bool combatInProgress { get; set; }
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
            for (int i = 1; i <= Info.waves[0].numberOfBatches; i++)
            {
                if (i == 1)
                {
                    StartCoroutine(SpawnManager.instance.LoadEncounter(0, false, i, 0));
                }
                else
                {
                    StartCoroutine(SpawnManager.instance.LoadEncounter(0, true, i, (Info.timeBetweenBatches/ Info.waves[0].numberOfBatches) * i));
                }
            }
                
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
