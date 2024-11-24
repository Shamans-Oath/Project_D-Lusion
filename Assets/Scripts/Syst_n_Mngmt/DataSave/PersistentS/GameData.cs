using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New ElemntData", menuName = "Data/02-GameData")]
public class GameData : PersistentScriptableObject
{
    public int language;

    public float sens;
    public float genVolume;
    public float mscVolume;
    public float sndVolume;

    public override void Reset()
    {
    
        language= 1;
        sens = 0.5f;
        genVolume = 0.5f;
        mscVolume = 0.5f;
        sndVolume = 0.5f;

       
        base.Reset();
    }
}
