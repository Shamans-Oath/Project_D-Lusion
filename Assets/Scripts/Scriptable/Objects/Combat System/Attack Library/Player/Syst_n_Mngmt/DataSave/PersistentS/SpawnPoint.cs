using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Spawnpoint", menuName = "Data/03-SpawnPoint")]
public class SpawnPoint : PersistentScriptableObject
{
    public string scnName;
    public int zoneID;

    public float x;
    public float y;
    public float z;


    public override void Reset()
    {

        scnName = "";
        x = 0;
        y = 0;
        z = 0;

        base.Reset();
    }

    public void SettPoint(Transform spawnPlace)
    {
        x = spawnPlace.position.x;
        y = spawnPlace.position.y;
        z = spawnPlace.position.z;
    }

    public void SettPoint(Vector3 spawnPlace)
    {
        x = spawnPlace.x;
        y = spawnPlace.y;
        z = spawnPlace.z;
    }
}
