using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public SpawnPool pool;
    public EncounterScriptable currentEncounter;
    public FightModule currentModule;

    public List<GameObject> remainingEnemies = new List<GameObject>();

    public bool isActive;

    [HideInInspector]
    public float waveCooldown;
    public int currentWave = 0;

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
        if(isActive)
        {
            if(remainingEnemies.Count == 0) 
            {
                if(currentWave == currentEncounter.waves.Length - 1)
                {
                    isActive = false;
                }
                else
                {
                    waveCooldown -= Time.deltaTime;

                    if (waveCooldown <= 0)
                    {
                        currentWave++;
                        waveCooldown = currentEncounter.timeBetweenWaves;
                        ReadEncounter(currentWave);
                        StartCoroutine(LoadEncounter(currentWave));
                    }
                }
            }
        }
    }

    public void ReadEncounter(int waveIndex)
    {
        for (int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            pool.UpdatePool(currentEncounter.waves[waveIndex].waveInfo[x].poolIndex, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies, currentEncounter.waves[waveIndex].waveInfo[x].enemyName);
        }
    }

    public IEnumerator LoadEncounter(int waveIndex)
    {
        for(int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies);
        }

        yield return new WaitForEndOfFrame();

        PlaceEnemies();
    }

    public void SpawnEnemy(string enemyName, int amountToSpawn)
    {
        for(int i = 0; i < amountToSpawn; i++)
        {
            GameObject enemy = pool.GetPooledObject(enemyName);

            if (enemy != null)
            {
                enemy.SetActive(true);
                remainingEnemies.Add(enemy);
            }
        }
        
    }

    public void PlaceEnemies()
    {
        int leftoverPoints = currentModule.spawnPoints.Length - remainingEnemies.Count;

        for (int i = 0; i < currentModule.spawnPoints.Length - leftoverPoints; ++i)
        {
            remainingEnemies[i].transform.position = currentModule.spawnPoints[i].position;
        }
    }
}
