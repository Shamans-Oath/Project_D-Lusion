using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [Header("Parameters")]
    public float groundDrag;
    public float airDrag;

    [Header("References")]
    public PlayerMovement cmp_move;
    public Rigidbody cmp_rb;
    public Gravity cmp_gravity;
    public PlayerDash cmp_dash;
    public Slope cmp_slope;
    public LadderStep cmp_ladder;

    private void Awake()
    {
        if (cmp_move == false && gameObject.GetComponent<PlayerMovement>()) cmp_move = gameObject.GetComponent<PlayerMovement>();
        if (cmp_rb == false && gameObject.GetComponent<Rigidbody>()) cmp_rb = gameObject.GetComponent<Rigidbody>();
        if (cmp_gravity == false && gameObject.GetComponent<Gravity>()) cmp_gravity = gameObject.GetComponent<Gravity>();
        if (cmp_dash == false && gameObject.GetComponent<PlayerDash>()) cmp_dash = gameObject.GetComponent<PlayerDash>();
        if (cmp_slope == false && gameObject.GetComponent<Slope>()) cmp_slope = gameObject.GetComponent<Slope>();
        if (cmp_ladder == false && gameObject.GetComponent<LadderStep>()) cmp_ladder = gameObject.GetComponent<LadderStep>();
    }

    public void ApplyDrag(Vector2 direction)
    {
        bool grounded = cmp_gravity.isGrounded;
        Vector3 velocity = cmp_rb.velocity;

        Vector3 flattenDirection = cmp_move.CalculateDirection(new Vector3(direction.x, 0, direction.y));
        Vector3 flattenVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (cmp_slope.onSlope)
        {
            flattenDirection = cmp_slope.OnSlopePlane(flattenDirection);
            flattenVelocity = velocity;
        }

        if(cmp_ladder.onStep)
        {
            flattenDirection = cmp_ladder.OnStepPlane(flattenDirection.normalized);
            flattenVelocity = velocity;
        }

        bool changeDir = changeDirection(flattenDirection, flattenVelocity);

        float drag = 0f;

        if (grounded && !cmp_ladder)
        {
            drag = airDrag;
        } else if(changeDir || direction == Vector2.zero)
        {
            drag = groundDrag;
        }

        if(drag != 0) cmp_rb.AddForce(-flattenVelocity * drag);

        bool changeDirection(Vector3 direction, Vector3 velocity)
        {
            if(direction == Vector3.zero) return false;

            return Vector3.Dot(direction, velocity) < 0;
        }
    }
}
