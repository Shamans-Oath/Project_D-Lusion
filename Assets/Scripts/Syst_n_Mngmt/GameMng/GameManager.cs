using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    public DataSaver saveGroup;
    public GameData gameData;
    public static GameManager manager;
    [ReadOnly]
    public float timeSet;
    public GameState initialState;


    public static event Action StateChanged = delegate { };
    public static event Action<InputType> ChangedInputType = delegate { };
    public static GameState gameState;
    public static InputType currentGameInput { get; private set; }
    public InputActionAsset inputActions;
    public InputUser user;

    public static ActionControls gameInputSystem;
    public enum GameState
    {
        Gameplay,
        OnPause,
        OnMenu

    };

    public enum InputType
    {
        Keyboard = 0,
        XInputController = 1,
    };

    private void Awake()
    {
        if(saveGroup)saveGroup.LoadData();
        manager = this;
        SetInputSystem();
        

        GameManager.StateChanged += CheckState;
        StartAutoControlSchemeSwitching();
        SetState(initialState);
        

        //GameManager.gameInputSystem.GamePlay.Escape.performed +=_=> CheckState();
    }
    private void OnDestroy() => StopAutoControlSchemeSwitching();

    private void OnEnable()
    {
        ChangedInputType += (InputType sch) => { Debug.Log("Sipasalawea device" + sch); };
    }
    private void OnDisable()
    {
        ChangedInputType -= (InputType sch) => { Debug.Log("Sipasalawea device" + sch); };
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
                if (AudioManager.instance) AudioManager.instance.TogglePause(false);
                break;
            case GameState.OnPause:
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                manager.ToggleActionMap(gameInputSystem.UI);
                if (AudioManager.instance) AudioManager.instance.TogglePause(true);
                break;
            case GameState.OnMenu:
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                manager.ToggleActionMap(gameInputSystem.UI);
                if(AudioManager.instance) AudioManager.instance.TogglePause(true);
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

    #region SchemesControl

    void StartAutoControlSchemeSwitching()
    {
        user = InputUser.CreateUserWithoutPairedDevices();
        /*for (int i = 0; i < inputActions.actionMaps.Count; i++)
        {
            user.AssociateActionsWithUser(inputActions.actionMaps[i]);
        }*/
       
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += InputUser_onUnpairedDeviceUsed;
        //InputUser.on

    }
    private void StopAutoControlSchemeSwitching()
    {
        InputUser.onUnpairedDeviceUsed -= InputUser_onUnpairedDeviceUsed;
        if (InputUser.listenForUnpairedDeviceActivity > 0)
            --InputUser.listenForUnpairedDeviceActivity;

    }

    private void InputUser_onUnpairedDeviceUsed(InputControl ctrl, UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr)
    {
        var device = ctrl.device;
        Debug.Log(ctrl.device + " device");

        /*if ((currentGameInput == InputType.Keyboard) &&
             (/*(device is Pointer) || (device is Keyboard)))
        {
            //InputUser.PerformPairingWithDevice(device, user);
            if (ChangedInputType != null) ChangedInputType(InputType.Keyboard);
            TrigerControlScheme(InputType.Keyboard);

            return;
        }*/

        if (device is Gamepad)
        {
            if (currentGameInput == InputType.XInputController) return;

            if (ChangedInputType != null) ChangedInputType(InputType.XInputController);
            currentGameInput = InputType.XInputController;
            TrigerControlScheme(InputType.XInputController);
            
        }
        else if ((device is Keyboard) /*|| (device is Pointer)*/)
        {
            if (currentGameInput == InputType.Keyboard) return;

            if (ChangedInputType != null) ChangedInputType(InputType.Keyboard);
            currentGameInput = InputType.Keyboard;
            TrigerControlScheme(InputType.Keyboard);

        }
        else return;

    }
    public void TrigerControlScheme(InputType scheme)
    {
        //ChangedInputType.Invoke(scheme);
        
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
