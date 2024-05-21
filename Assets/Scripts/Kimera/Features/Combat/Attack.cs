using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Attack :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
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
        [SerializeField] private Vector3 playerForward;
        public bool ActiveAttack { get => activeAttack; }
        [SerializeField] private AttackSwing actualAttack;
        //Properties
        //References
        [Header("References")]
        public AttackBox attackBox;
        //Componentes

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

        public void StartAttackBox(AttackSwing attack)
        {
            if (!active || attack == null) return;

            actualAttack = attack;
            activeAttack = true;
            attackBox.SetBox(attack.size, attack.offset, new Vector3(playerForward.x * attack.movement.x, attack.movement.y, playerForward.z * attack.movement.z), true);
            attackBox.SetAttack(attack.settings);
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