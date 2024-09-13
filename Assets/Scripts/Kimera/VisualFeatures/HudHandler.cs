using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Features
{
    public class HudHandler :  MonoBehaviour, IActivable, IFeatureSetup,IFeatureUpdate //Other channels
    {
        private float hudCurrentFurry;
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        int currentHealth, maxHealth;
        float parryCooldown, parryCooldownTimer;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            parryCooldown = controller.SearchFeature<Block>().parryCooldown;
            //Setup Properties

            ToggleActive(true);
            //hudCurrentFurry=controller.SearchFeature<StatsHandler>();
        }
        public void UpdateFeature(Controller controller)
        {
            LivingEntity Life = controller as LivingEntity;

            currentHealth = Life.currentHealth;
            maxHealth = Life.maxHealth;
            HUDController.instance.UpdateLifeBar(currentHealth, maxHealth);

            parryCooldownTimer = controller.SearchFeature<Block>().parryCooldownTimer;
            HUDController.instance.UpdateParryCooldown(parryCooldownTimer, parryCooldown);
        }

        /*public void UpdateLife()
        {
            HUDController.instance.UpdateLifeBar(currentHealth, maxHealth);
        }*/

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