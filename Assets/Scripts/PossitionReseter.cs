using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossitionReseter : MonoBehaviour
{
    public Vector3 resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        resetPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = resetPosition;
    }
}
