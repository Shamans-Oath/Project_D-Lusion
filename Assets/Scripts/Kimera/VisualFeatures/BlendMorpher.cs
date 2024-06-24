using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class BlendMorpher :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        public SkinnedMeshRenderer cmp_smr;
        [Range(0.5f,10)]
        public float changeSpeed;
        [SerializeField]
        private float blendValue=0;
        private float time;
        public float maxValue = 100;
        //References
        //Componentes

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

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        public void UpdateFeature(Controller controller)
        {
            Debug.Log("Test");

            FurryEntity furryEntity = controller as FurryEntity;

            blendValue = furryEntity.furryCount;

            if (cmp_smr == null) return;
            /*if(cmp_smr.GetBlendShapeWeight(0)!=blendValue && time < changeSpeed)
            {                
                float currentWeight = cmp_smr.GetBlendShapeWeight(0);
                currentWeight = Mathf.Lerp(currentWeight, blendValue, time / changeSpeed);
                cmp_smr.SetBlendShapeWeight(0, currentWeight);

                time += Time.deltaTime;
            }*/

            BlendChange(blendValue);
        }

        public void BlendChange(float value)
        {
            float currentWeight = cmp_smr.GetBlendShapeWeight(0);
            currentWeight = Mathf.Lerp(currentWeight, value, changeSpeed);
            cmp_smr.SetBlendShapeWeight(0, currentWeight);
        }

    }
}