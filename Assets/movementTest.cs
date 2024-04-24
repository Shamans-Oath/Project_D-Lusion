using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class movementTest : MonoBehaviour
{
   public float speed = 5f;
    public Camera virtualCamera;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the forward direction based on the camera's transform
        Vector3 cameraPosition = virtualCamera.transform.position;
        Vector3 cameraForward = (-cameraPosition + transform.position).normalized;

        // Calculate the movement direction based on the camera's forward direction
        Vector3 movement = cameraForward * moveVertical + virtualCamera.transform.right * moveHorizontal;
        movement.Normalize();

        // Move the player in the calculated direction
        transform.position += movement * speed * Time.deltaTime;
    }
}
