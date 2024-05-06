using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float maxSpeed;
    public float acceleration;

    [Header("Control")]
    [SerializeField] private bool _allowMovement = true;
    public bool AllowMovement { get { return _allowMovement; } }

    [Header("References")]
    public Camera playerCamera;
    public Rigidbody cmp_rb;
    public Slope cmp_slope;
    public LadderStep cmp_ladder;

    private void Awake()
    {
        if (playerCamera == false && GameObject.Find("Main Camera")) playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if(cmp_rb == false && gameObject.GetComponent<Rigidbody>()) cmp_rb = gameObject.GetComponent<Rigidbody>();
        if (cmp_slope == false && gameObject.GetComponent<Slope>()) cmp_slope = gameObject.GetComponent<Slope>();
        if (cmp_ladder == false && gameObject.GetComponent<LadderStep>()) cmp_ladder = gameObject.GetComponent<LadderStep>();
    }

    public void Movement(float moveHorizontal, float moveVertical)
    {
        if (_allowMovement == false) return;

        Vector3 movement = CalculateDirection(new Vector3(moveHorizontal, 0, moveVertical));

        if (cmp_slope.onSlope) movement = cmp_slope.OnSlopePlane(movement);

        if (cmp_ladder.onStep) movement = cmp_ladder.OnStepPlane(movement);

        if (movement != Vector3.zero) cmp_rb.AddForce(movement.normalized * acceleration * 10f);
    }

    public void LimitSpeed()
    {
        if (cmp_slope.onSlope || cmp_ladder.onStep)
        {
            if(cmp_rb.velocity.magnitude > maxSpeed) cmp_rb.velocity = cmp_rb.velocity.normalized * maxSpeed;
            return;
        }

        Vector3 flattenVelocity = new Vector3(cmp_rb.velocity.x, 0, cmp_rb.velocity.z);

        if(flattenVelocity.magnitude > maxSpeed)
        {
            cmp_rb.velocity = flattenVelocity.normalized * maxSpeed + Vector3.up * cmp_rb.velocity.y;
        }
    }

    public Vector3 CalculateDirection(Vector3 vector) //S hice algunos cambios en favor de la legibilidad
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 cameraForward = (transform.position - cameraPosition).normalized;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);

        // Calculate the movement direction based on the camera's forward direction
        Vector3 direction = cameraForward * vector.x + cameraRight * vector.z;

        direction.y = 0;
        direction.Normalize();

        return direction;
    }

    public void ToggleMovement(bool allowMovement)
    {
        _allowMovement = allowMovement;
    }
}
