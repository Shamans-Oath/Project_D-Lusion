using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class EntityEffector : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, IFeatureAction //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if (!active) return;

            
        }

        public void FeatureAction(Controller controller, params Setting[] settings)
        {
            if (!active) return;

           
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