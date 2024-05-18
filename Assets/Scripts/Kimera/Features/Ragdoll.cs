using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
public class Ragdoll :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        //References
        //Componentes
        public Animator cmp_anim;
        public GameObject ragdolModel;
        public Collider[] cmp_normalCols;
        public Rigidbody[] cmp_normalRB;
        private Collider[] cmp_ragdollCols;
        private Rigidbody[] cmp_ragdollRB;

        private void Awake()
        {
            cmp_anim = GetComponent<Animator>();

            if (ragdolModel)
            {
                cmp_ragdollCols = ragdolModel.GetComponentsInChildren<Collider>();
                cmp_ragdollRB = ragdolModel.GetComponentsInChildren<Rigidbody>();
            }
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(false);
        }

        public void RagdollSetActive(bool active)
        {
            if (ragdolModel == null) return;

            if (cmp_anim) cmp_anim.enabled = !active;

            foreach (Collider col in cmp_ragdollCols)
            {
                col.enabled = active;
            }
            foreach (Rigidbody rig in cmp_ragdollRB)
            {
                rig.isKinematic = !active;
            }

            foreach (Collider col in cmp_normalCols)
            {
                col.enabled = !active;
            }
            foreach (Rigidbody rig in cmp_normalRB)
            {
                rig.isKinematic = active;
            }
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            RagdollSetActive(active);
        }
    }
}