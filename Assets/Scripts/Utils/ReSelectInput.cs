using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ReSelectInput : MonoBehaviour
{
    public EventSystem evSys;
    private GameObject lastSelect;
    private bool enableCheck;
    // Start is called before the first frame update
    void Start()
    {
        //evSys.
    }

    // Update is called once per frame
    void Update()
    {
        if (evSys != null)
        {
            UnselectCheck();
            DetectToSelect();
        }
    }
    public void UnselectCheck()
    {
        if (lastSelect == evSys.currentSelectedGameObject)
        {
            enableCheck = false;
            return;
        }
        else if (evSys.currentSelectedGameObject != null)
        {
            enableCheck = false;
            lastSelect = evSys.currentSelectedGameObject;
        }
        else if (evSys.currentSelectedGameObject == null)
        {
            enableCheck = true;
        }
               
    }
    public void DetectToSelect()
    {
        if (enableCheck != true) return;
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs( Input.GetAxisRaw("Vertical")) > 0)
        {
            Debug.Log("1");
            if (lastSelect.activeInHierarchy == false) return;
            evSys.SetSelectedGameObject(lastSelect);
            Debug.Log("2");
        }
    }
}
