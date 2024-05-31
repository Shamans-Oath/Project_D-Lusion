using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "CamShakeSettings", menuName = "Camera/CameraShake")]
public class CameraShakeScriptable : ScriptableObject
{
    public string Name;

    public CinemachineImpulseDefinition impulseDefinition;

    public Vector3 defaultVelocity = new Vector3(0, -1f, 0);
    public float impulseForce = 1f;
}