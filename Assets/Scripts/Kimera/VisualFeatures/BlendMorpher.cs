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
        public BlendGroup[] activeObjects;
        public float gapActivationValue;
        public SkinMeshBlend[] meshRenderers;
        [Range(0.5f,10)]
        public float changeSpeed;
        [SerializeField]
        private float blendValue=0;
        private float time;
        public float maxValue = 100;
        public float blendUnit;
        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;
            
            //Setup Properties

            ToggleActive(true);
            FurryEntity furryEntity = controller as FurryEntity;
            blendUnit = maxValue / settings.Search("furryMax");

            GroupEnabler(furryEntity.furryCount * blendUnit);
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
            //Debug.Log("Test");

            FurryEntity furryEntity = controller as FurryEntity;

            if (blendValue != furryEntity.furryCount) GroupEnabler(furryEntity.furryCount * blendUnit);

            blendValue = furryEntity.furryCount * blendUnit;

            if (meshRenderers.Length>0)
            {
                foreach (SkinMeshBlend mesh in meshRenderers)
                {
                    BlendChange(blendValue, mesh);
                }
            }
            /*if(cmp_smr.GetBlendShapeWeight(0)!=blendValue && time < changeSpeed)
            {                
                float currentWeight = cmp_smr.GetBlendShapeWeight(0);
                currentWeight = Mathf.Lerp(currentWeight, blendValue, time / changeSpeed);
                cmp_smr.SetBlendShapeWeight(0, currentWeight);

                time += Time.deltaTime;
            }*/
 
  
        }

        public void BlendChange(float value, SkinMeshBlend blend)
        {
            if (blend.skinMesh == null) return;

            float currentWeight = blend.skinMesh.GetBlendShapeWeight(0);
            if (currentWeight == value) return;

            if (blend.inverse == true) value = maxValue - value;
            currentWeight = Mathf.Lerp(currentWeight, value, changeSpeed);
            blend.skinMesh.SetBlendShapeWeight(0, currentWeight);
        }
        public void GroupEnabler(float value)
        {
            float baseValue = maxValue / activeObjects.Length;
            for (int i = 0; i < activeObjects.Length; i++)
            {
                bool toSet = false;
                float tmpVal = value;
                if (i == activeObjects.Length - 1) tmpVal--;
                else if(i==0) tmpVal++;
                if (tmpVal + gapActivationValue >= baseValue * i && tmpVal - gapActivationValue < baseValue * (i+1)) toSet = true;

                for (int j = 0; j < activeObjects[i].setOnElements.Length; j++)
                {
                    activeObjects[i].setOnElements[j].SetActive(toSet);
                }
            }
        }
    }

    [System.Serializable]
    public class BlendGroup
    {
        [Header("To Active Objects")]
        public GameObject[] setOnElements;
    }
    [System.Serializable]
    public class SkinMeshBlend
    {
        public bool inverse;
        public SkinnedMeshRenderer skinMesh;
    }

}