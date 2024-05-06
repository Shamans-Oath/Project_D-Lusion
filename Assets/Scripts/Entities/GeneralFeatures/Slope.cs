using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    [Header("Parameters")]
    public float extraDistance;
    public LayerMask slopeLayer;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    [SerializeField] private bool _onSlope;
    public bool onSlope { get { return _onSlope; } }
    public Vector3 slopeNormal { get { return slopeHit.normal; } }

    public void OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, extraDistance, slopeLayer))
        {
            float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);

            _onSlope = slopeAngle != 0 && slopeAngle <= maxSlopeAngle;

            return;
        }

        _onSlope = false;
    }

    public bool IsSlopeSurface(Vector3 normal)
    {
        float slopeAngle = Vector3.Angle(normal, Vector3.up);

        return slopeAngle != 0 && slopeAngle <= maxSlopeAngle;
    }

    public Vector3 OnSlopePlane(Vector3 direction)
    {
        if(_onSlope == false) return direction;

        if(slopeHit.normal == Vector3.zero) return direction;
    
        return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }
}
