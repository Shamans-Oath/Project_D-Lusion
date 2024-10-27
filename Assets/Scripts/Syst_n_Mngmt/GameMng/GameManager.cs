using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [ReadOnly]
    public float timeSet;


    public static event Action StateChanged = delegate { };
    public static GameState gameState;
    public static ActionControls gameInputSystem;
    public enum GameState
    {
        Gameplay,
        OnMenu

    };

    private void Awake()
    {
        manager = this;
        SetInputSystem();
        GameManager.StateChanged += CheckState;
        //GameManager.gameInputSystem.GamePlay.Escape.performed +=_=> CheckState();
    }

    #region GameState&InputConfigIssues
    public static void SetInputSystem()
    {
        if (gameInputSystem == null)
        {

            Debug.Log("GameManager: add input system");
            gameInputSystem = new ActionControls();
            gameInputSystem.GamePlay.Enable();
        }
    }

    public static void SetState(GameState state)
    {
        if (gameState != state)
        {
            gameState = state;
            Debug.Log("GameManager: set state to " + state);
            StateChanged.Invoke();
        }
    }

    public void CheckState()
    {
        Debug.Log("ChangeState gamemanager checker");
        if (gameState != GameState.OnMenu)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void ToggleMenus()
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                SetState(GameState.OnMenu);
                ToggleActionMap(gameInputSystem.UI);
            break;

            case GameState.OnMenu:
                SetState(GameState.Gameplay);
                ToggleActionMap(gameInputSystem.GamePlay);
            break;
        }        
    }

    public void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
            return;

        gameInputSystem.Disable();
        actionMap.Enable();
    }

    #endregion

    #region TimeSettings
    private static float defaultTime = 1;
    private static float currentGameplayTime = 1;
    public static void SetTimeTo(float timeValue)
    {
        Time.timeScale = timeValue;
    }
    private static Coroutine timeLerp;
    public static void LerpTimeTo(float timeValue, float secconds)
    {
        if (timeLerp != null) manager.StopCoroutine(timeLerp);
        timeLerp = manager.StartCoroutine(manager.ChangeTime(timeValue, secconds));
    }
    public IEnumerator ChangeTime(float timeValue, float secconds)
    {
        float t = 0;
       
        while (true)
        {
            yield return null;
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeValue, t / secconds);
            t += Time.unscaledDeltaTime;
            if (t > secconds)
            {
                break;
            }
        }


        timeLerp = null;
        yield return 0;
    }
    public static void SaveCurrentGameplayTime()
    {
        currentGameplayTime = Time.timeScale;
    }

    public static void ResetTime()
    {
        Time.timeScale = defaultTime; ;
    }
    #endregion

    #region CursorSettings

    #endregion


}
