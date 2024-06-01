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
        [Header("References")]
        public Life life;
        public Stun stun;
        //Componentes
        [Header("Components")]
        public Animator animator;

        private void Awake()
        {
            //Get References
            life = GetComponent<Life>();
            stun = GetComponent<Stun>();

            //Get Components
            if(animator == null) animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (life != null) life.OnDamage += ReactToDamage;
        }

        private void OnDisable()
        {
            if (life != null) life.OnDamage -= ReactToDamage;
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            disableTimeAfterHit = settings.Search("disableTimeAfterHit");

            ToggleActive(true);
        }

        private void ReactToDamage()
        {
            if (!active) return;

            if (stun != null) stun.StunSomeTime(disableTimeAfterHit);
            if (animator != null) animator.SetTrigger("isHurt");
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