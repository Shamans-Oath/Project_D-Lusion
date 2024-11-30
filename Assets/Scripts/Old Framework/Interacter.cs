using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    public string[] intrTag;
    public GameObject interactObj;
    private Interactable itrc;

    public void Interact()
    {
        if(itrc != null)
        {
            itrc.ExcecuteAction();
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        for (int i = 0; i < intrTag.Length; i++)
        {
            if (collision.gameObject.CompareTag(intrTag[i]))
            {
                interactObj = collision.gameObject;
                if(interactObj.GetComponent<Interactable>()==false)return;

                itrc = interactObj.GetComponent<Interactable>();
                if (itrc.onFirstTriger) Interact();
            }
        }
        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == interactObj)
        {
            interactObj = null;
            itrc = null;
        }
    }
}
