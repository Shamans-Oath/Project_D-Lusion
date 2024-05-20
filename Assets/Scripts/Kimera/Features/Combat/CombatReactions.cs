using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class CombatReactions :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
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
        public float disableTimeAfterHit;
        //References
        public Life life;
        public Stun stun;
        //Componentes

        private void Awake()
        {
            life = GetComponent<Life>();
            stun = GetComponent<Stun>();
        }

        private void OnEnable()
        {
            life.OnDamage += () =>
            {
                if (active) stun.StunSomeTime(disableTimeAfterHit);
            };
        }

        private void OnDisable()
        {
            life.OnDamage -= () =>
            {
                if (active) stun.StunSomeTime(disableTimeAfterHit);
            };
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            disableTimeAfterHit = settings.Search("disableTimeAfterHit");

            ToggleActive(true);
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