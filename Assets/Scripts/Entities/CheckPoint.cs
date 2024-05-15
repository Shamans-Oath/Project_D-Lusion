using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public SpawnPoint spwPoint;
    
    public void ReSpawn()
    {
        if(gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        gameObject.transform.position = new Vector3(spwPoint.x, spwPoint.y, spwPoint.z);
    }
}
