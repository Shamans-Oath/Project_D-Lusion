using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace Features
{
    public class HeadRotation :  MonoBehaviour, IActivable, IFeatureSetup,IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("Properties")]
        [SerializeField] float angleMin;
        [SerializeField] TargetLock targetLock;
        [SerializeField] AimConstraint aimConstraint;
        ConstraintSource currentConstraint;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            //angleMin = settings.Search("angleMin");
            ToggleActive(true);
        }

        public void RotateHead()
        {
            if(targetLock.currentTarget != null)
            {
                currentConstraint = new ConstraintSource { sourceTransform = targetLock.currentTarget, weight = 1f };
                float angleTarget = Vector3.Angle(transform.forward,targetLock.currentTarget.position-transform.position);
                Debug.Log(angleTarget);
                if(angleTarget < angleMin)
                {    
                    if (aimConstraint.sourceCount == 0)
                    aimConstraint.AddSource(currentConstraint);
                }
                else
                {
                    if (aimConstraint.sourceCount != 0)
                    aimConstraint.RemoveSource(0);
                }
            }
            else
            {
                if (aimConstraint.sourceCount != 0)
                aimConstraint.RemoveSource(0);
            }
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
            RotateHead();
        }
    }
}