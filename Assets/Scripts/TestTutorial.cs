using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class TestTutorial :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        float FuryValue;
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
            FurryEntity furry = controller as FurryEntity;

            FuryValue = furry.furryCount;

            if(tutorialTriggered == false && FuryValue >= furry.maxFurryCount/2)
            {
                CanvasManager.instance.ToggleTutorialPause("ComboPopup", "Bloody Combo");
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