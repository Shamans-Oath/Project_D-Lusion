using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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

    private void OnEnable()
    {
        GameManager.gameInputSystem.UI.Navigate.performed += _ => DetectToSelect();
    }

    private void OnDisable()
    {
        GameManager.gameInputSystem.UI.Navigate.performed -= _ => DetectToSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (evSys != null)
        {
            UnselectCheck();
            //DetectToSelect();
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
        /*if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs( Input.GetAxisRaw("Vertical")) > 0)
        {*/

            if (lastSelect.activeInHierarchy == false) return;
            evSys.SetSelectedGameObject(lastSelect);

        //}
    }
}
