using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float maxDistance = 0.5f;
    public float gravity;
    public Rigidbody cmp_rb;
    public Vector3 boxSize;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGravity(float apliedGravity)
    {
        if (!IsGrounded())
            cmp_rb.AddForce(new Vector2(0, -apliedGravity), ForceMode.Force);
    }

    public bool IsGrounded()
    {
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    Gizmos.DrawWireCube(transform.position - transform.up * maxDistance, boxSize);    
    //}
}
