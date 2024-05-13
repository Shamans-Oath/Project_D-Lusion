using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableToggle : MonoBehaviour
{
    void OnDisable()
    {
        SpawnManager.instance.remainingEnemies.Remove(this.gameObject);
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
