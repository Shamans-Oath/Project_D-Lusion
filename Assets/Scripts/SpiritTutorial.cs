using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class SpiritTutorial :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        float Health;
        public bool tutorialTriggered;
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
            LivingEntity life = controller as LivingEntity;
            Life ftr_Life = controller.SearchFeature<Life>();


            Health = life.currentHealth;
            

            if(tutorialTriggered == false && Health < ftr_Life.lowHpIndicator)
            {
                CanvasManager.instance.ToggleTutorialPause("PausePopup", "Spiritual Strike", true);
                tutorialTriggered = true;
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
    }
}