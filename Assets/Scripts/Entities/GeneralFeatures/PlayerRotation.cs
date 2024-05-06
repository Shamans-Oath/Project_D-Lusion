using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Parameters")]
    public float startRotationSpeed;
    public float maxRotationSpeed;
    public float rotationAcceleration; //Degrees per second squared
    [SerializeField] private float _rotationAngularSpeed; //Degrees per second
    public float rotationSpeed { get { return _rotationAngularSpeed; } }
    public AnimationCurve rotationSpeedCurve;
    private Coroutine aceleration;
    [SerializeField] private bool _isRotating;
    public bool IsRotating { get { return _isRotating; } }


    [Header("References")]
    public Camera playerCamera;
    public PlayerMovement cmp_movement;

    private void Awake()
    {
        if (playerCamera == false && GameObject.Find("Main Camera")) playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (cmp_movement == false && gameObject.GetComponent<PlayerMovement>()) cmp_movement = gameObject.GetComponent<PlayerMovement>();
    }

    public void Rotate(Vector2 direction)
    {
        //if the player is not rotating the rotation is stopped an the speed ceases to increase
        if (direction == Vector2.zero)
        {
            _isRotating = false;
            return;
        }

        //If the player is not rotating, the rotation speed is set to the start rotation speed
        if (!_isRotating)
        {
            _rotationAngularSpeed = startRotationSpeed;
        }

        Vector3 flattenDirection = cmp_movement.CalculateDirection(new Vector3(direction.x, 0, direction.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flattenDirection), _rotationAngularSpeed * Time.deltaTime);

        RotateSpeed();

        _isRotating = true;
    }

    public void RotateSpeed()
    {
        if(_isRotating) return;

        aceleration = StartCoroutine(Rotation());
    }

    private IEnumerator Rotation()
    {
        float speedSpan = maxRotationSpeed - startRotationSpeed;

        float time = speedSpan / rotationAcceleration;
        float elapsedTime = 0;

        while(_isRotating && elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            _rotationAngularSpeed = rotationSpeedCurve.Evaluate(t) * speedSpan + startRotationSpeed;
            
            yield return null;
        }

        _rotationAngularSpeed = maxRotationSpeed;
    }
}
