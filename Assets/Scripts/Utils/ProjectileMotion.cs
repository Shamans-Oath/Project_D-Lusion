using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public static Vector3 GetStartSpeed(Vector3 direction, float height, float horizontalSpeed, float gravityMultiplier)
    {
        float gravity = Mathf.Abs(Physics.gravity.y) * gravityMultiplier;

        float verticalComponent = Mathf.Sqrt(2 * gravity * height);

        Vector3 speed = direction.normalized * horizontalSpeed + Vector3.up * verticalComponent;
    
        return speed;
    }

    public static float GetFlightTime(float height, float gravityMultiplier)
    {
        float gravity = Mathf.Abs(Physics.gravity.y) * gravityMultiplier;

        float time = Mathf.Sqrt(2 * height / gravity);

        return time;
    }

    public static float GetFlightTime(float height)
    {
        return GetFlightTime(height, 1f);
    }
}
