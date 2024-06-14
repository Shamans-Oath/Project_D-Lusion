using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Features
{
    public class Rotation : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        private float angularSpeed;
        public float AngularSpeed { get { return angularSpeed; } }
        [SerializeField] private bool isRotating;
        public bool IsRotating { get { return isRotating; } }
        private Coroutine accelerating;
        //Properties
        [Header("Properties")]
        public float startRotationSpeed;
        public float maxRotationSpeed;
        public float acceleration;
        public AnimationCurve rotationSpeedCurve;
        //References
        //Componentes
        [Header("Components")]
        [SerializeField] private NavMeshAgent agent;

        private void Awake()
        {
            if(agent == null) agent = GetComponent<NavMeshAgent>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            startRotationSpeed = settings.Search("startRotationSpeed");
            maxRotationSpeed = settings.Search("maxRotationSpeed");
            acceleration = settings.Search("rotationAcceleration");
            rotationSpeedCurve = settings.Search("rotationSpeedCurve");

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if (!active) return;

            InputEntity input = controller as InputEntity;
            if (input != null)
            {
                Vector3 forward = input.playerForward;
                RotateTo(forward);
                return;
            }

            if (agent != null)
            {
                if (agent.enabled)
                {
                    Vector3 forward = agent.destination - transform.position;
                    forward.y = 0f;
                    forward = forward.normalized;

                    RotateTo(forward);
                    return;
                }
            }

            FollowEntity follow = controller as FollowEntity;
            if(follow != null)
            {
                if(follow.target != null)
                {
                    Vector3 forward = follow.target.transform.position - transform.position;
                    forward.y = 0f;
                    forward.Normalize();

                    RotateTo(forward);
                    return;
                }
            }
        }

        public void RotateTo(Vector3 forward)
        {
            if (!active) return;

            //if the player is not rotating the rotation is stopped an the speed ceases to increase
            if (forward == Vector3.zero)
            {
                isRotating = false;
                return;
            }

            //If the player is not rotating, the rotation speed is set to the start rotation speed
            if (!isRotating)
            {
                angularSpeed = startRotationSpeed;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), angularSpeed * Time.deltaTime);

            RotateSpeed();

            isRotating = true;
        }

        private void RotateSpeed()
        {
            if (isRotating) return;

            accelerating = StartCoroutine(RotationOverTime());
        }

        private IEnumerator RotationOverTime()
        {
            float speedSpan = maxRotationSpeed - startRotationSpeed;

            float time = speedSpan / acceleration;
            float elapsedTime = 0;

            while (isRotating && elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / time;

                angularSpeed = rotationSpeedCurve.Evaluate(t) * speedSpan + startRotationSpeed;

                yield return null;
            }

            angularSpeed = maxRotationSpeed;
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

