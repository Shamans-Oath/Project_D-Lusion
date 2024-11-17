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
    public GameState initialState;


    public static event Action StateChanged = delegate { };
    public static GameState gameState;
    public static ActionControls gameInputSystem;
    public enum GameState
    {
        Gameplay,
        OnPause,
        OnMenu

    };

    private void Awake()
    {
        manager = this;
        SetInputSystem();
        GameManager.StateChanged += CheckState;
        SetState(initialState);
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

        switch(gameState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1;
                manager.ToggleActionMap(gameInputSystem.GamePlay);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AudioManager.instance.TogglePause(false);
                break;
            case GameState.OnPause:
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                manager.ToggleActionMap(gameInputSystem.UI);
                AudioManager.instance.TogglePause(true);
                break;
            case GameState.OnMenu:
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                manager.ToggleActionMap(gameInputSystem.UI);
                AudioManager.instance.TogglePause(true);
                break;
        }

        /*if (gameState != GameState.OnPause)
        {
            Time.timeScale = 1;
            manager.ToggleActionMap(gameInputSystem.GamePlay);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AudioManager.instance.TogglePause(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            manager.ToggleActionMap(gameInputSystem.UI);
            AudioManager.instance.TogglePause(true);
        }*/
    }

    public void ToggleMenus()
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                SetState(GameState.OnPause);
                //ToggleActionMap(gameInputSystem.UI);
                //AudioManager.instance.TogglePause(false);
                //AudioManager.instance.ToggleMute("GameplaySFXVolume");
            break;

            case GameState.OnPause:
                SetState(GameState.Gameplay);
                //ToggleActionMap(gameInputSystem.GamePlay);
                //AudioManager.instance.TogglePause(true);
                //AudioManager.instance.ToggleMute("GameplaySFXVolume");
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

    public void ChangeState(string name)
    {
        try
        {
            GameState enumerable = (GameState)System.Enum.Parse(typeof(GameState), name);
            SetState(enumerable);
        }
        catch (System.Exception)
        {
            Debug.LogErrorFormat("Parse: Can't convert {0} to enum, please check the spell.", name);
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
