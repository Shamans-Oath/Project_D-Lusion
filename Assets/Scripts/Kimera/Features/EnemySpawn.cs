using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class EnemySpawn :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        public void SpawnEnemy(int poolIndex, string enemyName, int spawnIndex)
        {
            if (SpawnManager.instance.currentModule != null)
            {
                SpawnManager.instance.SpawnEnemySingle(poolIndex, enemyName, spawnIndex);
            }
        }
    }
}