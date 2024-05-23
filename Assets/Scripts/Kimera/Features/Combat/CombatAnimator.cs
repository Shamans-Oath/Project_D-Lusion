using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features
{
    public class CombatAnimator :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureAction, IFeatureUpdate, ICombatAnimator //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        private Dictionary<string, bool> conditions;
        private Dictionary<string, Coroutine> coroutinesInput;
        [SerializeField] private string currentCondition;
        //Properties
        [Header("Properties")]
        public float inputPermanenceTime;
        //References
        //Componentes

        private void Awake()
        {
            conditions = new Dictionary<string, bool>();
            coroutinesInput = new Dictionary<string, Coroutine>();
            conditions.Add("stop", false);
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            inputPermanenceTime = settings.Search("inputPermanenceTime");

            string condition1 = settings.Search("combatCondition1");
            string condition2 = settings.Search("combatCondition2");
            string condition3 = settings.Search("combatCondition3");
            string condition4 = settings.Search("combatCondition4");
            string condition5 = settings.Search("combatCondition5");

            if (condition1 != null) if (condition1 != string.Empty)
                {
                    conditions.Add(condition1, false);
                    coroutinesInput.Add(condition1, null);
                }
            if (condition2 != null) if (condition2 != string.Empty)
                {
                    conditions.Add(condition2, false);
                    coroutinesInput.Add(condition2, null);
                }
            if (condition3 != null) if (condition3 != string.Empty)
                {
                    conditions.Add(condition3, false);
                    coroutinesInput.Add(condition3, null);
                }
            if (condition4 != null) if (condition4 != string.Empty)
                {
                    conditions.Add(condition4, false);
                    coroutinesInput.Add(condition4, null);
                }
            if (condition5 != null) if (condition5 != string.Empty)
                {
                    conditions.Add(condition5, false);
                    coroutinesInput.Add(condition5, null);
                }

            ToggleActive(true);
        }

        public void FeatureAction(Controller controller, params Setting[] settings)
        {
            if (!active) return;

            if (settings.Length <= 0) return;

            try
            {
                string condition = settings[0].value as string;
                InputConditon(condition);
            }
            catch
            {
                Debug.LogError("The setting value must be a string");
            }
        }

        public void InputConditon(string condition)
        {
            if (!active) return;

            if (coroutinesInput.ContainsKey(condition))
            {
                if (coroutinesInput[condition] != null) StopCoroutine(coroutinesInput[condition]);
                coroutinesInput[condition] = StartCoroutine(FlipFlopInputCondition(condition));
            }
        }

        private IEnumerator FlipFlopInputCondition(string condition)
        {
            if (conditions.ContainsKey(condition))
            {
                conditions[condition] = true;
                yield return new WaitForSeconds(inputPermanenceTime);
                conditions[condition] = false;
            }
        }

        public List<string> GetActiveConditions()
        {
            List<string> activeCondtions = new List<string>();

            foreach (KeyValuePair<string, bool> condition in conditions)
            {
                if (condition.Value) activeCondtions.Add(condition.Key);
            }

            return activeCondtions;
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        public bool CheckCondition(string condition)
        {
            if(conditions.ContainsKey(condition))
            {
                return conditions[condition];
            }

            return false;
        }

        public void UpdateFeature(Controller controller)
        {
            currentCondition = ""; 
            GetActiveConditions().ForEach(x => currentCondition += $"{x} ");
        }
    }
}