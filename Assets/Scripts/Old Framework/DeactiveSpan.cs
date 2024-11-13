using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveSpan : MonoBehaviour
{
    public float timeSpan;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer < Mathf.Abs(timeSpan))
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
}
