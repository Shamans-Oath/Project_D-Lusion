using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.EventSystems;

public class CamMovimientoMenu : MonoBehaviour
{
    //SHAMAN
   public GameObject Press;
    public GameObject HiMTitle;

    public GameObject MenuPrincipal;
    
    public Transform[] viewsMp;


    public float transitionSpeedMp;
    
    public Transform currentView;

    public MainMenuControllerAnt mainMenuController;




        void Start()
    {
        currentView = viewsMp[0];

    }


    public void StartMenu()
    {
        /*if (currentView == viewsMp[0]) //lugar inicial
        {*/
            currentView = viewsMp[1];
            Press.SetActive(false);
            HiMTitle.SetActive(false);
        /*}*/
    }

   private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeedMp);

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x,
            Time.deltaTime * transitionSpeedMp),

            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y,
            Time.deltaTime * transitionSpeedMp),

            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z,
            Time.deltaTime * transitionSpeedMp)

            );

        transform.eulerAngles = currentAngle;
    }

    public void OnTriggerEnter(Collider other) 
   {
       if (currentView == viewsMp[1])//MM
       {
           MenuPrincipal.SetActive(true);
          
       }
       else if (currentView == viewsMp[2]) //OP
       {
           mainMenuController.optionsMainMenu.SetActive(true);
       }

   }
     public void OnTriggerExit(Collider other)
    {
        if (currentView == viewsMp[2])
        {
            MenuPrincipal.SetActive(false);
        }
        else if (currentView == viewsMp[1])
        {
            mainMenuController.optionsMainMenu.SetActive(false);
        }

        
    }
}
