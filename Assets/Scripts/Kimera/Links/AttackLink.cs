using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class AttackLink : Link //Other Link channels
    {
        private const int LIFE_STEAL = 35;  

        //States
        //Properties

        public AttackLink(Controller actor, Controller reactor, Settings attack) : base(actor, reactor, attack)
		{
			//Link Features
            CombatEntity actorCombat = actor as CombatEntity;
            CombatEntity reactorCombat = reactor as CombatEntity;
            Rigidbody reactorRigidbody = reactor.gameObject.GetComponent<Rigidbody>();
            Furry furry = actor.SearchFeature<Furry>();
            FurryEntity furryEntity = actor as FurryEntity;

            CombatReactions actorReaction = actor.SearchFeature<CombatReactions>();
            Life reactorLife = reactor.SearchFeature<Life>();
            Life actorLife = actor.SearchFeature<Life>();

            Combat reactorCombatController = reactor.SearchFeature<Combat>();

            if (reactorLife == null)
            {
                Unlink();
                return;
            }

            if (reactorCombat == null)
            {
                Unlink();
                return;
            }

            int damage = actorCombat.attack;



            if(attack != null)
            {
                float? attackPotential = attack.Search("attackPotential");

                if (attackPotential.HasValue)
                {
                    damage = (int) (attackPotential * damage);
                }

                int? attackExtra = attack.Search("attackExtra");

                if (attackExtra.HasValue)
                {
                    damage += (int)attackExtra;
                }
            }

            //bool damaged = reactorCombat != null ? !reactorCombat.block && !reactorCombat.parry : true;

            if (!reactorCombat.block && !reactorCombat.parry)
            {
                //SR: Desactivado Temporalmente Para Probar
                //FMODManager.instance.LoadEvent("PlayerSounds", 0);
                //FMODManager.instance.PlayEvent();
                reactorLife.Health(-damage);
                if (furry != null) furry.IncreaseFurryCount();
                if (furryEntity != null) furryEntity.furryCombo++;
                //Añadir efectos de ataque

                if(attack != null)
                {
                    int? attackImpact = attack.Search("attackImpact");

                    if(attackImpact.HasValue)
                    {
                        reactorCombatController.PriorityBasedCancelAttack(attackImpact.Value);
                    }
                }

                if (actorReaction != null) actorReaction.PassTurn();

                if (attack != null)
                {
                    Vector3? attackKnockback = attack.Search("attackKnockback");

                    if (attackKnockback != null)
                    {
                        Vector3 direction = reactor.transform.position - actor.transform.position;
                        direction.Normalize();

                        AddAttackKnockback(reactorRigidbody, (Vector3)attackKnockback, direction);
                    }
                }

                //Efectos al matar enemigo
                if (reactorLife.CurrentHealth <= 0 && actorLife != null)
                {
                    if(attack != null)
                    {
                        int? lifeSteal = attack.Search("attackLifeSteal");

                        if (lifeSteal.HasValue)
                        {
                            actorLife.HealthPercentual(lifeSteal.Value, true);
                        }
                    }
                }

                Unlink();
                return;
            }
            else
            {
                if(reactorCombat.parry)
                {
                    //Debug.Log("TestCombate");
                    actor.SearchFeature<Combat>().StopAttack();
                    
                    reactor.SearchFeature<Friction>().ToggleActive(true);
                    reactor.SearchFeature<Block>().block = false;
                    reactor.SearchFeature<Block>().InvokeEnd();

                    Camera_System.instance.CameraShake("Parry");
                    reactor.SearchFeature<Combat>().attackCooldownTimer = 0;
                    reactor.CallFeature<CombatAnimator>(new Setting("combatCondition", "attack-counter", Setting.ValueType.String));
                    actor.SearchFeature<Stun>().StunSomeTime(reactor.settings.Search("parryStunDuration"));

                    if(reactor.SearchFeature<Furry>().furryCount > 80)
                    {
                        reactor.SearchFeature<Shield>().HalfShield();
                    }
                }
                else
                {
                    reactorLife.Health((int)(-damage * reactor.SearchFeature<Block>().blockingDamageMultiplier));
                }
            }

            //Añadir efectos de bloques

            Unlink();
        }

        private void AddAttackKnockback(Rigidbody reactorRigidbody, Vector3 attackKnockback, Vector3 direction)
        {
            if (reactorRigidbody == null || attackKnockback == Vector3.zero || direction == Vector3.zero) return;

            Vector3 knockbackInDirection = Vector3.Cross(direction, Vector3.up) * attackKnockback.x + Vector3.up * attackKnockback.y + direction * attackKnockback.z;

            reactorRigidbody.AddForce(knockbackInDirection, ForceMode.VelocityChange);
        }
    }
}