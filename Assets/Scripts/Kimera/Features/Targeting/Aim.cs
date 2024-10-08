using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features
{
    public class Aim :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureFixedUpdate //Other channels
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
        public float aimRadius;
        public float aimMaxAngle;
        public string aimTag;
        public LayerMask aimLayer;
        //References
        //Componentes
        InputEntity input;
        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;
            input = controller as InputEntity;
            //Setup Properties
            aimRadius = settings.Search("aimRadius");
            aimMaxAngle = settings.Search("aimMaxAngle");
            aimTag = settings.Search("aimTag");
            aimLayer = 1 << LayerMask.NameToLayer(settings.Search("aimLayer"));

            var tempAimLayer2 = settings.Search("aimLayer2");
            if(tempAimLayer2 != null) aimLayer = aimLayer | 1 << LayerMask.NameToLayer(tempAimLayer2);

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void FixedUpdateFeature(Controller controller)
        {
            if (!active) return;

            FollowEntity follow = controller as FollowEntity;
            KeepCurrentTarget(follow);
            DetectTarget(follow);
        }

        private void DetectTarget(FollowEntity follow)
        {
            if (follow == null) return;

            if (follow.target != null) return;

            List<Collider> possibleTargets = Physics.OverlapSphere(transform.position, aimRadius, aimLayer).ToList();
            List<GameObject> temptativeTargets = new List<GameObject>();

            foreach (Collider target in possibleTargets)
            {
                if (!target.CompareTag(aimTag)) continue;

                GameObject entity = target.gameObject;

                if(entity == null) continue;

                float angleBetween = DistanceAngle(entity);
                
                if(angleBetween > aimMaxAngle / 2) continue;

                temptativeTargets.Add(entity);
            }

            GameObject closestTarget = SelectClosestAngleTarget(temptativeTargets);

            if (closestTarget == null) return;

            follow.target = closestTarget;
        }

        private void KeepCurrentTarget(FollowEntity follow)
        {
            if(follow == null) return;

            if (follow.target == null) return;

            GameObject targetController = follow.target;
            float angleToTarget = DistanceAngle(targetController);
            float distanceToTarget = DistanceBetween(targetController);

            if (distanceToTarget <= aimRadius && angleToTarget < aimMaxAngle / 2) return;

            follow.target = null;
        }

        private GameObject SelectClosestAngleTarget(List<GameObject> tempTargets)
        {
            if(tempTargets == null) return null;

            if(tempTargets.Count == 0) return null;
        
            GameObject closestTarget = tempTargets[0];
            float angleDistance = DistanceAngle(closestTarget);
            float distanceBetween = DistanceBetween(closestTarget);
            Vector2 direction = input.inputDirection;
            if (direction != Vector2.zero)
            {
                foreach (GameObject target in tempTargets)
                {
                    float angle = DistanceAngle(target);

                    if (angle > angleDistance) continue;

                    angleDistance = angle;
                    closestTarget = target;
                }
            }
            else
            {
                foreach (GameObject target in tempTargets)
                {
                    float distance = DistanceBetween(target);

                    if (distance > distanceBetween) continue;

                    distanceBetween = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        private float DistanceAngle(GameObject target)
        {
            if (target == null) return aimMaxAngle / 2;

            Vector3 directionToTarget = target.transform.position - transform.position;
            directionToTarget.y = 0;
            directionToTarget.Normalize();
            Vector2 direction = input.inputDirection;
            float angleBetween = 0;
            if (direction!=Vector2.zero)
            {
                Vector3 directValue = ProjectOnCameraFlattenPlane(new Vector3(direction.x, 0f, direction.y), input.playerCamera);
                angleBetween = Vector3.Angle(directValue, directionToTarget);
            }
            else
            {
                angleBetween = Vector3.Angle(transform.forward, directionToTarget);
            }

            return angleBetween;
        }

        private float DistanceBetween(GameObject target)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            directionToTarget.y = 0;

            return directionToTarget.magnitude;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aimRadius);
            
            //------------------
            Vector2 direction = input.inputDirection;
            Vector3 directValue = ProjectOnCameraFlattenPlane(new Vector3(direction.x, 0f, direction.y), input.playerCamera);

            Vector3 lineEndA = Vector3.zero;
            Vector3 lineEndB = Vector3.zero;
            if (direction != Vector2.zero)
            {
                Gizmos.color = Color.cyan;
                lineEndA = directValue * aimRadius;
                lineEndB = directValue * aimRadius;
            }
            else
            {
                Gizmos.color = Color.magenta;
                lineEndA = transform.forward * aimRadius;
                lineEndB = transform.forward * aimRadius;
            }

            Quaternion rotateA = Quaternion.Euler(0, aimMaxAngle/2, 0);
            Quaternion rotateB = Quaternion.Euler(0, -aimMaxAngle / 2, 0);

            lineEndA = rotateA * lineEndA;
            lineEndB = rotateB * lineEndB;
            lineEndA = lineEndA + transform.position;
            lineEndB = lineEndB + transform.position;
     
            Gizmos.DrawLine(transform.position, lineEndA);
            Gizmos.DrawLine(transform.position, lineEndB);


        }
        private Vector3 ProjectOnCameraFlattenPlane(Vector3 direction, Camera camera)
        {
            if (camera == null) return Vector3.zero;

            Vector3 cameraForward = camera.transform.forward;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);

            // Calculate the movement direction based on the camera's forward direction
            Vector3 projection = cameraForward * direction.x + cameraRight * direction.z;

            projection.y = 0;
            projection.Normalize();

            return projection;
        }
    }
}