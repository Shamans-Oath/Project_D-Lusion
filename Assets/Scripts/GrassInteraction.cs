using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassInteraction : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            Debug.Log("x: " + transform.position.x + " y: " + transform.position.y);
        }

        Shader.SetGlobalVector("_Player", transform.position+Vector3.up * 0.5f);
    }
}
