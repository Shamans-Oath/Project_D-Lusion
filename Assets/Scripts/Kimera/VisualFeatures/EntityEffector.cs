using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class EntityEffector : MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        public string hitFxKey;
        public string healFxKey;
        public string deadFxKey;
        //public string shieldBrkFxKey;

        private Controller ctrll;

        //States
        //Properties
        //References
        //Componentes
        private void OnEnable()
        {
            if(ElementInstancer.instance!=null)
            {
                if(ctrll.SearchFeature<Life>())
                {
                    Life lf = ctrll.SearchFeature<Life>();
                    Debug.Log("thisis the enaabble");
                    if (hitFxKey != "") lf.OnDamage += () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(hitFxKey),transform.position,transform);
                    if (healFxKey != "") lf.OnHeal += () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(healFxKey), transform.position, transform);
                    if (deadFxKey != "") lf.OnDeath += () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(deadFxKey), transform.position, transform);
                }

            }
        }
        private void OnDisable()
        {
            if (ElementInstancer.instance != null)
            {
                if (ctrll.SearchFeature<Life>())
                {
                    Life lf = ctrll.SearchFeature<Life>();
                    if (hitFxKey != "") lf.OnDamage -= () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(hitFxKey), transform.position, transform);
                    if (healFxKey != "") lf.OnHeal -= () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(healFxKey), transform.position, transform);
                    if (deadFxKey != "") lf.OnDeath -= () => ElementInstancer.instance.Generate(ElementInstancer.instance.GetObjectListValue(deadFxKey), transform.position, transform);
                }
            }
        }
        public void SetupFeature(Controller controller)
        {
            this.enabled = false;
            settings = controller.settings;
            ctrll = controller;
            this.enabled = true;
            //Setup Properties

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