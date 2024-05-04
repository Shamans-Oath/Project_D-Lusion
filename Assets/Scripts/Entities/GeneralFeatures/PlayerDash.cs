using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Parameters")]
    public float timeMultiplier;
    public float baseHeight;
    public float gravityMultiplierUpHill;
    public float gravityMultiplierDownHill;
    private bool _isDashing;
    private bool _isCharging;
    public bool isDashing { get { return _isDashing; } }
    public bool isCharging { get { return _isCharging; } }
    private Vector3 _speed;
    private bool _tagToStop;
    private Vector3 _speedAfterStop;
    private Vector3 _targetPosition;

    [Header("Time Management")]
    public float chargeTimeSeconds;
    private Coroutine _dashCharge;
    public float dashCooldownSeconds;
    private float _dashCooldownTimer;
    private float _dashDurationTimer;
    public float timeToLand => _isDashing ? Mathf.Max(_dashDurationTimer / timeMultiplier, 0) : -1;

    [Header("References")]
    public Collider cmp_collider;
    public Rigidbody cmp_rb;
    public PlayerMovement cmp_movement;

    [Header("Debug")]
    public bool debug;

    private void Awake()
    {
        if (cmp_rb == false) cmp_rb = GetComponent<Rigidbody>();
        if (cmp_movement == false) cmp_movement = GetComponent<PlayerMovement>();
        if (cmp_collider == false) cmp_collider = GetComponent<Collider>();
    }

    private void Update()
    {
        //Decrease Dash Timers When Cooldown is Active
        WaitQueue(Time.deltaTime * timeMultiplier);
    }

    private void FixedUpdate()
    {
        if(_isDashing && !_isCharging)
        {
            SetDashSpeed();
        }

        if (_tagToStop)
        {
            cmp_rb.velocity = _speedAfterStop;
            _tagToStop = false;
            _speedAfterStop = Vector3.zero;
        }
    }

    public void ChargeDash(Vector3 position)
    {
        //Check inner state: if the cooldown is not ready or the player is already dashing interrupt
        if (!DashCooldownReady() || _isDashing || _isCharging) return;

        _dashCharge = StartCoroutine(Dash(position));
    }

    private IEnumerator Dash(Vector3 position)
    {
        _targetPosition = position;

        _isCharging = true;

        //Start Calculations
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = position + Vector3.up * cmp_collider.bounds.extents.y;
        Vector3 direction = targetPosition - startPosition;
        direction.y = 0f;

        float height = baseHeight + (targetPosition - startPosition).y;

        float flightTime = ProjectileMotion.GetFlightTime(height, gravityMultiplierUpHill) + ProjectileMotion.GetFlightTime(baseHeight, gravityMultiplierDownHill);

        float length = direction.magnitude;

        float newHorizontalSpeed = length / flightTime;

        //Get the speed to reach the target position
        _speed = ProjectileMotion.GetStartSpeed(direction.normalized, height, newHorizontalSpeed, gravityMultiplierUpHill);

        //Disable movement while dashing and charging
        DashState(true);

        //Set Cooldown On Charge
        SetTimers(flightTime + chargeTimeSeconds * timeMultiplier);

        //Wait for the charge time
        yield return new WaitForSeconds(chargeTimeSeconds);

        _isCharging = false;
    }

    private void SetDashSpeed()
    {
        float timeDelta = Time.fixedDeltaTime * timeMultiplier;
        float gravity = Physics.gravity.y;

        gravity *= _speed.y < 0 ? gravityMultiplierDownHill : gravityMultiplierUpHill;

        _speed += Vector3.up * gravity * timeDelta;

        //Set the speed to the rigidbody
        cmp_rb.velocity = _speed * timeMultiplier;
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
        _speedAfterStop = Vector3.zero;

        //Process Landing
        ProcessLanding(_targetPosition);
        _targetPosition = Vector3.zero;
    }

    private void DashState(bool state)
    {
        _isDashing = state;

        //Disable movement while dashing
        cmp_movement.allowMovement = !state;
    }

    public void InterruptDash()
    {
        InterruptDash(Vector3.zero);
    }

    public void InterruptDash(Vector3 speedAfterInterruption)
    {
        if (!_isDashing) return;

        //If is still charging, interrupt the charge
        if (_isCharging) StopCoroutine(_dashCharge);

        //Allow movement when interrupted
        DashState(false);

        //Stop the player one frame when interrupted, to avoid sliding, delegating to fixed update because of physics
        _tagToStop = true;
        //Set the speed after the interruption
        _speedAfterStop = speedAfterInterruption;
    }

    #endregion

    #region Debug

    private void ProcessLanding(Vector3 position)
    {
        if(!debug) return;

        Vector3 landingPosition = transform.position;
        landingPosition.y = position.y;
        float distance = Vector3.Distance(landingPosition, position);
    
        Debug.Log($"Landing at {landingPosition} {distance} units away of target in plane XZ");
    }

    #endregion
}
