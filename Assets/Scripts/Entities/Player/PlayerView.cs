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
    // Start is called before the first frame update
    void Start()
    {
        _cmp_controller = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButton(0))
        {
            StartCoroutine(emissionChange(0, 0));
        }

        if (Input.GetMouseButton(1))
        {
            StartCoroutine(emissionChange(0, 1));
        }*/
    }

    public void AnimatorUpdater()
    {
        cmp_anim.SetFloat("Blend",_cmp_controller.currentFurymeter);
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
                tatooMaterials[materialIndex].material.SetColor("_EmissionColor", Color.green * emissionIntensity);

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
                tatooMaterials[materialIndex].material.SetColor("_EmissionColor", Color.green * emissionIntensity);

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
}
