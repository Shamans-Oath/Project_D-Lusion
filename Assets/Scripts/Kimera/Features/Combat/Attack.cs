using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Attack :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, IFeatureFixedUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private bool activeAttack;
        [SerializeField] private bool uniqueEffectsTriggered = false;
        [SerializeField] private Vector3 playerForward;
        public bool ActiveAttack { get => activeAttack; }
        [SerializeField] private AttackSwing actualAttack;
        //Properties
        //References
        [Header("References")]
        public AttackBox attackBox;
        //Componentes
        [Header("Components")]
        [SerializeField] private Rigidbody playerRigidbody;

        private void Awake()
        {
            //Setup References
            attackBox = GetComponent<AttackBox>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(!active) return;
        
            InputEntity inputEntity = controller as InputEntity;
            if(inputEntity != null) playerForward = inputEntity.playerForward;
        }

        public void FixedUpdateFeature(Controller controller)
        {
            if (!active) return;

            if (actualAttack != null && activeAttack) AttackEffects(actualAttack.settings);
        }

        public void StartAttackBox(AttackSwing attack)
        {
            if (!active || attack == null) return;

            actualAttack = attack;
            activeAttack = true;
            attackBox.SetBox(attack.size, attack.offset, new Vector3(playerForward.x * attack.movement.x, attack.movement.y, playerForward.z * attack.movement.z), true);
            attackBox.SetAttack(attack.settings);
            uniqueEffectsTriggered = false;

            if (attack.settings != null) attack.settings.AssemblySettings();
        }

        private void AttackEffects(Settings attackSettings)
        {
            if (!active || attackSettings == null || playerRigidbody == null) return;

            //Contiuous Events During Swing
            AttackFollowForce(attackSettings);

            if (uniqueEffectsTriggered) return;
            uniqueEffectsTriggered = true;

            //Unique Events
            AttackImpulse(attackSettings);
            VerticalAttackImpulse(attackSettings);
        }

        private void AttackFollowForce(Settings attackSettings)
        {
            if (attackSettings == null) return;

            float? attackFollowMove = attackSettings.Search("attackFollowMove");

            if (attackFollowMove == null) return;

            if (attackFollowMove == 0) return;

            playerRigidbody.AddForce(transform.forward * (float)attackFollowMove);
        }

        private void AttackImpulse(Settings attackSettings)
        {
            if(attackSettings == null) return;

            float? attackImpulse = attackSettings.Search("attackImpulse");

            if(attackImpulse == null) return;

            if (attackImpulse == 0) return;

            playerRigidbody.AddForce(transform.forward * (float)attackImpulse, ForceMode.Impulse);
        }

        private void VerticalAttackImpulse(Settings attackSettings)
        {
            if (attackSettings == null) return;

            float? attackImpulse = attackSettings.Search("attackImpulseVertical");

            if (attackImpulse == null) return;

            if (attackImpulse == 0) return;

            playerRigidbody.AddForce(transform.up * (float)attackImpulse, ForceMode.Impulse);
        }

        public void EndAttackBox()
        {
            actualAttack = null;
            activeAttack = false;
            attackBox.SetBox();
            attackBox.SetAttack();
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