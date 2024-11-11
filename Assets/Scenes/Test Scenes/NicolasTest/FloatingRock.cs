using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRock : MonoBehaviour
{
    public float amplitude = 0.5f;      
    public float minSpeed = 0.5f;       
    public float maxSpeed = 2.0f;       
    private float speed;                
    private float initialY;            
    private float targetY;              
    private bool goingUp = true;        

    void Start()
    {
        initialY = transform.position.y;
        SetNewTarget();
    }

    void Update()
    {
        float newY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (Mathf.Abs(newY - targetY) < 0.01f)
        {
            goingUp = !goingUp;    
            SetNewTarget();        
        }
    }

    void SetNewTarget()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        targetY = goingUp ? initialY + amplitude : initialY - amplitude;
    }
}


