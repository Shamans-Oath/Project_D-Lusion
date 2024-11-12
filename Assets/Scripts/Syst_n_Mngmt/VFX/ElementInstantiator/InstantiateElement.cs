using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateElement : MonoBehaviour
{
    public ElementInstancer spawner;
    public int listNumID;

    private void OnEnable()
    {
        if (spawner) spawner.MoveElementInArrayTo(listNumID, true,gameObject);

    }

    private void OnDisable()
    {
        if (spawner) spawner.MoveElementInArrayTo(listNumID, false, gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
