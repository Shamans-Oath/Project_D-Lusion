using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class PlayerAnimator : EntityAnimator
    {
        public bool OnIdle = false;

        public override void SetAnimator(Controller controller)
        {
            //arreglame
            if(gameObject.tag=="Enemy") cmp_animator=gameObject.GetComponent<Animator>();
            FurryEntity furry = controller as FurryEntity;
            KineticEntity kinetic = controller as KineticEntity;
            TerrainEntity terrain = controller as TerrainEntity;
            CombatEntity combatEntity = controller as CombatEntity;
            Combat combat = controller.SearchFeature<Combat>();

            if (kinetic == null || terrain == null || combat == null) return;

            cmp_animator.SetFloat("AttackFactor", combat.attackSpeedMultiplier);
            cmp_animator.SetBool("IsAttacking", combat.AttackTimer > combat.inBetweenAttacksTime);

            Vector3 speed = kinetic.speed;
            cmp_animator.SetFloat("VerticalSpeed", speed.y);
            speed.y = 0;
            cmp_animator.SetFloat("HorizontalSpeed", speed.magnitude);
            cmp_animator.SetFloat("WalkFactor", .5f + speed.magnitude / kinetic.maxSpeed);

            cmp_animator.SetBool("OnGround", terrain.onGround);

            if (furry == null || combatEntity == null) return;

            cmp_animator.SetFloat("FurryBlend", furry.furryCount / furry.maxFurryCount);
            cmp_animator.SetBool("IsBlocking", combatEntity.block || combatEntity.parry);

            /*if(cmp_animator.GetCurrentAnimatorStateInfo(0).IsName("IdleBlendTree") && OnIdle == false)
            {
                Debug.Log("TestAnimation");

                SoundLibrary soundLibrary = controller.GetComponent<SoundLibrary>();

                soundLibrary.CallAudioManager("Idle");
                OnIdle = true;
            }
            else
            {
                AudioManager.instance.Stop("IdleHumano");
                AudioManager.instance.Stop("IdleBestia");
                OnIdle = false;
            }*/
        }
    }
}


