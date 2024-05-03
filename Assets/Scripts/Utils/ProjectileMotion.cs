using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public static Vector3 GetSpeed(Vector3 direction, float height, float horizontalSpeed)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        float verticalComponent = Mathf.Sqrt(2 * gravity * height);

        Vector3 speed = direction.normalized * horizontalSpeed + Vector3.up * verticalComponent;
    
        return speed;
    }
}
