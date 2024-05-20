using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Features
{
    [CreateAssetMenu(fileName = "New Attack Preset", menuName = "Combat System/Attack Preset")]
    public class AttackPreset : ScriptableObject
    {
        public AnimationClip animationClip;
        public AttackSwing[] swings;
        public AnimatorOverrideController overrideController;


        public void BuildAnimationEvents()
        {
            AnimationEvent[] clipEvents = new AnimationEvent[swings.Length * 2];

            for (int i = 0; i < swings.Length; i++)
            {
                AnimationEvent startEvent = new AnimationEvent();
                startEvent.functionName = "StartAttack";
                startEvent.time = Mathf.Clamp(swings[i].start, 0, animationClip.length);
                startEvent.intParameter = i;

                AnimationEvent endEvent = new AnimationEvent();
                endEvent.functionName = "EndAttack";
                endEvent.time = Mathf.Clamp(swings[i].end, 0, animationClip.length);
                endEvent.intParameter = i;

                clipEvents[i * 2] = startEvent;
                clipEvents[i * 2 + 1] = endEvent;
            }

            AnimationUtility.SetAnimationEvents(animationClip, clipEvents);
        }
    }
}
