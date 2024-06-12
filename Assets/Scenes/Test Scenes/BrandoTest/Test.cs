using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Features
{
    public class Test :  MonoBehaviour, IActivable, IFeatureSetup //Other channels
    {
        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        //Properties
        // Raycasting settings
        public float raycastDistance = 1.0f;
        public LayerMask groundLayer;

    // Foot placement settings
        public float strideDistance = 0.1f;
        // The speed of the stride
        public float strideSpeed = 1.0f;

    // The animation curve for the stride
        public AnimationCurve strideCurve;
        //The previous positions of the feet
        private Vector3[] leftFootPreviousPositions;
        private Vector3[] rightFootPreviousPositions;       
        public float footRotationSpeed = 5.0f;

    // Animation rigging settings
        //public Rig rig;

        public GameObject[] oddFoot;
        public GameObject[] evenFoot;

         public GameObject[] oddFootEffectors;
        public GameObject[] evenFootEffectors;

        // The current stride progress
        private float[] oddStrideProgress;
        private float[] evenStrideProgress;

        //References
        //Componentes

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            // Create the animation curve
        strideCurve = new AnimationCurve();
        strideCurve.keys = new Keyframe[] 
            {
                new Keyframe(0, 0),
                new Keyframe(0.5f, 0.5f),
                new Keyframe(1, 1)
            };
            // Initialize the previous positions arrays
        leftFootPreviousPositions = new Vector3[oddFoot.Length];
        rightFootPreviousPositions = new Vector3[evenFoot.Length];

        // Initialize the stride progress arrays
        oddStrideProgress = new float[oddFoot.Length];
        evenStrideProgress = new float[evenFoot.Length];

        // Set the initial previous positions to the current positions
        for (int i = 0; i < oddFoot.Length; i++)
        {
            leftFootPreviousPositions[i] = oddFoot[i].transform.position;
        }
        for (int i = 0; i < evenFoot.Length; i++)
        {
            rightFootPreviousPositions[i] = evenFoot[i].transform.position;
        }
            //ToggleActive(true);
        }

        public void FixedUpdateFeature()
        {
            // Raycast down from each foot
        // Loop through each foot and effector
        for (int i = 0; i < oddFoot.Length; i++)
        {
            CheckStride(oddFoot[i], oddFootEffectors[i].transform, ref leftFootPreviousPositions[i], ref oddStrideProgress[i]);
        }
        for (int i = 0; i < evenFoot.Length; i++)
        {
            CheckStride(evenFoot[i], evenFootEffectors[i].transform, ref rightFootPreviousPositions[i], ref evenStrideProgress[i]);
        }

        // Update the Rig
        //rig.Evaluate();
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;

            if (active) return;
            
        }

        // Check if the foot has moved a certain distance away from its previous position
    // Check if the foot has moved a certain distance away from its previous position
    void CheckStride(GameObject foot, Transform target, ref Vector3 previousPosition, ref float strideProgress)
    {
        // Calculate the distance between the current and previous positions
        float distance = Vector3.Distance(foot.transform.position, previousPosition);

        // If the distance is greater than the stride distance, move the foot
        if (distance > strideDistance)
        {
            // Raycast down from the foot
            RaycastHit hit;
            if (Physics.Raycast(foot.transform.position, Vector3.down, out hit, 0.1f, groundLayer))
            {
                // Calculate the stride progress
                strideProgress = Mathf.Clamp01(strideProgress + Time.deltaTime * strideSpeed);

                // Evaluate the animation curve
                float curveValue = strideCurve.Evaluate(strideProgress);

                // Calculate the target position
                Vector3 targetPosition = Vector3.Lerp(previousPosition, hit.point, curveValue);

                // Set the target position
                target.position = targetPosition;

                // Update the previous position
                previousPosition = foot.transform.position;
            }
        }
        else
        {
            // If the foot hasn't moved far enough, reset the stride progress
            strideProgress = 0.0f;
        }
    }
    }
}