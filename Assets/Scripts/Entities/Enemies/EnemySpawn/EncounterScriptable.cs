using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Enemies/EncounterData", order = 1)]
public class EncounterScriptable : ScriptableObject
{
    public WaveInfo[] waves;
    public float timeBetweenWaves;
}

[System.Serializable]
public class WaveInfo
{
    public string enemyName;
    public int numberOfEnemies;
}
