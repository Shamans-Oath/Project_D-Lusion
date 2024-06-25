using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Camera_System : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineFreeLook cmp_playerCamera;
    [SerializeField] private float defaultFOV;
    [SerializeField] private List<float> defaultRadius = new List<float>();
    [SerializeField] private Timeline_Manager _timelineManager;

    public CameraShakeScriptable[] shakeBehaviors;
    [SerializeField] private CinemachineImpulseSource cmp_CIS;

    [Header("Lock System")]
    public TargetLock lockSys;
    // Start is called before the first frame update
    void Start()
    {
        if (lockSys)
        {
            lockSys.camSys = this;
            lockSys.cinemachineFreeLook = cmp_playerCamera;
            lockSys.mainCamera = mainCamera;
        }

        defaultFOV = cmp_playerCamera.m_Lens.FieldOfView;

        for(int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            defaultRadius.Add(cmp_playerCamera.m_Orbits[i].m_Radius);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CameraShake("TestExplosion");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SetFOV(80);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(FOVLerp(20, 2.5f));
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ResetFOV();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
           ResetFOVLerp(2.5f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SetRadius(1, 3);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(RadiusLerp(1, 5, 2.5f));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ResetRadius(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetRadiusLerp(1, 2.5f);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            GlobalRadiusChange(3);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GlobalRadiusLerp(5, 2.5f);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GlobalRadiusReset();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GlobalRadiusResetLerp(2.5f);
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
                return;
            }
        }
        Debug.Log("No existe un efecto de camara llamado " + name);
    }

    #region FOVChanges
    public void SetFOV(float FOV)
    {
        cmp_playerCamera.m_Lens.FieldOfView = FOV;
    }
    private  Coroutine fovCorrutine;

    public void FOVLerpInvoke(float targetFOV, float lerpDuration)
    {
        if (fovCorrutine != null) StopCoroutine(fovCorrutine);
        fovCorrutine = StartCoroutine(FOVLerp(targetFOV, lerpDuration));
    }
    public IEnumerator FOVLerp(float targetFOV, float lerpDuration)
    {
        float t = 0;
        float currentFOV = cmp_playerCamera.m_Lens.FieldOfView;

        while (true)
        {
            yield return null;
            cmp_playerCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, t / lerpDuration);

            t += Time.deltaTime;

            if (t > lerpDuration)
            {
                break;
            }
        }
    }

    public void ResetFOV()
    {
        SetFOV(defaultFOV);
    }

    public void ResetFOVLerp(float lerpDuration)
    {
        if (fovCorrutine != null) StopCoroutine(fovCorrutine);
        fovCorrutine=StartCoroutine(FOVLerp(defaultFOV, lerpDuration));
    }
    #endregion

    #region RadiusChange
    public void SetRadius(int orbitIndex , float radius)
    {
        cmp_playerCamera.m_Orbits[orbitIndex].m_Radius = radius;
    }

    public IEnumerator RadiusLerp(int orbitIndex , float targetRadius, float lerpDuration)
    {
        float t = 0;
        float currentRadius = cmp_playerCamera.m_Orbits[orbitIndex].m_Radius;

        while (true)
        {
            yield return null;
            cmp_playerCamera.m_Orbits[orbitIndex].m_Radius = Mathf.Lerp(currentRadius, targetRadius, t / lerpDuration);

            t += Time.deltaTime;

            if (t > lerpDuration)
            {
                break;
            }
        }
    }

    public void ResetRadius(int orbitIndex)
    {
        SetRadius(orbitIndex ,defaultRadius[orbitIndex]);
    }

    public void ResetRadiusLerp(int orbitIndex ,float lerpDuration)
    {
        StartCoroutine(RadiusLerp(orbitIndex ,defaultRadius[orbitIndex], lerpDuration));
    }
    #endregion

    #region GlobalRadiusChanges
    public void GlobalRadiusChange(float targetRadius)
    {
        float currentRadius = cmp_playerCamera.m_Orbits[1].m_Radius;
        float difference = targetRadius - currentRadius;

        for (int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            SetRadius(i, cmp_playerCamera.m_Orbits[i].m_Radius + difference);
        }
    }

    public void GlobalRadiusLerp(float targetRadius, float lerpDuration)
    {
        float currentRadius = cmp_playerCamera.m_Orbits[1].m_Radius;
        float difference = targetRadius - currentRadius;

        for (int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            StartCoroutine(RadiusLerp(i, cmp_playerCamera.m_Orbits[i].m_Radius + difference, lerpDuration));
        }
    }

    public void GlobalRadiusReset()
    {
        for (int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            ResetRadius(i);
        }
    }

    public void GlobalRadiusResetLerp(float lerpDuration)
    {
        for (int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            ResetRadiusLerp(i, lerpDuration);
        }
    }
    #endregion

    public Vector3? GetCameraLookat(float maxDistance)
    {
        if (mainCamera == null) return null;

        if (lockSys.currentTarget != null) return lockSys.currentTarget.transform.position;
        else return null;

        //Descomentar para permitir autotarget a la nada

        /*Vector3 point = mainCamera.transform.position + (mainCamera.transform.forward * maxDistance);
        return point;*/
    }
}
