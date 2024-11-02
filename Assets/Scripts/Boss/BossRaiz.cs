using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRaiz : MonoBehaviour
{
    public List<GameObject> childObjects;
    public Transform[] spawnPoints;
    public Transform playerPoint;
    public GameObject raizPrefab;
    public GameObject fakeRaizPrefab;
    public float spawnRadius = 5f;   
    public float spawnDelay = 2f;
    public Transform centerPoint;
    public float delayAdditive = 0.6f;
    public float minDistance =1.0f;

    void Start()
    {
        StartCoroutine(SpawnSequence());
        playerPoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private System.Collections.IEnumerator SpawnSequence()
    {
        RandomizeSpawnPoints();

        yield return new WaitForSeconds(spawnDelay);

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawnPoint = spawnPoints[randomIndex];
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject prefab = Instantiate(fakeRaizPrefab, spawnPoint.position, spawnPoint.transform.rotation,this.transform);
        }
        Instantiate(fakeRaizPrefab, playerPoint.position, playerPoint.transform.rotation,this.transform);
        ReplaceChild(Random.Range(0,childObjects.Count));
    }
    public void ReplaceChild(int indexToReplace)
    {
        PopulateChildList();
        if (childObjects == null || childObjects.Count == 0)
        {
            Debug.LogError("No hay hijitos :(");
            return;
        }

        if (raizPrefab == null)
        {
            Debug.LogError("No prefab");
            return;
        }


        if (indexToReplace >= 0 && indexToReplace < childObjects.Count)
        {
            Transform childToReplace = childObjects[Random.Range(0, childObjects.Count)].transform;
            GameObject newChild = Instantiate(raizPrefab, childToReplace.position, childToReplace.rotation);
            newChild.transform.parent = childToReplace.parent;
            childObjects.RemoveAt(indexToReplace);
            Destroy(childToReplace.gameObject);
            childObjects.Insert(indexToReplace, newChild);
        }
        else
        {

            Debug.LogError("Index error");
        }
    }


    void RandomizeSpawnPoints()
    {

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        centerPoint = transform; // Use this object's transform as the center
        List<Vector3> placedPositions = new List<Vector3>();


        foreach (Transform spawnPoint in spawnPoints)
        {
            bool positionFound = false;
            for (int i = 0; i < 10; i++)
            {
                float angle = Random.Range(0f, 2f * Mathf.PI);
                Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
                Vector3 newPosition = centerPoint.position + new Vector3(offset.x, 0f, offset.y);
                bool tooClose = false;
                foreach (Vector3 placedPos in placedPositions)

                {
                    if (Vector3.Distance(newPosition, placedPos) < minDistance)
                    {
                        tooClose = true;


                        break;
                    }
                }


                if (!tooClose)
                {

                    spawnPoint.position = newPosition;
                    placedPositions.Add(newPosition);
                    positionFound = true;
                    break;

                }

            }
            if (!positionFound)
            {
                Debug.LogWarning($"No se pudo poner {spawnPoint.name} luego de  10 intentos.");

            }

        }

    }
        public void PopulateChildList()
    {
        childObjects = new List<GameObject>();


        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Enemy")
            {
                childObjects.Add(child.gameObject);
                child.GetComponent<RaizAttack>().delay = delayAdditive;
                delayAdditive += 1f;
            }
            
        }
        if (childObjects.Count == 0)
        {
            Debug.LogWarning("No hay children");
        }
    }


}



