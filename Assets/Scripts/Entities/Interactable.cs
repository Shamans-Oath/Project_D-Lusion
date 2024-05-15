using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool onFirstTriger;
    public UnityEvent[] eventList;
    public bool loopable;
    private int index = 0;


    public void ExcecuteAction()
    {
        if(eventList.Length > 0)
        {
            eventList[index].Invoke();

            if(index < eventList.Length-1)
            {
                index++;
            }
            else if(loopable == true)
            {
                index = 0;
            }

        }


    }
}
