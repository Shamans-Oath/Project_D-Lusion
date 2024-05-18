using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public abstract class Hitbox : MonoBehaviour, IActivable, IFeatureSetup
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        [SerializeField] private Controller controller;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private Vector3 boxOffset;
        [SerializeField] private List<string> tagsToInteract;
        //Properties
        //References
        //Componentes
        [Header("Components")]
        //El Hitbox es hijo del punto de interacción
        [SerializeField] private BoxCollider cmp_collider;
        [SerializeField] private Rigidbody cmp_rigidbody;

        private void Awake()
        {
            tagsToInteract = new List<string>();
            if(cmp_collider != null) cmp_collider.isTrigger = true;
        }

        public virtual void SetupFeature(Controller controller)
        {
            settings = controller.settings;
            this.controller = controller;

            //Setup Properties

            ToggleActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(tagsToInteract.Contains(other.tag))
            {
                //Logica Link
                Controller otherController = other.GetComponent<Controller>();
                if (otherController != null) InteractEntity(otherController);
                //Logica otros sistemas
                else InteractObject(other.gameObject);
            }
        }

        public void SetBox(Vector3 boxSize = default(Vector3), Vector3 boxOffset = default(Vector3), Vector3 boxImpulse = default(Vector3), bool active = false)
        {
            ToggleActive(active);

            cmp_collider.size = boxSize;
            transform.localPosition = boxOffset;
            cmp_rigidbody.AddForce(boxImpulse, ForceMode.VelocityChange);
        }

        protected abstract void InteractEntity(Controller interactor);
        protected abstract void InteractObject(GameObject gameObject);

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            cmp_collider.enabled = active;
            cmp_rigidbody.isKinematic = !active;
        }
    }
}