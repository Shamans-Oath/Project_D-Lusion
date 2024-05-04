using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Parameters")]
    public float baseHeight;
    public float gravityMultiplierNearEnd;
    private bool _isDashing;
    public bool isDashing { get { return _isDashing; } }
    private Vector3 _speed;
    private bool _tagToStop;

    [Header("Time Management")]
    public float dashCooldownSeconds;
    private float _dashCooldownTimer;
    private float _dashDurationTimer;

    [Header("References")]
    public Rigidbody cmp_rb;
    public PlayerMovement cmp_movement;

    private void Awake()
    {
        if (cmp_rb == false) cmp_rb = GetComponent<Rigidbody>();
        if (cmp_movement == false) cmp_movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //Decrease Dash Timers When Cooldown is Active
        WaitQueue(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(_isDashing)
        {
            SetDashSpeed();
        }

        if (_tagToStop)
        {
            cmp_rb.velocity = Vector3.zero;
            _tagToStop = false;
        }
    }

    public void DashToPosition(Vector3 position)
    {
        //Check inner state: if the cooldown is not ready or the player is already dashing interrupt
        if (!DashCooldownReady() || _isDashing) return;

        //Start Calculations
        Vector3 startPosition = transform.position;
        Vector3 direction = position - startPosition;
        direction.y = 0f;

        float height = baseHeight + (position - startPosition).y;

        float flightTime = ProjectileMotion.GetFlightTime(height) + ProjectileMotion.GetFlightTime(height, gravityMultiplierNearEnd);

        float length = direction.magnitude; 

        float newHorizontalSpeed = length / flightTime;

        //Get the speed to reach the target position
        _speed = ProjectileMotion.GetStartSpeed(direction.normalized, height, newHorizontalSpeed);

        //Disable movement while dashing
        DashState(true);

        //Set Cooldown On Use
        SetTimers(flightTime);
    }

    private void SetDashSpeed()
    {
        float timeDelta = Time.fixedDeltaTime;
        float gravity = Physics.gravity.y;

        if(_speed.y < 0)
        {
            gravity *= gravityMultiplierNearEnd;
        }

        _speed += Vector3.up * gravity * timeDelta;

        //Set the speed to the rigidbody
        cmp_rb.velocity = _speed;
    }

    #region Time Management
    private void WaitQueue(float timeDelta)
    {
        if (_dashCooldownTimer > 0) _dashCooldownTimer -= timeDelta;
        if (_dashDurationTimer > 0) _dashDurationTimer -= timeDelta;
    }

    private void SetTimers(float dashDuration)
    {
        _dashCooldownTimer = dashCooldownSeconds + dashDuration;

        _dashDurationTimer = dashDuration;
    }

    public bool DashCooldownReady()
    {
        return _dashCooldownTimer <= 0;
    }

    #endregion

    #region State Management

    public void CheckLanding(bool isGrounded)
    {
        if (!isGrounded || !_isDashing || _dashDurationTimer > 0) return;
        
        Landing();
    }

    private void Landing()
    {
        //Allow Movement When landing after dashing
        DashState(false);

        //Stop the player from moving when landing, to avoid sliding, delegating to fixed update because of physics
        _tagToStop = true;
    }

    private void DashState(bool state)
    {
        _isDashing = state;

        //Disable movement while dashing
        cmp_movement.allowMovement = !state;
    }

    #endregion
}
