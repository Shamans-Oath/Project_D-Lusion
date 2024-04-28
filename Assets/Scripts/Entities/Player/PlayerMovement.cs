using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public bool allowMovement = true;
    public float rotationSpeed;
    public float speed;


    public Camera playerCamera;

    private void Awake()
    {
        if (playerCamera == false && GameObject.Find("Main Camera")) playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void Movement(float moveHorizontal, float moveVertical)
    {
        if (allowMovement == false) return;
        Vector3 movement = CalculateDirection(new Vector3(moveHorizontal, 0, moveVertical));
        transform.position += movement * speed * Time.deltaTime;
            
    }

    public void Rotate(float roteHorizontal, float roteVertical)
    {
        Vector3 direction = CalculateDirection(new Vector3(roteHorizontal, 0, roteVertical));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }

    Vector3 CalculateDirection(Vector3 vector)
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 cameraForward = (-cameraPosition + transform.position).normalized;

        // Calculate the movement direction based on the camera's forward direction
        Vector3 direction = cameraForward * vector.x + playerCamera.transform.right * vector.z;

        Vector3 tmpMov = new Vector3(direction.x, 0, direction.z);
        direction = tmpMov;
        direction.Normalize();

        return direction;
    }

    public void ToggleMovement()
    {
        if(allowMovement)
        {
            allowMovement = false;
        }
        else
        {
            allowMovement = true;
        }
    }
}
