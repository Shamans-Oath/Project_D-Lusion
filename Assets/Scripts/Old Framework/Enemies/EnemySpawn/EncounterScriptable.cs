using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Enemies/EncounterData", order = 1)]
public class EncounterScriptable : ScriptableObject
{
    public Waves[] waves;
    public float timeBetweenWaves;
    public float timeBetweenBatches;
}

[System.Serializable]
public class Waves
{
    public int enemyThreshold;
    public WaveInfo[] waveInfo;
    public int numberOfBatches;
}

[System.Serializable]
public class WaveInfo
{    
    public int poolIndex;
    public string enemyName;
    public int numberOfEnemies;
    public bool usesSubmodule;
    public int submoduleIndex;
}
