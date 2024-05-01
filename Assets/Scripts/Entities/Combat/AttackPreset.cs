using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum AttackAtachables
{
    Default,
    RightHand,
    LeftHand,
    RightFood,
    LeftFood,
    Weapon1,
    Weapon2
    

}

[CreateAssetMenu(fileName = "New AttackPreset", menuName = "Combat/01-Attack Preset")]
public class AttackPreset : ScriptableObject
{
    public AnimatorOverrideController animation;
    public float duration;
    public int damage;
    public Vector3 possitionOffset;
    public Vector3 hitSize;

    public AttackPreset nextNrmCombo;
    public AttackPreset nextSpcCombo;

    [Header("Extra Effects")]
    public int healOnHit=0;
}
