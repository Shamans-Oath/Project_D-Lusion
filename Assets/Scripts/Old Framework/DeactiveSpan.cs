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
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        timer = 0;
    }
}
