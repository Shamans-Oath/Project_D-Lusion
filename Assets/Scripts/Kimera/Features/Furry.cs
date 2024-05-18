using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Furry : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private float furryCount;
        [SerializeField] private float lastFurryPunchTime;
        //Properties
        [Header("Properties")]
        public float furryIncrement;
        public float furryDecrement;
        public float furryMax;
        //Properties / Time Management
        public float furryExtension;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            furryIncrement = settings.Search("furryIncrement");
            furryDecrement = settings.Search("furryDecrement");
            furryMax = settings.Search("furryMax");
            furryExtension = settings.Search("furryExtension");

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            FurryEntity furry = controller as FurryEntity;

            if (!active)
            {
                if(furry != null) furry.furryCount = 0;
                return;
            }

            if (furry != null) furry.furryCount = furryCount;

            if (lastFurryPunchTime + furryExtension < Time.time)
            {
                DecreaseFurryCount();
            }
        }

        public void IncreaseFurryCount()
        {
            if(!active) return;

            furryCount += furryIncrement;
            furryCount = Mathf.Clamp(furryCount, 0, furryMax);
            lastFurryPunchTime = Time.time;
        }

        private void DecreaseFurryCount()
        {
            furryCount -= furryDecrement * Time.deltaTime;
            furryCount = Mathf.Clamp(furryCount, 0, furryMax);
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            if (active) return;

            furryCount = 0;
        }
    }
}

