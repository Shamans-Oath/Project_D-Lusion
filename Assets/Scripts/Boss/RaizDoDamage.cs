using Features;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaizDoDamage : MonoBehaviour
{
    public float timeToDestroy = 20f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Life>().Health(50,false);
        }
    }
}
