using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject[] Screens;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu(int indexNumber)
    {
        if (Screens[indexNumber].activeInHierarchy)
        {
            Screens[indexNumber].SetActive(false);
        }
        else
        {
            Screens[indexNumber].SetActive(true);
        }
    }

    public void CallAnimation(int indexNumber, string triggerName)
    {
        if (Screens[indexNumber].GetComponent<Animator>() != null)
        {
            Animator temp_Anim = Screens[indexNumber].GetComponent<Animator>();

            temp_Anim.SetTrigger(triggerName);
        }
        else
        {
            Debug.Log("This canvas doesn't have an animator");
        }
    }
}
