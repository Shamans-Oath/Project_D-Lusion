using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaizAttack : MonoBehaviour
{
    public GameObject bomb;
    public float duration = 1f;          
    public AnimationCurve scaleCurve;   
    public bool scaleOnStart = true;
    public bool destroyOnCompletion = false;
    public float delay;
    private Vector3 initialScale;
    private bool isScaling = false;


    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        StartCoroutine(SpawnSequence());
    }

    void Update()
    {
        
    }
    public void StartScaling()
    {
        if (!isScaling)
        {
            StartCoroutine(ScaleCoroutine());
        }
    }
    private System.Collections.IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(delay);
        if (scaleOnStart)
        {
            StartScaling();
        }
        yield return new WaitForSeconds(2f+duration);
        Instantiate(bomb, transform.position,transform.rotation);
        Destroy(gameObject);
    }
    private System.Collections.IEnumerator ScaleCoroutine()
    {
        isScaling = true;
        transform.localScale = Vector3.zero; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scaleFactor = scaleCurve != null ? scaleCurve.Evaluate(t) : t;


            transform.localScale = initialScale * scaleFactor;


            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = initialScale;
        isScaling = false;
        if (destroyOnCompletion)
        {
            gameObject.SetActive(false);
            Destroy(gameObject,10f);
        }
    }
}
