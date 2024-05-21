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
            Furry furry = actor.SearchFeature<Furry>();

            Life reactorLife = reactor.SearchFeature<Life>();

            if (reactorLife == null)
            {
                Unlink();
                return;
            }

            int damage = actorCombat.attack;

            if(attack != null)
            {
                //Si hay settings de ataque
                if (int.TryParse(attack.Search("attackExtra"), out int extraAttack))
                {
                    damage += extraAttack;
                }
            }

            if (!reactorCombat.block && !reactorCombat.parry)
            {
                reactorLife.Health(-damage);
                if(furry != null) furry.IncreaseFurryCount();
                //Añadir efectos de ataque
                Unlink();
                return;
            }

            //Añadir efectos de bloques

            Unlink();
        }
    }
}