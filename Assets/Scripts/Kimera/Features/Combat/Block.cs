using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Block :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        public bool block;
        public bool parry;
        //States / Time Management
        private float parryTimer;
        private float blockTimer;
        //Properties
        [Header("Properties")]
        public float parryTime;
        public float blockTime;
        //References
        [Header("References")]
        public CombatAnimatorPlayer combatAnimator;
        //Componentes

        private void Awake()
        {
            combatAnimator = GetComponent<CombatAnimatorPlayer>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            parryTime = settings.Search("parryTime");
            blockTime = settings.Search("blockTime");

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(parryTimer > 0) parryTimer -= Time.deltaTime;
            if (blockTimer > 0) blockTimer -= Time.deltaTime;

            if (!active) return;

            CombatEntity combat = controller as CombatEntity;
            if (combat != null)
            {
                combat.parry = parry;
                combat.block = block;
            }
        }

        public void StartBlock()
        {
            block = true;
            parry = true;
            blockTimer = blockTime;
            parryTimer = parryTime;
            combatAnimator.InputConditon("stop");
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