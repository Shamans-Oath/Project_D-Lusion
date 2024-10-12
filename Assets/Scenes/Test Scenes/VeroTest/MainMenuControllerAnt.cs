using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControllerAnt : MonoBehaviour
{
    //sHAMAN
    public GameObject optionsMainMenu;
    public GameObject optionsCloseButton;
    public CamMovimientoMenu movimientoMenu;
    public GameObject menuFirstButton;

    public void OpenOptions()
    {
        movimientoMenu.currentView = movimientoMenu.viewsMp[2];
    }

    public void CloseOptions()
    {
        movimientoMenu.currentView = movimientoMenu.viewsMp[1];
    }
   
}
