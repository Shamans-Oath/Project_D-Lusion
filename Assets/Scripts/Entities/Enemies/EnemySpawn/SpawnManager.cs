using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPool pool;
    public EncounterScriptable currentEncounter;

    public List<GameObject> remainingEnemies = new List<GameObject>();

    public bool isActive;

    float waveCooldown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEncounter()
    {
        waveCooldown = currentEncounter.timeBetweenWaves;

        for(int i = 0; i < currentEncounter.waves.Length; i++)
        {
            StartCoroutine(Spawning(currentEncounter.waves[i].enemyName, currentEncounter.waves[i].numberOfEnemies));
        }
    }

    IEnumerator Spawning(string enemyName ,int amountToSpawn)
    {
        float t = 0;

        while (true)
        {
            yield return null;
            SpawnEnemy(enemyName);

            t++;

            if (t > amountToSpawn)
            {
                break;
            }
        }
    }

    public void SpawnEnemy(string enemyName)
    {
        GameObject enemy = pool.GetPooledObject(enemyName);

        if(enemy != null)
        {
            //enemy.transform.position = position;
            enemy.SetActive(true);
        }
    }
}
