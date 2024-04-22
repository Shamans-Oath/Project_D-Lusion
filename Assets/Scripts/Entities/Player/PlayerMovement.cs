using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody cmp_rb;

    public bool allowMovement = true;
    
    [SerializeField]
    float maxDistance = 0.5f;
    [SerializeField]
    Vector3 boxSize;
    public LayerMask layerMask;

    public float rotationSpeed;
    public float Speed;
    public float Gravity;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        cmp_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMovement)
        {
            Movement(Speed);
            Rotation(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump(jumpForce);
            }
        }
    }

    void FixedUpdate()
    {
        ApplyGravity(Gravity);
    }

    public void ApplyGravity(float Gravity)
    {
        if (!isGrounded())
            cmp_rb.AddForce(new Vector2(0, -Gravity), ForceMode.Force);
    }

    public void Rotation(float horizontalInput, float verticalInput)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput; 

        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
        }

    }

    public void Movement(float Speed)
    {
        Vector3 movementVector = new Vector3(CameraRelativeMovement().x, 0, CameraRelativeMovement().z).normalized * Speed;
        Vector3 verticalVelocity = new Vector3(0, cmp_rb.velocity.y, 0);

        cmp_rb.velocity = movementVector + verticalVelocity;
    }

    public void Jump(float jumpForce)
    {
        if (isGrounded())
        {
            cmp_rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    Vector3 CameraRelativeMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 rightRelativeHorizontalInput = horizontalInput * right;
        Vector3 forwardRelativeVerticalInput = verticalInput * forward;

        return rightRelativeHorizontalInput + forwardRelativeVerticalInput;
    }

    public Vector3 CameraRelativeMovement(float horizontalInput, float verticalInput)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 rightRelativeHorizontalInput = horizontalInput * right;
        Vector3 forwardRelativeVerticalInput = verticalInput * forward;

        return rightRelativeHorizontalInput + forwardRelativeVerticalInput;
    }

    bool isGrounded()
    {
        if(Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ToggleMovement()
    {
        if(allowMovement)
        {
            allowMovement = false;
        }
        else
        {
            allowMovement = true;
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position - transform.up * maxDistance, boxSize);    
    }*/
}
