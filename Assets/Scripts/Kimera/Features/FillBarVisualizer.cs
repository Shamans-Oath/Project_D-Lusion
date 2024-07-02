using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Features
{
    public class FillBarVisualizer :  MonoBehaviour, IActivable, IFeatureSetup,IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        [Tooltip("0 - to life | 1 to fury")]
        public ImageGroup[] uiBars;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;
            if (uiBars.Length <= 0) return;

            //Setup Properties

            //This is hardcode, it would be better to call specific array values from the controller
            //Life 
            if (uiBars[0]!=null) uiBars[0].maxValue = controller.SearchFeature<Life>().maxHealth;
            //Fury
            if (uiBars[1] != null) uiBars[1].maxValue = controller.SearchFeature<Furry>().furryMax;



            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void UpdateFeature(Controller controller)
        {
            FurryEntity furry = controller as FurryEntity;
            LivingEntity life = controller as LivingEntity;
            //Life 
            if (uiBars[0] != null) FillAmount(uiBars[0], life.currentHealth);
            //Fury
            if (uiBars[1] != null) FillAmount(uiBars[1], furry.furryCount);
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        public void FillAmount(ImageGroup imgGroup, float value)
        {

            imgGroup.progressBar.fillAmount = value / imgGroup.maxValue;

        }
        [System.Serializable]
        public class ImageGroup
        {
            public Image progressBar;
            public float maxValue;
        }
    }
}