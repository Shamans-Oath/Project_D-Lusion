using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;

namespace Features
{
    public class CameraTarget :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        [Header("Properties")]
        [SerializeField] float enemyWeightTarget;
        [SerializeField] float enemyRadius;
        [SerializeField] float radius;
        [SerializeField] LayerMask mask;
        [SerializeField] CinemachineTargetGroup targetGroup;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(true);
        }

        public void Update()
        {     
            DetectEnemy();
            
        }

        public void AddEnemy(Transform enemy)
        {
            if(targetGroup.FindMember(enemy) == -1)
            StartCoroutine(SmoothEntry(enemy));
        }

        public void RemoveEnemy(Transform enemy)
        {
            StartCoroutine(SmoothExit(enemy));
        }

        IEnumerator SmoothEntry(Transform enemy)
        {
            float enemyWeight = 0;
            targetGroup.AddMember(enemy, enemyWeight, enemyRadius);

            while(enemyWeight < enemyWeightTarget)
            {
                enemyWeight += Time.deltaTime * 0.5f;
                
                targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeight;
                targetGroup.DoUpdate();
                yield return null;
            }

            targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeightTarget;
            targetGroup.DoUpdate();
        }

        IEnumerator SmoothExit(Transform enemy)
        {
            float enemyWeight = enemyWeightTarget;
            
            while(enemyWeight > 0)
            {
                enemyWeight -= Time.deltaTime * 0.5f;
                
                targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeight;
                targetGroup.DoUpdate();
                yield return null;
            }

            targetGroup.RemoveMember(enemy);
        }

        public void DetectEnemy()
        {
            Collider[] enemyTarget = Physics.OverlapSphere(transform.position, radius, mask);

            foreach (Collider target in enemyTarget)
            {
                if((target.transform.position -transform.position).magnitude < radius)
                AddEnemy(target.transform);
                else
                RemoveEnemy(target.transform);
            }
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }
    }
}