using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Block :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, IFeatureAction //Other channels
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
        private float parryCooldownTimer;
        //Properties
        [Header("Properties")]
        public float parryTime;
        public float parryCooldown;
        //References
        [Header("References")]
        public CombatAnimator combatAnimator;
        public ISubcontroller movement;
        //Componentes

        private void Awake()
        {
            combatAnimator = GetComponent<CombatAnimator>();
            movement = GetComponent<Movement>() as ISubcontroller;
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            parryTime = settings.Search("parryTime");
            parryCooldown = settings.Search("parryCooldown");

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(parryTimer > 0) parryTimer -= Time.deltaTime;
            else if(parryTimer <= 0 && parry == true)
            {
                parry = false;
                InvokeEnd();
            }

            if (parryCooldownTimer > 0) parryCooldownTimer-= Time.deltaTime;

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
            if(parry == false && parryCooldownTimer <= 0)
            {               
                parry = true;
                block = true;
                parryTimer = parryTime;
                combatAnimator.InputConditon("stop");
            }                
        }

        public void EndBlock()
        {   
            if(block)
            {
                block = false;
                InvokeEnd();
            }            
        }

        public void InvokeEnd()
        {
            if(!parry && !block)
            {
                if (movement != null) movement.ToggleActiveSubcontroller(true);
                parryCooldownTimer = parryCooldown;
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

        public void FeatureAction(Controller controller, params Setting[] settings)
        {
            if (parryCooldownTimer > 0) return;
            if(settings.Length <= 0) return;

            bool value = settings[0].boolValue;

            if (value)
            {
                StartBlock();
                if (movement != null) movement.ToggleActiveSubcontroller(false);
                return;
            }
            
            EndBlock();
        }
    }
}