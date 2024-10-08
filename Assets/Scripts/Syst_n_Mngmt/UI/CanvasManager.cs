using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject pauseMenu;
    public CanvasObj[] Menus;

    void Awake()
    {
        instance = this;
        GameManager.SetInputSystem();
    }

    void OnEnable()
    {
        GameManager.gameInputSystem.Enable();        
        GameManager.gameInputSystem.GamePlay.Escape.performed +=_=> TogglePause();
        //GameManager.gameInputSystem.UI.Escape.performed +=_=> TogglePause();
    }

    void OnDisable()
    {        
        GameManager.gameInputSystem.GamePlay.Escape.performed -=_=> TogglePause();
        //GameManager.gameInputSystem.UI.Escape.performed -=_=> TogglePause();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu(string menuName)
    {
        CanvasObj s = Array.Find(Menus, canvasobj => canvasobj.name == menuName);

        if (s.canvObj.activeInHierarchy)
        {
            s.canvObj.SetActive(false);
        }
        else
        {
            s.canvObj.SetActive(true);
        }
    }

    public void TogglePause()
    {
        Debug.Log("Test");        

        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
        }

        GameManager.manager.ToggleMenus();
    }
}

[System.Serializable]
public class CanvasObj
{
    public string name;
    public GameObject canvObj;
}

