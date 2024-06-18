using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class TimeFocusMode :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //Stats
        //Properties
        public bool instantModify;
        [Range(0.0f,1.0f)]
        public float duration;
        [Header("TimeScale")]
        public float overrideTimeScale;
        [Header("FOV")]
        public float overrideFOV;
        [Header("Target Lock")]
        public float overrideTargetDistance;
        public float overrideTargetRadius;
        public float overrideTargetAdjustSpeed;
        //References
        //Componentes
        public Camera_System camSys;

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }

        public void EnableFocus()
        {
            if (camSys)
            {
                if (instantModify) camSys.SetFOV(overrideFOV);
                else StartCoroutine(camSys.FOVLerp(overrideFOV, duration));

                if (camSys.lockSys)
                {
                    camSys.lockSys.distance = overrideTargetDistance;
                    camSys.lockSys.detectRadius = overrideTargetRadius;
                    camSys.lockSys.adjustmentSpeed = overrideTargetAdjustSpeed;
                }
            }
            if (instantModify) GameManager.SetTimeTo(overrideTimeScale);
            else GameManager.LerpTimeTo(overrideTimeScale,duration);

        }

        public void DisableFocus()
        {
            if (camSys)
            {
                if (instantModify) camSys.ResetFOV();
                else camSys.ResetFOVLerp(duration);

                if (camSys.lockSys)
                {
                    camSys.lockSys.ResetValues();
                }
            }
            if (instantModify) GameManager.ResetTime();
            else GameManager.LerpTimeTo(1, duration);
        }
        public void ToggleActive(bool active)
        {
            this.active = active;
        }
    }
}