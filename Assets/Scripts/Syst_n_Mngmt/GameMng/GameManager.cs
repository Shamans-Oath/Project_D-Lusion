using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    }

    #region GameState&InputConfigIssues
    public static void SetInputSystem()
    {
        if (gameInputSystem == null)
        {

            Debug.Log("GameManager: add input system");
            gameInputSystem = new ActionControls();
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

    #endregion

    #region TimeSettings
    private static float defaultTime = 1;
    private static float currentGameplayTime = 1;
    public static void SetTimeTo(float timeValue)
    {
        Time.timeScale = timeValue;
    }

    public static void LerpTimeTo(float timeValue, float secconds)
    {
        
    }
    public IEnumerator ChangeTime(float timeValue, float secconds)
    {

        yield return 0;
    }
    public static void SaveCurrentGameplayTime()
    {
        currentGameplayTime = Time.timeScale;
    }
    #endregion

    #region CursorSettings

    #endregion


}
