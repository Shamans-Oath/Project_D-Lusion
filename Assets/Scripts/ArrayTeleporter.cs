using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTeleporter : MonoBehaviour
{
    public GameObject objectToTeleport;

    public Transform[] pointLists;
    private int indexPointer=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        objectToTeleport.transform.position = pointLists[indexPointer].position;
        indexPointer++;
        if(indexPointer>=pointLists.Length)indexPointer = 0;
    }

    public void TeleportTo(int pointNumber)
    {
        if (indexPointer >= pointLists.Length) pointNumber=pointNumber % (pointLists.Length);
        objectToTeleport.transform.position = pointLists[pointNumber].position;
        indexPointer = pointNumber++;
    }
}
