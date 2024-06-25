using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class AttackLink : Link //Other Link channels
    {

        //States
        //Properties

        public AttackLink(Controller actor, Controller reactor, Settings attack) : base(actor, reactor, attack)
		{
			//Link Features
            CombatEntity actorCombat = actor as CombatEntity;
            CombatEntity reactorCombat = reactor as CombatEntity;
            Rigidbody reactorRigidbody = reactor.gameObject.GetComponent<Rigidbody>();
            Furry furry = actor.SearchFeature<Furry>();

            Life reactorLife = reactor.SearchFeature<Life>();

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
                int? attackExtra = attack.Search("attackExtra");

                if (attackExtra != null)
                {
                    damage += (int)attackExtra;
                }
            }

            //bool damaged = reactorCombat != null ? !reactorCombat.block && !reactorCombat.parry : true;

            if (!reactorCombat.block && !reactorCombat.parry)
            {
                reactorLife.Health(-damage);
                if(furry != null) furry.IncreaseFurryCount();
                //Añadir efectos de ataque
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


                Unlink();
                return;
            }
            else
            {
                actor.SearchFeature<Combat>().StopAttack();

                if(reactorCombat.parry)
                {
                    Debug.Log("TestCombate");
                    reactor.CallFeature<CombatAnimator>(new Setting("combatCondition", "attack-normal", Setting.ValueType.String));
                    reactor.SearchFeature<Block>().ChangeBlock(1);
                }
                else
                {
                    reactor.SearchFeature<Block>().ChangeBlock(-1);
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