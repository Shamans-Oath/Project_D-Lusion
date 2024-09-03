using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Jump : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate, IFeatureAction
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        //States / Time Management
        [SerializeField] private float jumpTimer;
        //Properties
        [Header("Properties")]
        public float jumpForce;
        //Properties / Time Management
        public bool hasJumped;
        public float jumpCooldown;
        public bool reachedApex = false;
        //References
        public Gravity gravity;
        //Componentes
        [Header("Components")]
        [SerializeField] private Rigidbody cmp_rigidbody;

        private void Awake()
        {
            //Setup Components
            cmp_rigidbody = GetComponent<Rigidbody>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            jumpForce = settings.Search("jumpForce");
            jumpCooldown = settings.Search("jumpCooldown");

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if (jumpTimer > 0) jumpTimer -= Time.deltaTime;
            if (jumpTimer < 0) jumpTimer = 0;

            TerrainEntity terrain = controller as TerrainEntity;

            if (cmp_rigidbody.velocity.y < 0 && terrain.onGround == false && reachedApex == false && hasJumped == true)
            {
                reachedApex = true;


                if (gravity.gravityCoroutine != null)
                {
                    StopCoroutine(gravity.gravityCoroutine);
                    gravity.gravityCoroutine = null;
                }
                
                gravity.gravityCoroutine = StartCoroutine(gravity.ReturnGravity(jumpCooldown));
            }

            if (terrain.onGround == true && jumpTimer < 4 * jumpCooldown/5)
            {
                jumpTimer = 0;
                reachedApex = false;
                hasJumped = false;
            }
        }

        public void FeatureAction(Controller controller, params Setting[] settings)
        {
            TerrainEntity terrain = controller as TerrainEntity;

            if (!active) return;

            if(cmp_rigidbody == null) return;
            if (jumpTimer > 0) return;
            if (terrain.onGround == false) return;
            hasJumped = true;
            jumpTimer = jumpCooldown;
            cmp_rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);         
        }        

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }
    }
}

