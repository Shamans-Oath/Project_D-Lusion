using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEditor;
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
    public int enemyThreshold;

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
            if(remainingEnemies.Count <= enemyThreshold) 
            {
                if(currentWave == currentEncounter.waves.Length - 1)
                {
                    isActive = false;
                    if (currentModule) currentModule.OnCompleteEvent();
                    currentModule = null;
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
        int enemyCount = remainingEnemies.Count;

        enemyThreshold = currentEncounter.waves[waveIndex].enemyThreshold;

        for (int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            pool.UpdatePool(currentEncounter.waves[waveIndex].waveInfo[x].poolIndex, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies + enemyCount, currentEncounter.waves[waveIndex].waveInfo[x].enemyName);
        }
    }

    public IEnumerator LoadEncounter(int waveIndex)
    {
        int enemyCount = remainingEnemies.Count;

        for(int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            if(!currentEncounter.waves[waveIndex].waveInfo[x].usesSubmodule)
            {
                SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies + enemyCount);
            }
            /*else
            {
                SpawnEnemySingle(currentEncounter.waves[waveIndex].waveInfo[x].poolIndex, currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentModule.subModule[currentEncounter.waves[waveIndex].waveInfo[x].submoduleIndex]);
            }*/
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
        int x = 0;

        for (int i = 0; i < remainingEnemies.Count; ++i)
        {
            if(x >= currentModule.spawnPoints.Length)
            {
                x = 0;
            }

            remainingEnemies[i].transform.position = currentModule.spawnPoints[x].position;
            x++;
        }
    }

    public GameObject SpawnEnemySingle(int poolIndex, string enemyName, int spawnIndex)
    {
        bool listCheck()
        {
            for (int i = 0; i < pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies.Count; i++)
            {
                if (!pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies[i].activeInHierarchy)
                {
                    return false;
                }
            }
            return true;
        }


        if (listCheck() == true)
        {
            pool.UpdatePool(poolIndex, pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies.Count + 1, enemyName);
        }

        GameObject enemy = pool.GetPooledObject(enemyName);

        if (enemy != null)
        {
            enemy.SetActive(true);
            remainingEnemies.Add(enemy);
        }

        enemy.transform.position = currentModule.spawnPoints[spawnIndex].position;

        return enemy;
    }

    public GameObject SpawnEnemySingle(int poolIndex, string enemyName, Transform spawnPoint)
    {
        bool listCheck()
        {
            for (int i = 0; i < pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies.Count; i++)
            {
                if (!pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies[i].activeInHierarchy)
                {
                    return false;
                }
            }
            return true;
        }


        if (listCheck() == true)
        {
            pool.UpdatePool(poolIndex, pool.poolList.Find((x) => x.listName == enemyName).pooledEnemies.Count + 1, enemyName);
        }

        GameObject enemy = pool.GetPooledObject(enemyName);

        if (enemy != null)
        {
            enemy.SetActive(true);
            remainingEnemies.Add(enemy);
        }

        enemy.transform.position = spawnPoint.position;

        return enemy;
    }

    public GameObject GetEnemy(int index)
    {
        return remainingEnemies[index];
    }
}
