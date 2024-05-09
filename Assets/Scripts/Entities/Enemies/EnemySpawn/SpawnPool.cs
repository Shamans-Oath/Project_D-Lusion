using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnPool : MonoBehaviour
{
    public List<GameObject> pooledEnemies = new List<GameObject>();
    public GameObject[] enemiesToPool;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            UpdatePool(0, 5, "TestEnemy");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            UpdatePool(0, 8, "TestEnemy");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            UpdatePool(1, 4, "TestEnemy2");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdatePool(1, 7, "TestEnemy2");
        }
    }

    public void UpdatePool(int enemyIndex, int amountToPool, string enemyName)
    {
        if(pooledEnemies.Count > 0)
        {
            int alreadyPooled = 0;

            for (int i = 0; i < pooledEnemies.Count; i++)
            {
                if (pooledEnemies[i].name == enemyName)
                {
                    alreadyPooled++;
                }
            }

            GameObject tmp;
            for (int i = 0; i < amountToPool - alreadyPooled; i++)
            {
                tmp = Instantiate(enemiesToPool[enemyIndex]);
                tmp.name = enemyName;
                tmp.SetActive(false);
                pooledEnemies.Add(tmp);
            }
        }
        else
        {
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(enemiesToPool[enemyIndex]);
                tmp.name = enemyName;
                tmp.SetActive(false);
                pooledEnemies.Add(tmp);
            }
        }        
    }

    public GameObject GetPooledObject(string enemyName)
    {
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (pooledEnemies[i].name == enemyName && !pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }

        return null;
    }
}
