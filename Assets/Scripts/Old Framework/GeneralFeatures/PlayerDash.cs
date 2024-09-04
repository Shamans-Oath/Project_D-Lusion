using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDash : MonoBehaviour
{
    [System.Serializable]
    public struct DebugEvent
    {
        public string comment;
        public DashEvent debugEvent;
        public UnityEvent debugAction;
        public DashVariable[] logKeys;
    }

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
    public float realDashCooldown => Mathf.Max(_dashCooldownTimer / timeMultiplier, 0);

    [Header("References")]
    public Collider cmp_collider;
    public Rigidbody cmp_rb;
    public PlayerMovement cmp_movement;
    public Gravity cmp_gravity;

    [Header("Debug")]
    public bool debug;
    public enum DashEvent
    {
        StartCharge,
        StartDash, //Also EndCharge
        MidCurve, //When vertical speed goes to zero
        Land,
        CooldownRestart
    }

    public enum DashVariable
    {
        DashCooldown,
        DashDuration,
        IsDashing,
        IsCharging,
        TimeToLand,
        RealCooldown,
        Speed,
        RealSpeed,
        TargetPosition,
        TagToStop,
        SpeedAfterStop
    }

    public List<DebugEvent> debugEvents;

    private void Awake()
    {
        if (cmp_rb == false) cmp_rb = GetComponent<Rigidbody>();
        if (cmp_movement == false) cmp_movement = GetComponent<PlayerMovement>();
        if (cmp_collider == false) cmp_collider = GetComponent<Collider>();
        if (cmp_gravity == false) cmp_gravity = GetComponent<Gravity>();
    }

    private void Update()
    {
        //Decrease Dash Timers When Cooldown is Active
        WaitQueue(Time.deltaTime * timeMultiplier);

        if (_isDashing && !_isCharging)
        {
            DashMovement();
        }
    }

    private void FixedUpdate()
    {
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
        float gravity = cmp_gravity.gravity;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = position;
        Vector3 direction = targetPosition - startPosition;
        direction.y = 0f;

        float height = baseHeight + (targetPosition - startPosition).y;

        float flightTime = ProjectileMotion.GetFlightTime(height, gravity * gravityMultiplierUpHill) + ProjectileMotion.GetFlightTime(baseHeight, gravity * gravityMultiplierDownHill);

        float length = direction.magnitude;

        float newHorizontalSpeed = length / flightTime;

        //Get the speed to reach the target Position
        _speed = ProjectileMotion.GetStartSpeed(direction.normalized, height, newHorizontalSpeed, gravity * gravityMultiplierUpHill);

        //Disable movement while dashing and charging
        DashState(true);

        cmp_rb.velocity = Vector3.zero;

        //Set Cooldown On Charge
        SetTimers(flightTime + chargeTimeSeconds * timeMultiplier);

        CallDashEventDebug(DashEvent.StartCharge);

        //Wait for the charge time
        yield return new WaitForSeconds(chargeTimeSeconds);

        _isCharging = false;

        CallDashEventDebug(DashEvent.StartDash);
    }

    private void DashMovement()
    {
        float timeDelta = Time.deltaTime * timeMultiplier;

        float gravity = -Mathf.Abs(cmp_gravity.gravity);

        gravity *= _speed.y < 0 ? gravityMultiplierDownHill : gravityMultiplierUpHill;

        Vector3 previousSpeed = _speed;

        _speed += Vector3.up * gravity * timeDelta;

        bool midCurve = _speed.y <= 0 && previousSpeed.y > 0;

        if(midCurve) CallDashEventDebug(DashEvent.MidCurve);

        //Set the speed to the rigidbody
        Vector3 realSpeed = _speed * timeMultiplier;

        transform.position += realSpeed * Time.deltaTime;
    }

    #region Time Management
    private void WaitQueue(float timeDelta)
    {
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= timeDelta;
            if(_dashCooldownTimer <= 0) CallDashEventDebug(DashEvent.CooldownRestart);
        }
        if (_dashDurationTimer > 0) _dashDurationTimer -= timeDelta;
    }

    private void SetTimers(float dashDuration)
    {
        _dashCooldownTimer = dashCooldownSeconds * timeMultiplier + dashDuration;

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

        CallDashEventDebug(DashEvent.Land);

        //Process Landing
        _targetPosition = Vector3.zero;
    }

    private void DashState(bool state)
    {
        _isDashing = state;

        //Disable movement while dashing
        cmp_movement.ToggleMovement(!state);
    }

    public void InterruptDash()
    {
        InterruptDash(Vector3.zero);
    }

    public void FlipDash(float angle)
    {
        InterruptDash(Quaternion.Euler(new Vector3(0f, angle, 0f)) * _speed * .18f);
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

    private string VariableLog(DashVariable key)
    {
        switch(key)
        {
            case DashVariable.DashCooldown:
                return $"Dash Cooldown > {_dashCooldownTimer} altered seconds left";
            case DashVariable.DashDuration:
                return $"Dash Duration > {_dashDurationTimer} altered seconds left";
            case DashVariable.IsDashing:
                return $"Is Dashing > {YesOrNo(_isDashing)}";     
            case DashVariable.IsCharging:
                return $"Is Charging > {YesOrNo(isCharging)}";              
            case DashVariable.TimeToLand:
                return $"Time To Land > {timeToLand} seconds left";              
            case DashVariable.RealCooldown:
                return $"Real Dash Cooldown > {realDashCooldown} seconds left";
            case DashVariable.Speed:
                return $"Speed > XZ {new Vector3(_speed.x, 0f, _speed.z).magnitude} Y {_speed.y} virtual u/s";
            case DashVariable.RealSpeed:
                return $"Real Speed > XZ {new Vector3(_speed.x, 0f, _speed.z).magnitude * timeMultiplier} Y {_speed.y * timeMultiplier} real u/s";
            case DashVariable.TargetPosition:
                return $"Target Position > XYZ {_targetPosition} Distance XZ {Vector3.Distance(new Vector3(transform.position.x, _targetPosition.y, transform.position.z), _targetPosition)}";
            case DashVariable.TagToStop:
                return $"Tagged To Stop > {YesOrNo(_tagToStop)}";
            case DashVariable.SpeedAfterStop:
                return $"Speed After Stop > {_speedAfterStop} real u/s";

            default:
                return $"Key {key} not found";
        }

        string YesOrNo(bool value)
        {
            return value ? "Yes" : "No";
        }
    }

    private void LogVariables(DashEvent dashEvent, params DashVariable[] keys)
    {
        if(!debug) return;

        string debugEventName;

        switch (dashEvent)
        {
            case DashEvent.StartCharge:
                debugEventName = "Start Charge";
                break;
            case DashEvent.StartDash:
                debugEventName = "Start Dash";
                break;
            case DashEvent.MidCurve:
                debugEventName = "Mid Curve";
                break;
            case DashEvent.Land:
                debugEventName = "Land";
                break;
            case DashEvent.CooldownRestart:
                debugEventName = "Cooldown Restart";
                break;

            default:
                debugEventName = "Unknown";
                break;
        }

        string log = $"Player Dash > {debugEventName} > ";

        for(int i = 0; i < keys.Length; i++)
        {
            DashVariable key = keys[i];
            Debug.Log(log + VariableLog(key));
        }
    }

    private List<DebugEvent> FindAllDebugEvents(DashEvent dashEvent)
    {
        return debugEvents.FindAll(debugEvents => debugEvents.debugEvent == dashEvent);
    }

    private void CallDashEventDebug(DashEvent dashEvent)
    {
        if (!debug) return;

        List<DebugEvent> debugEvents = FindAllDebugEvents(dashEvent);

        foreach (DebugEvent debugEvent in debugEvents)
        {
            Debug.Log(debugEvent.comment);

            if(debugEvent.debugAction != null) debugEvent.debugAction.Invoke();
            LogVariables(debugEvent.debugEvent, debugEvent.logKeys);
        }
    }

    #endregion
}
