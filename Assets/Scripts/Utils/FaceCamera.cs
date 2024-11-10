using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public bool ignoreX = false;
    public bool ignoreY = false;
    public bool ignoreZ = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        if (this.gameObject.activeSelf == false || Camera_System.instance.mainCamera == null|| (ignoreX&&ignoreY&&ignoreZ)) return;
        Vector3 rotateDirection = Camera_System.instance.mainCamera.transform.position;
        if (ignoreX) rotateDirection.x = gameObject.transform.forward.x;
        if (ignoreY) rotateDirection.y = gameObject.transform.forward.y;
        if (ignoreZ) rotateDirection.z = gameObject.transform.forward.z;


        gameObject.transform.LookAt(new Vector3(rotateDirection.x, rotateDirection.y, rotateDirection.z));

    }
}
