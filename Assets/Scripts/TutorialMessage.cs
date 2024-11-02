using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float activationDuration = 5f;
    [SerializeField] private float fadeOutDuration = 2f; 

    private float timer = 0f;
    public bool isActive = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.gameObject.tag=="Player")
        {
            objectToActivate.SetActive(true);
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isActive)
        {
            FadeOut();
        }
    }
    private void Update()
    {

    }
    public void FadeOut()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float fadeTimer = 0f;

        while (fadeTimer < fadeOutDuration)
        {
            float t = fadeTimer / fadeOutDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            fadeTimer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        objectToActivate.SetActive(false);
    }
}

