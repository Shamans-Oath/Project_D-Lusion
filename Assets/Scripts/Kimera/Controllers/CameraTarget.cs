using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;

namespace Features
{
    public class CameraTarget :  MonoBehaviour
    {
        [SerializeField] float enemyWeightTarget;
        [SerializeField] float enemyRadius;
        [Header("Detection Enemies")]
        [SerializeField] float radiusDetectEnemy;
        [SerializeField] float angle;
        [SerializeField] LayerMask mask;
        [SerializeField] CinemachineTargetGroup targetGroup;
        [SerializeField] Camera camera;

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
                
                if(targetGroup.FindMember(enemy) != -1)
                {
                    targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeight;
                    targetGroup.DoUpdate();
                }
                else
                {
                    enemyWeight = enemyWeightTarget+0.1f;
                }
                yield return null;
            }

            if(targetGroup.FindMember(enemy) != -1)
            targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeightTarget;
            targetGroup.DoUpdate();
        }

        IEnumerator SmoothExit(Transform enemy)
        {
            float enemyWeight = enemyWeightTarget;
            
            while(enemyWeight > 0)
            {
                enemyWeight -= Time.deltaTime * 0.5f;

                if(targetGroup.FindMember(enemy) != -1)
                {
                    targetGroup.m_Targets[targetGroup.FindMember(enemy)].weight = enemyWeight;
                    targetGroup.DoUpdate();
                }
                else
                {
                    enemyWeight = -1;
                }
                yield return null;
            }

            if(targetGroup.FindMember(enemy) != -1)
            targetGroup.RemoveMember(enemy);
        }

        public void DetectEnemy()
        {
            Collider[] enemyTarget = Physics.OverlapSphere(transform.position, radiusDetectEnemy, mask);
            

            foreach (Collider target in enemyTarget)
            {
                float distance = (target.transform.position -transform.position).magnitude;
                if(distance < radiusDetectEnemy)
                {
                    float angleTarget = Vector3.Angle(camera.transform.forward,target.transform.position-camera.transform.position);
                    if(angleTarget < angle-5)
                    AddEnemy(target.transform);
                    else if(angleTarget > angle+5)
                    RemoveEnemy(target.transform);
                }
                else
                RemoveEnemy(target.transform);
            }
        }
    }
}