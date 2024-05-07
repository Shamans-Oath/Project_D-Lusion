using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//[RequireComponent(typeof(PlayerController))]
public class PlayerView : MonoBehaviour
{
    private PlayerController _cmp_controller;
    public Animator cmp_anim;

    public Renderer[] tatooMaterials;
    float emissionIntensity;
    public float lerpDuration;

    public SkinnedMeshRenderer cmp_smr;
    public float blendLerpDuration;

    public RagdolSetter cmp_ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        //cmp_smr = GetComponent<SkinnedMeshRenderer>();
        _cmp_controller = gameObject.GetComponent<PlayerController>();
        _cmp_controller.cmp_life.HealthGain += UpdateHealthOnHeal;
        _cmp_controller.cmp_life.HealthLose += UpdateHealthOnLoss;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _cmp_controller.cmp_life.LooseHealth(10);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _cmp_controller.cmp_life.GainHealth(10);
        }

        if (Input.GetMouseButtonDown(2))
        {
            UpdateBlendShape();
        }  

        if(Input.GetMouseButtonDown(3))
        {
            ActivateRagdoll();
        }

        if (Input.GetMouseButtonDown(4))
        {
            DeactivateRagdoll();
        }

        /*if(_cmp_controller.cmp_move.cmp_rb.velocity.y > 0)
        {
            AnimatorSetFloat("UpSpd", _cmp_controller.cmp_move.cmp_rb.velocity.y);
        }*/


        cmp_anim.SetBool("Ground", _cmp_controller.cmp_gravity.isGrounded);
        AnimatorSetFloat("Blend", _cmp_controller.currentFurymeter);
        AnimatorSetFloat("Speed", _cmp_controller.direction.x + _cmp_controller.direction.y);
    }

    public void ActivateRagdoll()
    {
        cmp_ragdoll.RagdollSetActive(true);
    }


    public void DeactivateRagdoll()
    {
        cmp_ragdoll.RagdollSetActive(false);
    }

    public void AnimatorSetBoolTrue(string name)
    {
        cmp_anim.SetBool(name, true);
    }
    public void AnimatorSetBoolFalse(string name)
    {
        cmp_anim.SetBool(name, false);
    }

    public void AnimatorSetFloat(string parameter, float value)
    {
        cmp_anim.SetFloat(parameter, value);
    }

    public void PlayAnimation(string name)
    {
        cmp_anim.Play(name);
    }

    /*public void AnimatorUpdater()
    {
        cmp_anim.SetFloat("Blend",_cmp_controller.currentFurymeter);
    }*/

    //Estas funciones todavia no estan listas, usar solo la corutina de emissionChange
    public void UpdateHealthOnHeal()
    {
        float percentageDivision = 1 / tatooMaterials.Length;

        for (int i = 0; i < tatooMaterials.Length; i++)
        {
            if (_cmp_controller.cmp_life.currentHealth >= _cmp_controller.cmp_life.maxHealth * (percentageDivision * (i + 1)))
            {
                StartCoroutine(emissionChange(i, 1));

            }
            else
            {
                break;
            }
        }
    }

    public void UpdateHealthOnLoss()
    {
        float percentageDivision = 1 / tatooMaterials.Length;

        for (int i = tatooMaterials.Length - 1; i >= 0; i--)
        {
            if (_cmp_controller.cmp_life.currentHealth <= _cmp_controller.cmp_life.maxHealth * (percentageDivision * i))
            {
                StartCoroutine(emissionChange(i, 0));
            }
            else
            {
                break;
            }
            
        }
    }

    public IEnumerator emissionChange(int materialIndex, int changeType) 
    {
        float t = 0;

        if (changeType == 0) 
        {
            while (true)
            {
                yield return null;
                emissionIntensity = Mathf.Lerp(5, -10, t / lerpDuration);

                float adjustedIntensity = emissionIntensity - (0.4169f);
                
                tatooMaterials[materialIndex].material.SetColor("_EmissionColor", new Color(0, 0.75f, 0.01f) * Mathf.Pow(2, adjustedIntensity));

                t += Time.deltaTime;

                if (t > lerpDuration)
                {
                    break;
                }
            }
        }
        else if (changeType == 1) 
        {
            while (true)
            {
                yield return null;
                emissionIntensity = Mathf.Lerp(-10, 5, t / lerpDuration);

                float adjustedIntensity = emissionIntensity - (0.4169f);
                
                tatooMaterials[materialIndex].material.SetColor("_EmissionColor", new Color(0, 0.75f, 0.01f) * Mathf.Pow(2, adjustedIntensity));

                t += Time.deltaTime;

                if (t > lerpDuration)
                {
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Esta corrutina solo acepta valores de 0 y 1, 0 para apagar el material y 1 para encederlo");
            StopCoroutine("emissionChange");
        }
    }

    public void UpdateBlendShape()
    {
        if(_cmp_controller.currentFurymeter == 0 || _cmp_controller.currentFurymeter == 0.25f || _cmp_controller.currentFurymeter == 0.5f || _cmp_controller.currentFurymeter == 0.75f || _cmp_controller.currentFurymeter == 1)
        {
            StopCoroutine("blendShapeLerp");
            StartCoroutine(blendShapeLerp(_cmp_controller.currentFurymeter * 100));
        }
    }

    public IEnumerator blendShapeLerp(float targetValue)
    {
        float t = 0;

        float currentWeight = cmp_smr.GetBlendShapeWeight(0);
                
        while (true)
        {
            yield return null;
            currentWeight = Mathf.Lerp(currentWeight, targetValue, t / lerpDuration);
            cmp_smr.SetBlendShapeWeight(0, currentWeight);

            t += Time.deltaTime;

            if (t > lerpDuration)
            {
                break;
            }
        }        
    }

    void OnDisable()
    {
        _cmp_controller.cmp_life.HealthGain -= UpdateHealthOnHeal;
        _cmp_controller.cmp_life.HealthLose -= UpdateHealthOnLoss;
    }
}
