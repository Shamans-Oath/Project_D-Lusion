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
        int currentShield, maxShield;
        float specialCooldown, specialCooldownTimer;
        float dashCooldown, dashCooldownTimer;
        bool isDashing;
        float furryValue, maxFurry;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            specialCooldown = controller.SearchFeature<CooldownAction>().GetCooldownListValue("SpecialAtk").cooldownTime;
            dashCooldown = settings.Search("dashCooldownSeconds");
            //Setup Properties

            ToggleActive(true);
            //hudCurrentFurry=controller.SearchFeature<StatsHandler>();
        }
        public void UpdateFeature(Controller controller)
        {
            LivingEntity Life = controller as LivingEntity;
            FurryEntity furry = controller as FurryEntity;

            if(HUDController.instance != null)
            {
                currentHealth = Life.currentHealth;
                maxHealth = Life.maxHealth;
                HUDController.instance.UpdateLifeBar(currentHealth, maxHealth);

                currentShield = controller.SearchFeature<Shield>().currentShield;
                maxShield = controller.SearchFeature<Shield>().maxShield;
                HUDController.instance.UpdateShieldBar(currentShield, maxShield);

                specialCooldownTimer = controller.SearchFeature<CooldownAction>().GetCooldownListValue("SpecialAtk").timer;
                HUDController.instance.UpdateSpecialCooldown(specialCooldownTimer, specialCooldown);

                dashCooldownTimer = controller.SearchFeature<Dash>().realDashCooldown;
                isDashing = controller.SearchFeature<Dash>().IsDashing;
                if(!isDashing)
                {
                    HUDController.instance.UpdateDashCooldown(dashCooldownTimer, dashCooldown);
                }

                furryValue = furry.furryCount;
                maxFurry = furry.maxFurryCount;
                HUDController.instance.UpdateFuryImage(furryValue, maxFurry);
            }                
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