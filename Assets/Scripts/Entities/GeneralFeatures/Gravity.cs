using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Gravity")]
    public float gravity;
    public float maxVerticalSpeed;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Vector3 feetSize;
    public Vector3 offset;
    [SerializeField] private bool _isGrounded;
    public bool isGrounded { get { return _isGrounded; } }

    [Header("References")]
    public Rigidbody cmp_rb;
    public Collider cmp_collider;

    private void Awake()
    {
        if (cmp_rb == false && gameObject.GetComponent<Rigidbody>()) cmp_rb = gameObject.GetComponent<Rigidbody>();
        if (cmp_collider == false && gameObject.GetComponent<Collider>()) cmp_collider = gameObject.GetComponent<Collider>();
    }

    public void ApplyGravity(float multiplier)
    {
        ApplyGravity(Vector3.up, multiplier);
    }

    public void ApplyGravity(Vector3 direction, float multiplier)
    {
        if (IsGrounded()) return;

        cmp_rb.AddForce(-direction * gravity * multiplier, ForceMode.VelocityChange);
    }

    public void LimitVerticalSpeed()
    {
        if(Mathf.Abs(cmp_rb.velocity.y) > maxVerticalSpeed)
        {
            cmp_rb.velocity = new Vector3(cmp_rb.velocity.x, maxVerticalSpeed * Mathf.Sign(cmp_rb.velocity.y), cmp_rb.velocity.z);
        }
    }

    public bool IsGrounded()
    {
        Vector3 origin = transform.position + offset - feetSize.y / 2 * transform.up;

        _isGrounded = Physics.OverlapBox(origin, feetSize / 2, transform.rotation, groundLayer).Length > 0;

        return _isGrounded;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position + offset - feetSize.y / 2 * transform.up, feetSize);
    }
}
