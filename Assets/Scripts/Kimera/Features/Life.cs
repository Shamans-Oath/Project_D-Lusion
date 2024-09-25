using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Life : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Events
        public event Action OnDamage;
        public event Action OnHeal;
        public event Action OnDeath;

        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        Controller cmp_Controller;
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private int currentHealth;
        public int CurrentHealth { get => currentHealth; }
        //Properties
        [Header("Properties")]
        public int maxHealth;
        //References

        //Componentes

        public void SetupFeature(Controller controller)
        {
            cmp_Controller = controller;
            settings = controller.settings;

            maxHealth = settings.Search("maxHealth");
            currentHealth = maxHealth;

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void UpdateFeature(Controller controller)
        {
            if (!active) return;

            LivingEntity life = controller as LivingEntity;
            if (life != null)
            {
                life.currentHealth = currentHealth;
                life.maxHealth = maxHealth;
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }

        public void Health(int amount, bool triggerEvents = true, bool ignoreBlock = false)
        {
            if (!active || amount == 0) return;

            int previousCurrentHealth = currentHealth;

            if(amount > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

                if (cmp_Controller.SearchFeature<Shield>() && !ignoreBlock)
                {
                    if (previousCurrentHealth + amount > maxHealth)
                    {
                        cmp_Controller.SearchFeature<Shield>().ModifyShield((previousCurrentHealth + amount) - maxHealth);
                    }
                }
            }
            else
            {
                if (cmp_Controller.SearchFeature<Shield>() && cmp_Controller.SearchFeature<Shield>().currentShield > 0 && !ignoreBlock)
                {
                    cmp_Controller.SearchFeature<Shield>().ModifyShield(amount);
                }
                else 
                {
                    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
                }
            }           

            if (!triggerEvents) return;

            int diff = currentHealth - previousCurrentHealth;            

            if (diff > 0) OnHeal?.Invoke();
            else if (diff < 0) OnDamage?.Invoke();

            if (currentHealth <= 0) OnDeath?.Invoke();
        }

        public void MaxHealth(int amount, bool readjust = true, bool triggerEvents = true)
        {
            if(!active || amount == 0) return;

            float previousMaxHealth = maxHealth;
            maxHealth = Mathf.Max(0, maxHealth + amount);

            if(readjust)
            {
                float readjustmentRatio = (previousMaxHealth != 0) ? currentHealth / previousMaxHealth : 0;
                int readJustHealth= (int)(maxHealth * readjustmentRatio);
                
                Health(readJustHealth - currentHealth, triggerEvents, true);
            }
        }

        public void HealthPercentual(int percentage, bool triggerEvents = true)
        {
            Health(PercentageToAmount(percentage), triggerEvents);
        }

        public void MaxHealthPercentual(int percentage, bool readjust = true, bool triggerEvents = true)
        {
            MaxHealth(PercentageToAmount(percentage), readjust, triggerEvents);
        }

        [ContextMenu("kill")]
        public void Kill()
        {
            currentHealth = 0;
            OnDeath?.Invoke();
        }

        private int PercentageToAmount(int percentage)
        {
            return (int)(maxHealth * Mathf.Clamp01((float)percentage / 100));
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }
    }
}

