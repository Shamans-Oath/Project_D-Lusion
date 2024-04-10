using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;


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
}
