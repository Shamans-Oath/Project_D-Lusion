using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI Name, Description;
    public Image buttonIcon;
    public VideoPlayer cmp_VideoPlayer;
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

        Name.text = t.Name;
        if (t.Icon != null)
        {
            buttonIcon.sprite = t.Icon;
        }
        else
        {
            buttonIcon.sprite = null;
        }
        
        if (t.Description != null && Description != null)
        {
            Description.text = t.Description;
        }

        if(t.Clip != null && cmp_VideoPlayer != null)
        {
            cmp_VideoPlayer.clip = t.Clip;
            cmp_VideoPlayer.Play();
        }

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
    public VideoClip Clip;
    [TextArea(1,5)]
    public string Description;
    public float Time;
    public bool oneTimeTrigger;
}
