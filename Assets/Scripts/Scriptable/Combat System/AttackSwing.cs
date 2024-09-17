using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    [System.Serializable]
    public class AttackSwing
    {
        public string swingName;

        [Header("Hitbox Settings")]
        public Vector3 size;
        public Vector3 offset;
        public Vector3 movement;
        public CombatAnimatorLinker.BodyParts bodyPart;

        [Header("Effects")]
        public Settings settings;

        [Header("Timing")]
        [HideInInspector] public float duration;
        public float start;
        public float end;
    }
}
