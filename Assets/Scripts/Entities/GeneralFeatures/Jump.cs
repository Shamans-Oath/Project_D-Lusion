using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce;
    public Rigidbody cmp_rb;

    public void Hop(float jumpForce)
    {
        cmp_rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
    }
}
