using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class RevertProgressActions : MonoBehaviour
{
    public SpawnPoint resetData;
    public ZoneActions[] zonesEvents;
    public static RevertProgressActions instance;
    private void OnEnable()
    {
        instance = this;
        //ReEnable this code when ContinueSave Game
        /*
        if (resetData == null) return;
        if (SceneManager.GetActiveScene().name != resetData.scnName) return;
        for (int i = 0; i < resetData.zoneID; i++)
        {
            zonesEvents[i].rebuildEvents.Invoke();
        }
        */
    }
    private void OnDisable()
    {
        if (instance == this) instance = null;

    }

    public void SaveZone(int zoneID)
    {
        if (resetData == null) return;
        resetData.zoneID = zoneID;
        resetData.scnName = SceneManager.GetActiveScene().name;
        resetData.Save();
    }

    public void Reset()
    {
        if (resetData == null) return;
        zonesEvents[resetData.zoneID].resetEvents.Invoke();
    }
}
[System.Serializable]
public class ZoneActions
{
    public string name;
    public UnityEvent resetEvents;
    public UnityEvent rebuildEvents;
}
