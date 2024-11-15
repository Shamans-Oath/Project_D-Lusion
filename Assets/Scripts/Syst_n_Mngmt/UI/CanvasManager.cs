using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject pauseMenu;
    public CanvasObj[] Menus;

    bool pausedByTutorial;

    void Awake()
    {
        instance = this;
        GameManager.SetInputSystem();
    }

    void OnEnable()
    {        
        GameManager.gameInputSystem.GamePlay.Escape.performed +=_=> TogglePause();
        GameManager.gameInputSystem.UI.Interact.performed +=_=> ToggleTutorialPause("ComboPopup", "Bloody Combo", false);
        GameManager.gameInputSystem.UI.Escape.performed +=_=> TogglePause();
    }

    void OnDisable()
    {        
        GameManager.gameInputSystem.GamePlay.Escape.performed -=_=> TogglePause();
        GameManager.gameInputSystem.UI.Interact.performed -=_=> ToggleTutorialPause("ComboPopup", "Bloody Combo", false);
        GameManager.gameInputSystem.UI.Escape.performed -=_=> TogglePause();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.T))
        {
            ToggleTutorial("InputPopup", "Movimiento");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleTutorial("InputPopup", "Camara");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleTutorial("InputPopup", "Atacar");
        }*/

        /*if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleTutorialPause("ComboPopup", "Bloody Combo");
        }*/
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

    public void ToggleTutorial(string menuName, string tutorialName)
    {
        CanvasObj s = Array.Find(Menus, canvasobj => canvasobj.name == menuName);

        TutorialManager tutorialManager = s.canvObj.GetComponent<TutorialManager>();

        if (s.canvObj.activeInHierarchy)
        {
            string activeTutorial = tutorialManager.lastTutorial;

            if(tutorialName != activeTutorial)
            {
                tutorialManager.StartCoroutine(tutorialManager.UpdateTutorial(tutorialName));
            }
            else
            {
                tutorialManager.StartCoroutine("ExitTutorial");
            }            
        }
        else
        {
            s.canvObj.SetActive(true);
            tutorialManager.LoadTutorial(tutorialName);            
        }
    }

    public void ToggleTutorialPause(string menuName, string tutorialName, bool toActivate)
    {
        CanvasObj s = Array.Find(Menus, canvasobj => canvasobj.name == menuName);

        Debug.Log("TestCanvas");

        TutorialManager tutorialManager = s.canvObj.GetComponent<TutorialManager>();

        if (s.canvObj.activeInHierarchy && toActivate == false)
        {
            if (pausedByTutorial)
            {
                tutorialManager.StartCoroutine("ExitTutorial");
                pausedByTutorial = false;
                GameManager.manager.ToggleMenus();
                AudioManager.instance.PlaySound("Boton");
            }            
        }
        else if(!s.canvObj.activeInHierarchy && toActivate == true)
        {
            if (s.oneTimeTrigger == true)
            {
                return;
            }
            else
            {
                s.oneTimeTrigger = true;
                s.canvObj.SetActive(true);
                tutorialManager.LoadTutorial(tutorialName);
                pausedByTutorial = true;
                GameManager.manager.ToggleMenus();
                AudioManager.instance.PlaySound("AparicionTutorial");
            }            
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
            AudioManager.instance.PlaySound("Pausa");
        }

        GameManager.manager.ToggleMenus();
    }

    public void CallUISound(string name)
    {
        AudioManager.instance.PlaySound(name);
    }
}

[System.Serializable]
public class CanvasObj
{
    public string name;
    public GameObject canvObj;
    public bool oneTimeTrigger;
}

