using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Parameters")]
    public float baseHeight;
    public float baseHorizontalSpeed;
    public float gravityIntensifierNearEnd;
    public LayerMask groundLayer;
    private bool _isDashing;
    private bool _isGrounded;
    private Vector3 _speed;
    private bool tagToStop;

    [Header("Cooldowns")]
    public float dashCooldownSeconds;
    public float dashDurationSeconds;
    private float _dashCooldownTimer;
    private float _dashDurationTimer;
    public bool canDash => _dashCooldownTimer <= 0 && !_isDashing && _isGrounded;

    [Header("References")]
    public Collider playerCollider;
    public Camera playerCamera;
    public Rigidbody cmp_rb;
    public PlayerMovement cmp_movement;
    public Gravity cmp_gravity;

    private void Awake()
    {
        if (cmp_rb == false) cmp_rb = GetComponent<Rigidbody>();
        if (cmp_movement == false) cmp_movement = GetComponent<PlayerMovement>();
        if (cmp_gravity == false) cmp_gravity = GetComponent<Gravity>();
    }

    private void Update()
    {
        //Decrease Dash Timers When Cooldown is Active
        if (_dashCooldownTimer > 0) _dashCooldownTimer -= Time.deltaTime;
        if (_dashDurationTimer > 0) _dashDurationTimer -= Time.deltaTime;

        //Allow Movement When landing after dashing
        if (_isDashing && _isGrounded && _dashDurationTimer <= 0)
        {
            _isDashing = false;
            cmp_movement.allowMovement = true;
            tagToStop = true;
        }
    }

    private void FixedUpdate()
    {
        //Save grounded state
        _isGrounded = cmp_gravity.IsGrounded() || InnerGroundCheck();

        if(_isDashing)
        {
            SetDashSpeed();
        }

        if (tagToStop)
        {
            cmp_rb.velocity = Vector3.zero;
            tagToStop = false;
        }
    }

    public void DashForward(Vector2 direction)
    {
        if(!canDash) return;

        Vector3 directionFlatten = CalculateDirection(new Vector3(direction.y, 0f, direction.x));

        _speed = ProjectileMotion.GetSpeed(directionFlatten, baseHeight, baseHorizontalSpeed);

        _isDashing = true;

        //Disable movement while dashing
        cmp_movement.allowMovement = false;

        //Set Cooldown On Use
        _dashCooldownTimer = dashCooldownSeconds;

        _dashDurationTimer = dashDurationSeconds;
    }

    public void DashToPosition(Vector3 position)
    {
        if(!canDash) return;
    
        Vector3 startPosition = transform.position;
        Vector3 direction = position - startPosition;
        direction.y = 0f;

        float newHeight = baseHeight + (position - startPosition).y;

        float flightTime = Mathf.Sqrt(2 * newHeight / Mathf.Abs(Physics.gravity.y)) + Mathf.Sqrt(2 * baseHeight / Mathf.Abs(Physics.gravity.y + gravityIntensifierNearEnd));

        float length = direction.magnitude; 

        float newHorizontalSpeed = length / flightTime;

        _speed = ProjectileMotion.GetSpeed(direction.normalized, newHeight, newHorizontalSpeed);

        _isDashing = true;

        //Disable movement while dashing
        cmp_movement.allowMovement = false;

        //Set Cooldown On Use
        _dashCooldownTimer = Mathf.Max(dashCooldownSeconds, flightTime);

        _dashDurationTimer = flightTime;


    }

    private void SetDashSpeed()
    {
        float timeDelta = Time.fixedDeltaTime;
        float gravity = Physics.gravity.y;

        if(_speed.y < 0)
        {
            gravity += gravityIntensifierNearEnd;
        }

        _speed += Vector3.up * gravity * timeDelta;

        cmp_rb.velocity = _speed;
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

    private bool InnerGroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerCollider.bounds.extents.y + .025f, groundLayer);
    }
}
