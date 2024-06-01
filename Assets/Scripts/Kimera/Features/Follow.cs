using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Follow :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("State")]
        [SerializeField] private bool targetDetected = false;
        //Properties
        [Header("Properties")]
        public float detectDistance;
        public string targetTag;
        //References
        //Componentes
        [Header("Components")]
        GameObject targetGameObject;

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            detectDistance = settings.Search("detectDistance");
            targetTag = settings.Search("targetTag");

            targetGameObject = GameObject.FindGameObjectWithTag(targetTag);

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(!active) return;

            FollowEntity follow = controller as FollowEntity;
            if(follow == null) return;

            DetectTarget();
            UpdateTarget(follow);
        }

        private void DetectTarget()
        {
            if (!active || targetGameObject == null || targetDetected) return;
            
            float distanceToTarget = Vector3.Distance(transform.position, targetGameObject.transform.position);

            if (distanceToTarget < detectDistance)
            {
                targetDetected = true;
            }
        }

        private void UpdateTarget(FollowEntity follow)
        {
            if (!active) return;

            if (!targetDetected)
            {
                follow.target = null;
                return;
            }

            follow.target = targetGameObject;
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