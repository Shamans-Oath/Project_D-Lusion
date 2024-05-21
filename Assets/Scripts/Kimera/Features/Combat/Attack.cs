using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Attack :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
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

        public void StartAttackBox(AttackSwing attack)
        {
            if (!active || attack == null) return;

            actualAttack = attack;
            activeAttack = true;
            attackBox.SetBox(attack.size, attack.offset, attack.movement, true);
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