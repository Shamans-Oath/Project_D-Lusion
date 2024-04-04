using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New ElemntData", menuName = "Data/02-GameData")]
public class GameData : PersistentScriptableObject
{
    public int language; 

    public override void Reset()
    {
    
        language= 1;
       
        base.Reset();
    }
}
