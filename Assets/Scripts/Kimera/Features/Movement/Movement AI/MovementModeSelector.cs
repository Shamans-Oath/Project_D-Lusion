using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Features
{
    public class MovementModeSelector :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureFixedUpdate, ISubcontroller //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private MovementMode activeMoveMode;
        //Properties
        [Header("Properties")]
        public string defaultMoveMode;
        public float reachDestinationDistance;
        //References
        [Header("References")]
        public Dictionary<string, MovementMode> moveModes;
        //Componentes
        [Header("Components")]
        public NavMeshAgent agent;
        public Rigidbody rb;

        private void Awake()
        {
            if(rb == null) rb = GetComponent<Rigidbody>();
            if(agent == null) agent = GetComponent<NavMeshAgent>();
            if (agent != null) agent.destination = transform.position;
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            defaultMoveMode = settings.Search("defaultMoveMode");
            reachDestinationDistance = settings.Search("reachDestinationDistance");

            ToggleActive(true);
        }

        private void Start()
        {
            //Setup Modes

            List<MovementMode> moveModesList = new List<MovementMode>(GetComponents<MovementMode>());
            moveModes = new Dictionary<string, MovementMode>();
            foreach (var moveMode in moveModesList)
            {
                moveModes.Add(moveMode.modeName, moveMode);
            }

            SetActiveMode(defaultMoveMode);
        }

        public void FixedUpdateFeature(Controller controller)
        {
            if(!active) return;

            FollowEntity follow = controller as FollowEntity;
            if(follow == null) return;

            MoveToMode(follow);
        }

        public void SetActiveMode(string mode)
        {
            if(!active || agent == null) return;

            if (!moveModes.ContainsKey(mode))
            {
                activeMoveMode = null;
                agent.destination = transform.position;
                agent.speed = 0f;
                return;
            }

            activeMoveMode = moveModes[mode];
            agent.speed = activeMoveMode.modeSpeed;
            agent.destination = transform.position;
        }

        public void MoveToMode(FollowEntity follow)
        {
            if(!active || agent == null || activeMoveMode == null) return;

            float distanceToTarget = Vector3.Distance(transform.position, agent.destination);

            if (distanceToTarget > reachDestinationDistance) return;

            agent.destination = activeMoveMode.RequestNextPoint(follow);
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            if (agent == null) return;

            agent.enabled = active;

            if(rb == null) return;

            rb.velocity = Vector3.zero;
        }

        public void ToggleActiveSubcontroller(bool active)
        {
            moveModes.Values.ToList().ForEach(mode => mode.ToggleActive(active));

            ToggleActive(active);
        }
    }
}