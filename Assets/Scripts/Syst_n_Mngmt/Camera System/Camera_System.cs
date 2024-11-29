using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Camera_System : MonoBehaviour
{
    public static Camera_System instance;

    [SerializeField] public Camera mainCamera;
    
    public CinemachineFreeLook cmp_playerCamera;
    public float defaultFOV;
    [SerializeField] float deadZoneWidth;
    bool midChange;
    [SerializeField] private List<float> defaultRadius = new List<float>();
    [SerializeField] private Timeline_Manager _timelineManager;

    public CameraShakeScriptable[] shakeBehaviors;
    [SerializeField] private CinemachineImpulseSource cmp_CIS;

    [Header("Lock System")]
    public TargetLock lockSys;

    [Header("Sensivility")]
    public float minSens;
    public float maxSens;
    public float sensYRatio = 130;

    [Header("GameData")]
    public GameData gameData;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (lockSys)
        {
            lockSys.camSys = this;
            lockSys.cinemachineFreeLook = cmp_playerCamera;
            lockSys.mainCamera = mainCamera;
        }

        //definir valor de la deadzone
        for (int i = 0; i < 3; i++)
        {
            cmp_playerCamera.GetRig(i).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneWidth = deadZoneWidth;
        }

        defaultFOV = cmp_playerCamera.m_Lens.FieldOfView;

        for(int i = 0; i < cmp_playerCamera.m_Orbits.Length; i++)
        {
            defaultRadius.Add(cmp_playerCamera.m_Orbits[i].m_Radius);
        }

        cmp_playerCamera.m_XAxis.m_MaxSpeed = gameData.sens;
        cmp_playerCamera.m_YAxis.m_MaxSpeed= gameData.sens/sensYRatio;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("TestLerp");
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

    public void FOVLerpVoid(float targetFOV, float lerpDuration)
    {
        Debug.Log("TestLerp");
        midChange = true;

        float currentFOV = cmp_playerCamera.m_Lens.FieldOfView;

        if (midChange == true)
        {
            cmp_playerCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, lerpDuration * Time.deltaTime);

            if (currentFOV == targetFOV)
            {
                midChange = false;
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

        /*Vector3 point = mainCamera.transform.Position + (mainCamera.transform.forward * maxDistance);
        return point;*/
    }

    public float ReadSens()
    {
        float sensRatio = Mathf.InverseLerp(minSens, maxSens, cmp_playerCamera.m_XAxis.m_MaxSpeed);

        return Mathf.Lerp(0 , 1 , sensRatio);
    }

    public void UpdateSens(float value)
    {
        float sensRatio = Mathf.InverseLerp(0, 1, value);

        float newSens = Mathf.Lerp(minSens, maxSens, sensRatio);
        float newSensY = newSens / sensYRatio;


        cmp_playerCamera.m_XAxis.m_MaxSpeed = newSens;
        cmp_playerCamera.m_YAxis.m_MaxSpeed = newSensY;
        gameData.sens = newSens;
        gameData.Save();
    }
}
