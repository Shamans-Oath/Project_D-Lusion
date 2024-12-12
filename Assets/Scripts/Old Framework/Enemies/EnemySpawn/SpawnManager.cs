using Features;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
    int tempEnemies;
    public int spawnNumber;

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
        if (isActive)
        {
            if (remainingEnemies.Count <= enemyThreshold)
            {
                if (currentWave == currentEncounter.waves.Length)
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
                        for (int i = 1; i <= currentEncounter.waves[currentWave].numberOfBatches; i++)
                        {
                            if (i == 1)
                            {
                                StartCoroutine(LoadEncounter(currentWave, false, i, 0));
                            }
                            else
                            {
                                StartCoroutine(LoadEncounter(currentWave, true, i, (currentEncounter.timeBetweenBatches / currentEncounter.waves[currentWave].numberOfBatches) * i));
                            }
                        }

                    }
                }
            }
        }
    }

    public void ReadEncounter(int waveIndex)
    {
        int enemyCount = remainingEnemies.Count;

        enemyThreshold = currentEncounter.waves[waveIndex].enemyThreshold;

        spawnNumber = 0;

        for (int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            pool.UpdatePool(currentEncounter.waves[waveIndex].waveInfo[x].poolIndex, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies + enemyCount, currentEncounter.waves[waveIndex].waveInfo[x].enemyName);
        }
    }

    public IEnumerator LoadEncounter(int waveIndex, bool ignoreTemp, int batchNumber, float batchTimer)
    {
        int temp = 0;

        if (ignoreTemp == true)
        {
            temp = 0;
        }
        else
        {
            temp = remainingEnemies.Count;
        }

        tempEnemies = remainingEnemies.Count;

        /*for (int i = 1; i <= numberOfBatches; i++)
        {
            for (int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
            {
                if (!currentEncounter.waves[waveIndex].waveInfo[x].usesSubmodule)
                {
                    if(i == 1)
                    {
                        SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies/numberOfBatches + tempEnemies);
                    }
                    else
                    {
                        SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies / numberOfBatches * i);
                    }

                    PlaceEnemies(i);
                }
            }

            yield return new WaitForEndOfFrame();

            //PlaceEnemies(i);          

            yield return new WaitForSeconds(1f);
        }*/
        if (remainingEnemies.Count > 0)
        {
            yield return new WaitForSeconds(batchTimer);
        }

        int numberOfBatches = currentEncounter.waves[waveIndex].numberOfBatches;

        for (int x = 0; x < currentEncounter.waves[waveIndex].waveInfo.Length; x++)
        {
            int enemiesToSpawn = currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies / numberOfBatches;

            Debug.Log(enemiesToSpawn);

            if (enemiesToSpawn != 0)
            {
                SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, (enemiesToSpawn * batchNumber) + temp);
            }
            else
            {
                SpawnEnemy(currentEncounter.waves[waveIndex].waveInfo[x].enemyName, currentEncounter.waves[waveIndex].waveInfo[x].numberOfEnemies);
            }
        }

        yield return new WaitForEndOfFrame();

        //PlaceEnemies();


    }

    public void SpawnEnemy(string enemyName, int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject enemy = pool.GetPooledObject(enemyName);

            if (enemy != null)
            {
                if (spawnNumber >= currentModule.spawnPoints.Length)
                {
                    spawnNumber = 0;
                }

                enemy.transform.position = currentModule.spawnPoints[spawnNumber].position;
                enemy.SetActive(true);
                remainingEnemies.Add(enemy);

                spawnNumber++;


            }

            /*if(remainingEnemies.Count != amountToSpawn)
            {
                remainingEnemies.Add(enemy);
            }*/
        }

    }

    public void PlaceEnemies()
    {
        int totalBatches = currentEncounter.waves[currentWave].numberOfBatches;


        for (int i = 0 + tempEnemies; i < remainingEnemies.Count; ++i)
        {
            if (spawnNumber >= currentModule.spawnPoints.Length)
            {
                spawnNumber = 0;
            }

            remainingEnemies[i].transform.position = currentModule.spawnPoints[spawnNumber].position;
            spawnNumber++;
        }

        /*if (batchNumber == 1)
        {
            Debug.Log(remainingEnemies.Count);            

            for (int i = 0 + tempEnemies; i < remainingEnemies.Count/totalBatches; ++i)
            {
                if (spawnNumber >= currentModule.spawnPoints.Length)
                {
                    spawnNumber = 0;
                }

                remainingEnemies[i].transform.position = currentModule.spawnPoints[spawnNumber].position;
                spawnNumber++;
            }
        }
        else
        {
            Debug.Log(remainingEnemies.Count);

            for (int i = remainingEnemies.Count / totalBatches * (batchNumber - 1); i < remainingEnemies.Count/totalBatches * batchNumber; ++i)
            {
                Debug.Log("Current iteration: " + i);
                Debug.Log(remainingEnemies[i].name);

                if (spawnNumber >= currentModule.spawnPoints.Length)
                {
                    spawnNumber = 0;
                }

                remainingEnemies[i].transform.position = currentModule.spawnPoints[spawnNumber].position;
                spawnNumber++;
            }
        } */
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

    public void CancelEncounter()
    {
        isActive = false;
        GameObject[] list = remainingEnemies.ToArray();

        foreach (GameObject obj in list)
        {
            obj.SetActive(false);
        }
        currentEncounter = null;
        currentModule = null;

    }
}
