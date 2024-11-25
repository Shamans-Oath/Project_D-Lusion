using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaizManager : MonoBehaviour
{
    public int maxObjects = 9;
    public List<GameObject> objectList; 

    public void AddObject(GameObject newObject)
    {
        if (objectList == null)
        {
            objectList = new List<GameObject>();
        }


        if (objectList.Count < maxObjects)

        {
            objectList.Add(newObject);
        }
        else

        {
            GameObject oldestObject = objectList[0];
            if (oldestObject != null)
            {
                Destroy(oldestObject);
            }
            ClearList();
            objectList.Add(newObject);
        }

    }

    public void RemoveObject(GameObject objToRemove)
    {

        if (objectList != null && objectList.Contains(objToRemove))


        {
            objectList.Remove(objToRemove);
            Destroy(objToRemove);
        }

    }

    public void ClearList()
    {
        if (objectList != null)
        {
            foreach (GameObject obj in objectList)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }

            }
            objectList.Clear();
        }
    }
}
