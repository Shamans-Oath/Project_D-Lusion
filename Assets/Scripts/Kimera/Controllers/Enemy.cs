using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Enemy : Controller, LivingEntity, KineticEntity, TerrainEntity, SpecialTerrainEntity, FollowEntity, CombatEntity
    {
        private const float DESPAWN_TIME = 5.5f;

        //Living
        public int currentHealth { get; set; }
        public int maxHealth { get; set; }

        //Kinetic
        public float currentSpeed { get; set; }

        //Terrain
        public bool onGround { get; set; }
        public bool onSlope { get; set; }

        public bool onLadder { get; set; }

        //Follow
        public GameObject target {  get; set; }
        //Combat
        public bool block { get; set; }
        public bool parry { get; set; }
        public int attack {  get; set; }

        private void OnEnable()
        {
            SearchFeature<Life>().OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            SearchFeature<Life>().OnDeath -= OnDeath;
        }

        public void OnDeath()
        {
            CallFeature<Ragdoll>(new Setting("ragdollActivation", true, Setting.ValueType.Bool));
            ToggleActive(false);
            Invoke("ReanimateAndSave", DESPAWN_TIME);
        }

        public void ReanimateAndSave()
        {
            ToggleActive(true);
            CallFeature<Ragdoll>(new Setting("ragdollActivation", false, Setting.ValueType.Bool));
            gameObject.SetActive(false);
        }
    }
}


