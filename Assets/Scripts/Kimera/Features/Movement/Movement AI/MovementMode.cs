using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public abstract class MovementMode :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        protected const int MAX_ITERATIONS = 100;

        //Configuration
        [Header("Settings")]
        public Settings settings;
        public string modeName;
        public float modeSpeed;
        //Control
        [Header("Control")]
        [SerializeField] protected bool active;
        //States
        //Properties
        [Header("Properties")]
        public LayerMask solidLayer;
        public float checkBlockRadius;
        //References
        //Componentes

        public virtual void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            solidLayer = 1 << LayerMask.NameToLayer(settings.Search("solidLayer"));
            checkBlockRadius = settings.Search("checkBlockRadius");

            ToggleActive(true);
        }

        public abstract Vector3 RequestNextPoint(FollowEntity follow);

        protected bool CheckBlocked(Vector3 position)
        {
            return Physics.OverlapSphere(position, checkBlockRadius, solidLayer).Length > 0;
        }

        protected Vector3 ToFloor(Vector3 position)
        {
            if(Physics.Raycast(position, -Vector3.up, out RaycastHit floorHit, Mathf.Infinity, solidLayer))
            {
                return floorHit.point;
            }
            return position;
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