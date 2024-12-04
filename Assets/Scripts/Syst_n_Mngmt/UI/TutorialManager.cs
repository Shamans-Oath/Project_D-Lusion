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
    [ReadOnly]
    public bool active = false;

    public string lastTutorial;
    private Tutorial lastTutorialGroup;
    Coroutine timerCoroutine, exitCoroutine;


    public Tutorial[] tutorials;

    private int inputImageNumber = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GameManager.ChangedInputType += (GameManager.InputType type) => 
        {
            inputImageNumber = (int)type;
            if (active == false) return;
            Debug.Log((int)type);
            UpdatePanel(lastTutorialGroup);
        };

        if (inputImageNumber < 0) inputImageNumber = (int)GameManager.currentGameInput;
    }

    private void OnDisable()
    {
        GameManager.ChangedInputType -= (GameManager.InputType type) =>
        {
            inputImageNumber = (int)type;
            if (active == false) return;
            Debug.Log((int)type);
            UpdatePanel(lastTutorialGroup);
        };
    }

    public void LoadTutorial(string tutorialName)
    {
        Tutorial t = Array.Find(tutorials, tutorial => tutorial.Name == tutorialName);

        UpdatePanel(t);
        lastTutorial = tutorialName;
        lastTutorialGroup = t;
        AudioManager.instance.PlaySound("AparicionTutorial");

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(PopupTimer(t.Time));
    }
    public void UpdatePanel(Tutorial t)
    {
        if (t.Icons.Length >= inputImageNumber)
        {
            if (t.Icons[inputImageNumber]) buttonIcon.sprite = t.Icons[inputImageNumber];
        }
        else if (t.Icon != null)
        {
            buttonIcon.sprite = t.Icon;
        }
        else
        {
            buttonIcon.sprite = null;
        }

        if (t.Name != null && Name != null)
        {
            Name.text = t.Name;
        }

        if (t.Description != null && Description != null)
        {
            Description.text = t.Description;
        }

        if (t.Clip != null && cmp_VideoPlayer != null)
        {
            cmp_VideoPlayer.clip = t.Clip;
            cmp_VideoPlayer.Play();
        }
    }

    public IEnumerator PopupTimer(float seconds)
    {
        active = true;
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
        active = true;
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
    [Tooltip("Match de number of icon with de Scheme Control of the Game Input")]
    public Sprite[] Icons;
    public VideoClip Clip;
    [TextArea(1,5)]
    public string Description;
    public float Time;
    public bool oneTimeTrigger;
}
