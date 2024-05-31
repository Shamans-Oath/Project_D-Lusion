using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Camera_System : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _playerCamera;
    [SerializeField] private Timeline_Manager _timelineManager;

    public CameraShakeScriptable[] shakeBehaviors;
    [SerializeField] private CinemachineImpulseSource cmp_CIS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CameraShake("TestExplosion");
        }
    }

    public void CameraShake(string name)
    {
        foreach (CameraShakeScriptable camShake in shakeBehaviors)
        {
            if (camShake.Name == name)
            {
                cmp_CIS.m_ImpulseDefinition = camShake.impulseDefinition;
                cmp_CIS.m_DefaultVelocity = camShake.defaultVelocity;

                cmp_CIS.GenerateImpulseWithForce(camShake.impulseForce);
                break;
            }
        }
    }
}
