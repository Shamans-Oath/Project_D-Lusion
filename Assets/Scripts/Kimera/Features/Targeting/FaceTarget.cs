using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class FaceTarget :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
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

            ToggleActive(false);
        }

        public void UpdateFeature(Controller controller)
        {
            if (!active) return;

            FollowEntity follow = controller as FollowEntity;
            InputEntity input = controller as InputEntity;

            FaceTo(follow, input);
        }

        private void FaceTo(FollowEntity follow, InputEntity input)
        {
            if(follow == null) return;

            if(follow.target == null) return;

            Vector3 directionToTarget = follow.target.transform.position;
            directionToTarget.y = transform.position.y;

            transform.LookAt(directionToTarget);

            if (input == null) return;

            input.playerForward = (directionToTarget - transform.position).normalized;
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