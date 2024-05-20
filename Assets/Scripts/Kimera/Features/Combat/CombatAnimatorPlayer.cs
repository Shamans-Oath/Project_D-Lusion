using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class CombatAnimatorPlayer :  MonoBehaviour, IActivable, IFeatureSetup, ICombatAnimator //Other channels
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
        //Properties
        [Header("Properties")]
        public float inputPermanenceTime;
        //References
        //Componentes

        private void Awake()
        {
            conditions = new Dictionary<string, bool>();
            conditions.Add("normalAttack", false);
            conditions.Add("specialAttack", false);
            conditions.Add("stop", false);
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            inputPermanenceTime = settings.Search("inputPermanenceTime");

            ToggleActive(true);
        }

        public void InputConditon(string condition)
        {
            if (!active) return;

            if (conditions.ContainsKey(condition))
            {
                StartCoroutine(FlipFlopInputCondition(condition));
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
            return false;
        }
    }
}