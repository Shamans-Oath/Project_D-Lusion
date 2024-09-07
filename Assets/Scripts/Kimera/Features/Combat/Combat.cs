using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Features
{
    public class Combat :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, ISubcontroller //Other channels
    {
        private const float DEFAULT_ATTACK_COOLDOWN = 1.25f;

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
        [SerializeField] private float attackCooldownTimer;
        //Properties
        [Header("Properties")]
        public List<ComboPreset> defaultCombos;
        public int attack;
        public float attackCooldown;
        //References
        [Header("References")]
        public CombatAnimator combatAnimator;
        public List<Attack> possibleAttacks;
        public ISubcontroller movement;
        public ISubcontroller movementAI;
        public FaceTarget faceTarget;
        [Header("Components")]
        public Animator cmp_animator;


        private void Awake()
        {
            attackQueue = new Queue<AttackPreset>();

            //Setup References
            combatAnimator = GetComponent<CombatAnimator>();
            faceTarget = GetComponent<FaceTarget>();
            movement = GetComponent<Movement>() as ISubcontroller;
            movementAI = GetComponent<MovementModeSelector>() as ISubcontroller;

            //Setup Components
            if (cmp_animator == null) cmp_animator = GetComponent<Animator>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            attack = settings.Search("attack");

            defaultCombos = new List<ComboPreset>();

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

            float? tempAttackCooldown = settings.Search("attackCooldown");
            if (tempAttackCooldown.HasValue) attackCooldown = tempAttackCooldown.Value;
            else attackCooldown = DEFAULT_ATTACK_COOLDOWN;

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

            if (attackQueue.Count <= 0 && !activeAttack && attackTimer <= 0f && actualCombo != null)
            {
                actualCombo = null;
                attackCooldownTimer = attackCooldown;
            }

            if (actualCombo == null && attackCooldownTimer <= 0)
            {
                for (int i = 0; i < defaultCombos.Count; i++)
                {
                    var combo = defaultCombos[i];

                    if (conditions.Contains(combo.condition) && combo.attackChain.Length > 0)
                    {
                        actualCombo = combo;

                        for (int j = 0; j < combo.attackChain.Length; j++)
                        {
                            attackQueue.Enqueue(combo.attackChain[j]);
                        }
                        break;
                    }
                }
            } else if (actualCombo != null) {
                if (actualCombo.interruptions.Length > 0)
                {
                    for (int i = 0; i < actualCombo.interruptions.Length; i++)
                    {
                        var interruption = actualCombo.interruptions[i];

                        if (conditions.Contains(interruption.condition) && interruption.nextCombo != null)
                        {
                            actualCombo = interruption.nextCombo;

                            if (actualCombo.attackChain.Length == 0 || !conditions.Contains(actualCombo.condition)) continue;

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
        }

        public void UpdateFeature(Controller controller)
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;
            if(attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;

            if (!active) return;

            StartCombo(combatAnimator.GetActiveConditions());

            CombatEntity combat = controller as CombatEntity;
            if (combat != null) combat.attack = attack;

            activeAttack = false;
            possibleAttacks.ForEach(attack =>
            {
                if (attack.ActiveAttack) activeAttack = true;
            });

            if (!activeAttack && attackTimer <= .3f && attackCooldownTimer <= 0f && attackQueue.Count > 0 && combatAnimator.CheckCondition(actualCombo.condition))
            {
                SetupAttack(attackQueue.Dequeue());
                combat.comboCount++;
            }

            else if (attackTimer <= .05f && !activeAttack && actualAttack != null)
            {
                StopAttack();
                combat.comboCount = 0;
            }

            FurryEntity furry = controller as FurryEntity;

            if (furry != null)
            {
                cmp_animator.SetFloat("Blend", furry.furryCount / furry.maxFurryCount);
            }
        }


        public void SetupAttack(AttackPreset attack)
        {
            if(movement != null) movement.ToggleActiveSubcontroller(false);
            if(movementAI != null) movementAI.ToggleActiveSubcontroller(false);
            if (faceTarget != null) faceTarget.ToggleActive(true);

            actualAttack = attack;
            attackTimer = attack.animationClipHuman.length;
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController(cmp_animator.runtimeAnimatorController);
            animatorOverride["AnimTest1"] = attack.animationClipHuman;
            animatorOverride["AnimTest2"] = attack.animationClipBeast;
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
            if (i < 0 || i >= possibleAttacks.Count) return;

            if (actualAttack == null) return;

            possibleAttacks[i].EndAttackBox();
        }

        public void StopAttack()
        {
            //Debug.Log("TestStop" + this.gameObject);

            if (movement != null) movement.ToggleActiveSubcontroller(true);
            if (movementAI != null) movementAI.ToggleActiveSubcontroller(true);
            if (faceTarget != null) faceTarget.ToggleActive(false);

            for (int i = 0; i < possibleAttacks.Count; i++)
            {
                EndAttack(i);
            }

            actualAttack = null;
            actualCombo = null;
            attackCooldownTimer = attackCooldown;
            activeAttack = false;
            attackQueue.Clear();

            cmp_animator.SetBool("Attack", false);
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetAttack(int attack)
        {
            this.attack = attack;
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