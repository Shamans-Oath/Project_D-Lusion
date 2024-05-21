using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Enemy : Controller, LivingEntity
    {
        public int currentHealth { get; set; }
        public int maxHealth { get; set; }

        public Animator anim;

        private void OnEnable()
        {
            SearchFeature<Life>().OnDeath += OnDeath;
            SearchFeature<Life>().OnDamage += Dummy; //S: Eliminar mas Adelante
        }

        private void OnDisable()
        {
            SearchFeature<Life>().OnDeath -= OnDeath;
            SearchFeature<Life>().OnDamage -= Dummy; //S: Eliminar mas Adelante
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }

        public void Dummy()
        {
            anim.SetTrigger("isHurt");
        }
    }
}


