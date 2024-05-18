using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    public string intrTag;
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
        if(collision.gameObject.CompareTag(intrTag))
        {
            interactObj = collision.gameObject;
            itrc = interactObj.GetComponent<Interactable>();
            if (itrc.onFirstTriger) Interact();
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
