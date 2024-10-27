using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class CanvasLoaders : MonoBehaviour
{
    public string menuName, tutorialName;

    public bool tutorialPauses;
    public bool tutorialTriggered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && tutorialTriggered == false)
        {
            if(tutorialPauses == false)
            {
                CanvasManager.instance.ToggleTutorial(menuName, tutorialName);
            }
            else
            {
                CanvasManager.instance.ToggleTutorialPause(menuName, tutorialName);
            }
            
            tutorialTriggered = true;
        }
    }
}
