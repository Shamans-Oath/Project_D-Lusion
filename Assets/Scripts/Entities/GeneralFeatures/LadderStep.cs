using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderStep : MonoBehaviour
{
    [Header("Parameters")]
    public float stepHeight = .05f;
    public float stepDepth = .5f;
    public float stepDistance;
    public Vector3 stepSize;
    public AnimationCurve stepCurve;
    [SerializeField] private bool _onStep;
    private RaycastHit _stepHit;
    public bool onStep { get { return _onStep; } }
    private Vector3 _flattenDirection;

    [Header("Time Management")]
    public float timeToStep = 1f;
    [SerializeField] private float _stepTimer;

    [Header("References")]
    public Slope cmp_slope;
    public PlayerMovement cmp_move;
    public Collider cmp_collider;

    private void Awake()
    {
        if (cmp_slope == false && gameObject.GetComponent<Slope>()) cmp_slope = gameObject.GetComponent<Slope>();
        if (cmp_move == false && gameObject.GetComponent<PlayerMovement>()) cmp_move = gameObject.GetComponent<PlayerMovement>();
        if (cmp_collider == false && gameObject.GetComponent<Collider>()) cmp_collider = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if (_onStep)
        {
            _stepTimer += Time.deltaTime;
        } else _stepTimer = 0;
    }

    public void OnStep(Vector2 direction)
    {
        _flattenDirection = cmp_move.CalculateDirection(direction);

        Vector3 origin = transform.position + transform.up * stepHeight + _flattenDirection * stepDepth;

        if (Physics.BoxCast(origin, stepSize / 2, _flattenDirection, out _stepHit, transform.rotation, stepDistance, cmp_slope.slopeLayer))
        {
            _onStep = !cmp_slope.IsSlopeSurface(_stepHit.normal);
            return;
        }

        _onStep = false;
    }

    public Vector3 OnStepPlane(Vector3 direction)
    {
        if (_onStep == false) return direction;

        if (_stepHit.normal == Vector3.zero) return direction;

        Vector3 flattenDirection = new Vector3(direction.x, 0, direction.z);

        float value = Mathf.Clamp01(_stepTimer / timeToStep);

        float stepInterpolation = stepCurve.Evaluate(value);

        Vector3 projection = flattenDirection.normalized * (1 - stepInterpolation) + Vector3.up * stepInterpolation;

        return projection.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + transform.up * stepHeight + _flattenDirection* stepDepth;
        Vector3 end = transform.position + transform.up * stepHeight + _flattenDirection * (stepDepth + stepDistance);
        Gizmos.DrawWireCube(origin, stepSize);
        Gizmos.DrawWireCube(end, stepSize);
    }
}
