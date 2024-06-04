using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Features
{
    public class Combat :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, ISubcontroller //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField]
        private bool activeAttack;
        public bool ActiveAttack { get => activeAttack; }
        [SerializeField] private AttackPreset actualAttack;
        [SerializeField] private Queue<AttackPreset> attackQueue;
        [SerializeField] private ComboPreset actualCombo;
        //States / Time Management
        [SerializeField] private float attackTimer;
        //Properties
        [Header("Properties")]
        public List<ComboPreset> defaultCombos;
        public int attack;
        //References
        [Header("References")]
        public CombatAnimator combatAnimator;
        public List<Attack> possibleAttacks;
        public ISubcontroller movement;
        [Header("Components")]
        public Animator cmp_animator;


        private void Awake()
        {
            defaultCombos = new List<ComboPreset>();
            attackQueue = new Queue<AttackPreset>();

            //Setup References
            combatAnimator = GetComponent<CombatAnimator>();

            //Setup Components
            cmp_animator = GetComponent<Animator>();
            movement = GetComponent<Movement>() as ISubcontroller;
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            attack = settings.Search("attack");

            ComboPreset combo1 = settings.Search("defaultCombo1") as ComboPreset;
            ComboPreset combo2 = settings.Search("defaultCombo2") as ComboPreset;
            ComboPreset combo3 = settings.Search("defaultCombo3") as ComboPreset;
            ComboPreset combo4 = settings.Search("defaultCombo4") as ComboPreset;
            ComboPreset combo5 = settings.Search("defaultCombo5") as ComboPreset;

            if (combo1 != null) defaultCombos.Add(combo1);
            if (combo2 != null) defaultCombos.Add(combo2);
            if (combo3 != null) defaultCombos.Add(combo3);
            if (combo4 != null) defaultCombos.Add(combo4);
            if (combo5 != null) defaultCombos.Add(combo5);

            ToggleActive(true);
        }

        public void StartCombo(List<string> conditions)
        {
            if (!active) return;

            if (conditions.Contains("stop"))
            {
                StopAttack();
                return;
            }

            if (actualCombo == null)
            {
                for(int i = 0; i < defaultCombos.Count; i++)
                {
                    var combo = defaultCombos[i];

                    if(conditions.Contains(combo.condition) && combo.attackChain.Length > 0)
                    {
                        actualCombo = combo;
                        
                        for(int j = 0; j < combo.attackChain.Length; j++)
                        {
                            attackQueue.Enqueue(combo.attackChain[j]);
                        }
                        break;
                    }
                }
            } else if (actualCombo.interruptions.Length > 0)
            {
                for (int i = 0; i < actualCombo.interruptions.Length; i++)
                {
                    var interruption = actualCombo.interruptions[i];

                    if (conditions.Contains(interruption.condition) && interruption.nextCombo != null)
                    {
                        actualCombo = interruption.nextCombo;

                        if(actualCombo.attackChain.Length == 0 || !conditions.Contains(actualCombo.condition)) continue;

                        attackQueue.Clear();

                        for (int j = 0; j < actualCombo.attackChain.Length; j++)
                        {
                            attackQueue.Enqueue(actualCombo.attackChain[j]);
                        }
                        break;
                    }
                }
            }
        }

        public void UpdateFeature(Controller controller)
        {
            if(attackTimer > 0) attackTimer -= Time.deltaTime;

            if (!active) return;

            StartCombo(combatAnimator.GetActiveConditions());

            CombatEntity combat = controller as CombatEntity;
            if (combat != null) combat.attack = attack;

            activeAttack = false;
            possibleAttacks.ForEach(attack =>
            {
                if (attack.ActiveAttack) activeAttack = true;
            });

            if(!activeAttack && attackTimer <= .3f && attackQueue.Count > 0 && combatAnimator.CheckCondition(actualCombo.condition))
            {
                SetupAttack(attackQueue.Dequeue());
            } 

            else if(attackTimer <= .2f && !activeAttack && actualAttack != null && !combatAnimator.CheckCondition(actualCombo.condition))
            {
                StopAttack();
            }
        }


        public void SetupAttack(AttackPreset attack)
        {
            if(movement != null) movement.ToggleActiveSubcontroller(false);

            actualAttack = attack;
            attackTimer = attack.animationClip.length;
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController(cmp_animator.runtimeAnimatorController);
            animatorOverride["Box"] = attack.animationClip;
            cmp_animator.runtimeAnimatorController = animatorOverride;
            cmp_animator.SetBool("Attack", true);
        }

        public void StartAttack(int i)
        {
            if (!active || i < 0 || i >= possibleAttacks.Count) return;
            if (actualAttack == null) return;
            possibleAttacks[i].StartAttackBox(actualAttack.swings[i]);
        }

        public void EndAttack(int i)
        {
            if (!active || i < 0 || i >= possibleAttacks.Count) return;

            if (actualAttack == null) return;

            possibleAttacks[i].EndAttackBox();
        }

        private void StopAttack()
        {
            if (movement != null) movement.ToggleActiveSubcontroller(true);

            actualAttack = null;
            actualCombo = null;
            activeAttack = false;
            attackQueue.Clear();

            cmp_animator.SetBool("Attack", false);
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            if (active) return;

            StopAttack();
        }

        public void ToggleActiveSubcontroller(bool active)
        {
            ToggleActive(active);
        }
    }
}