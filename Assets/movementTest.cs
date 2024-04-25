using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

   
    public Camera virtualCamera;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraPosition = virtualCamera.transform.position;
        Vector3 cameraForward = (-cameraPosition + transform.position).normalized;

        // Calculate the movement direction based on the camera's forward direction
        Vector3 movement = cameraForward * moveVertical + virtualCamera.transform.right * moveHorizontal;

        Vector3 tmpMov = new Vector3(movement.x, 0,movement.z);
        movement = tmpMov;  
        movement.Normalize();

        // Move the player in the calculated direction
        if (speed != 0 && movement != Vector3.zero)
        {
            transform.position += movement * speed * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.deltaTime);
        }

        // Calculate the forward direction based on the camera's transform
        
    }

    public void Movement(float moveHorizontal, float moveVertical)
    {
        

    }
}
