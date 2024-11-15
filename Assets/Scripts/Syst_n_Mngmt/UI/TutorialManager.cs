using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI cmp_TextMeshPro;
    public Image buttonIcon;
    public Animator cmp_Animator;

    public string lastTutorial;
    Coroutine timerCoroutine, exitCoroutine;


    public Tutorial[] tutorials;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTutorial(string tutorialName)
    {
        Tutorial t = Array.Find(tutorials, tutorial => tutorial.Name == tutorialName);

        cmp_TextMeshPro.text = t.Name;
        buttonIcon.sprite = t.Icon;
        lastTutorial = tutorialName;
        AudioManager.instance.PlaySound("AparicionTutorial");

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(PopupTimer(t.Time));
    }

    public IEnumerator PopupTimer(float seconds)
    {
        if(seconds == 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(seconds);

        if (exitCoroutine != null) StopCoroutine(exitCoroutine);
        exitCoroutine = StartCoroutine(ExitTutorial());
    }

    public IEnumerator UpdateTutorial(string tutorialName)
    {
        AnimatorStateInfo stateInfo = cmp_Animator.GetCurrentAnimatorStateInfo(0);

        /*if(stateInfo.IsName("SlideIn"))
        {
            cmp_Animator.SetTrigger("Exit");

            yield return new WaitForSeconds(0.5f);

            LoadTutorial(tutorialName);
            cmp_Animator.SetTrigger("Start");
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            LoadTutorial(tutorialName);
            cmp_Animator.SetTrigger("Start");
        }*/

        cmp_Animator.SetTrigger("Exit");

        yield return new WaitForSeconds(0.5f);

        LoadTutorial(tutorialName);
        cmp_Animator.SetTrigger("Start");
    }

    public IEnumerator ExitTutorial()
    {
        cmp_Animator.SetTrigger("Exit");

        yield return new WaitForSeconds(0.5f);

        //cmp_Animator.gameObject.SetActive(false);
        cmp_Animator.transform.parent.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class Tutorial
{
    public string Name;
    public Sprite Icon;
    public float Time;
}
