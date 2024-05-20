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

            Life reactorLife = reactor.SearchFeature<Life>();

            if (reactorLife == null)
            {
                Unlink();
                return;
            }

            int damage = actorCombat.attack;

            if (int.TryParse(attack.Search("attackExtra"), out int extraAttack))
            {
                damage += extraAttack;
            }

            if (!reactorCombat.block && !reactorCombat.parry)
            {
                reactorLife.Health(-damage);
            }

            Unlink();
        }
    }
}