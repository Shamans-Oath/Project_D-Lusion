using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
[CreateAssetMenu(fileName = "New AttackPreset", menuName = "Combat/01-Attack Preset")]
public class AttackPreset : ScriptableObject
{
    public AnimatorOverrideController animation;
    public float duration;
    public Vector3 possitionOffset;
    public Vector3 hitSize;

    public AttackPreset nextNrmCombo;
    public AttackPreset nextSpcCombo;
}
