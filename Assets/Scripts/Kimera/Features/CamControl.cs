using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Features
{
    public class CamControl :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        float camLerpDuration;
        float currentFury;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            camLerpDuration = settings.Search("camLerpDuration");

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        public void UpdateFeature(Controller controller)
        {
            FurryEntity furryEntity = controller as FurryEntity;

            currentFury = furryEntity.furryCount;

            ChangeFOV(currentFury);
        }

        void ChangeFOV(float fury)
        {
            float f = Mathf.InverseLerp(0, 100, fury);
            float fov = Mathf.Lerp(Camera_System.instance.defaultFOV, 100, f);
           

            Camera_System.instance.SetFOV(fov);
            //Camera_System.instance.FOVLerpInvoke(fov, camLerpDuration);
        }
    }
}