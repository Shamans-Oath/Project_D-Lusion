using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Enemies/EncounterData", order = 1)]
public class EncounterScriptable : ScriptableObject
{
    
}

[System.Serializable]
public class EnemyInfo
{
    public string enemyName;
    public int numberOfEnemies;
}
