using Features;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePhase : MonoBehaviour
{
    public class SpawnAndCharge : MonoBehaviour
    {
        public GameObject chargingPrefab;    // The prefab to spawn and charge
        public Transform[] spawnPoints;     // Array of 6 spawn points (assign in Inspector)
        public Transform player;           // Player transform for targeting
        public float chargeSpeed = 10f;
        public float chargeDelay = 1f;

        private List<GameObject> spawnedObjects = new List<GameObject>();
        private int currentChargerIndex = -1;

        private void Start()
        {
            StartSequence();
        }
        public void StartSequence()
        {
            SpawnChargingObjects();

            //StartCoroutine(ChargeSequence());
        }

        void SpawnChargingObjects()
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject spawnedObject = Instantiate(chargingPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedObjects.Add(spawnedObject);
            }
        }

       /* private System.Collections.IEnumerator ChargeSequence()
        {
            while (spawnedObjects.Count > 0)
            {
                int nextChargerIndex;
                do
                {
                    nextChargerIndex = Random.Range(0, spawnedObjects.Count);
                } 
                while (nextChargerIndex == currentChargerIndex && spawnedObjects.Count > 1);
                currentChargerIndex = nextChargerIndex;
                GameObject charger = spawnedObjects[currentChargerIndex];

                if (charger != null) 
                {
                    ChargeAtPlayer chargeScript = charger.GetComponent<ChargeAtPlayer>();
                    if (chargeScript != null)
                    {
                        chargeScript.StartCharge();
                        while (charger != null)
                        {
                            yield return null;
                        }
                    }
                }
            }
        }*/
    }
}
