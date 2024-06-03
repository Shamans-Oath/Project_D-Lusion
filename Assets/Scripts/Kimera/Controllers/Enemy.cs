using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Enemy : Controller, LivingEntity, KineticEntity, TerrainEntity, SpecialTerrainEntity, FollowEntity
    {

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
            Destroy(gameObject);
        }
    }
}


