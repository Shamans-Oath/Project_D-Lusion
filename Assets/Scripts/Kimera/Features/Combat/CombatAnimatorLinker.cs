using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features
{
    public class CombatAnimatorLinker :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate //Other channels
    {
        public enum BodyParts
        {
            LeftHand,
            RightHand,
            LeftElbow,
            RightElbow,
            LeftKnee,
            RightKnee,
            LeftFoot,
            RightFoot,
            Chest
        }

        [System.Serializable]
        public struct AnimationParentLink
        {
            public string guid;
            public BodyParts bodyPart;
            public Transform child;
        }

        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private IDictionary<string, AnimationParentLink> links;
        [SerializeField] private List<AnimationParentLink> readLinks;
        //Properties
        //References
        //Componentes
        [Header("Components")]
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftElbow;
        [SerializeField] private Transform rightElbow;
        [SerializeField] private Transform leftKnee;
        [SerializeField] private Transform rightKnee;
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform chest;

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            links = new Dictionary<string, AnimationParentLink>();

            ToggleActive(true);
        }

        public bool GetActive()
        {
            return active;
        }



        public void UpdateLink(AnimationParentLink link)
        {
            Transform bodyPart = GetAnimationBodyPart(link.bodyPart);

            link.child.position = bodyPart.position;
            link.child.rotation = bodyPart.rotation;
        }

        public string CreateLink(Transform target, BodyParts bodyPart)
        {
            AnimationParentLink link = new AnimationParentLink()
            {
                guid = System.Guid.NewGuid().ToString(),
                bodyPart = bodyPart,
                child = target
            };

            links.Add(link.guid, link);

            return link.guid;
        }

        public void DestroyLink(string guid)
        {
            if (!links.ContainsKey(guid)) return;
        
            links.Remove(guid);
        }

        private Transform GetAnimationBodyPart(BodyParts bodyPart)
        {
            switch(bodyPart)
            {
                case BodyParts.LeftHand:
                    return leftHand;

                case BodyParts.RightHand:
                    return rightHand;

                case BodyParts.LeftElbow:
                    return leftElbow;

                case BodyParts.RightElbow:
                    return rightElbow;

                case BodyParts.LeftKnee:
                    return leftKnee;

                case BodyParts.RightKnee:
                    return rightKnee;

                case BodyParts.LeftFoot:
                    return leftFoot;

                case BodyParts.RightFoot:
                    return rightFoot;

                case BodyParts.Chest:
                    return chest;
                
                default:
                    return null;
            }
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        void IFeatureUpdate.UpdateFeature(Controller controller)
        {
            if (!active) return;

            readLinks = links.Values.ToList();

            links.Values.ToList().ForEach(link => UpdateLink(link));
        }
    }
}