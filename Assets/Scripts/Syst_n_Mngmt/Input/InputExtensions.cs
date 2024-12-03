using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/*Helper class for input management. Only have exactly one permanently active object
in your scene at any time holding an instance of this - or create a singleton if not possible otherwise*/
public class InputExtensions : MonoBehaviour
{
    // Subscribe to this event
    public static event Action<ControlScheme> OnInputSchemeChanged;
    public static ControlScheme CurrentControlScheme { get; private set; }
    public InputActionAsset inputActions;

    public InputUser user;
    private void Start() => StartAutoControlSchemeSwitching();
    private void OnDestroy() => StopAutoControlSchemeSwitching();

    void StartAutoControlSchemeSwitching()
    {
        user = InputUser.CreateUserWithoutPairedDevices();
        for(int i = 0; i<inputActions.actionMaps.Count;i++)
        {
            user.AssociateActionsWithUser(inputActions.actionMaps[i]);
        }
        /*user.AssociateActionsWithUser(inputActions.actionMaps[0])*/; // need to be there at least one actionmap defined in InputActionAsset, otherwise rises exception during paring process
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += InputUser_onUnpairedDeviceUsed;
        //user.UnpairDevices();
    }
    private void StopAutoControlSchemeSwitching()
    {
        InputUser.onUnpairedDeviceUsed -= InputUser_onUnpairedDeviceUsed;
        if (InputUser.listenForUnpairedDeviceActivity > 0)
            --InputUser.listenForUnpairedDeviceActivity;
       // user.UnpairDevicesAndRemoveUser();
    }

    private void InputUser_onUnpairedDeviceUsed(InputControl ctrl, UnityEngine.InputSystem.LowLevel.InputEventPtr eventPtr)
    {
        var device = ctrl.device;
        Debug.Log(ctrl.device + " device");

        if ((CurrentControlScheme == ControlScheme.KeyboardMouse) &&
             ((device is Pointer) || (device is Keyboard)))
        {
            //InputUser.PerformPairingWithDevice(device, user);
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.KeyboardMouse);
            TrigerControlScheme(ControlScheme.KeyboardMouse);
            Debug.Log("Contro Scheme " + CurrentControlScheme);
            return;
        }

        if (device is Gamepad)
        {
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.Gamepad);
            CurrentControlScheme = ControlScheme.Gamepad;
            TrigerControlScheme(ControlScheme.Gamepad);
            Debug.Log("Contro Scheme " + CurrentControlScheme);
        }
        else if ((device is Keyboard) || (device is Pointer))
        {
            if (OnInputSchemeChanged != null) OnInputSchemeChanged(ControlScheme.KeyboardMouse);
            CurrentControlScheme = ControlScheme.KeyboardMouse;
            TrigerControlScheme(ControlScheme.KeyboardMouse);
            Debug.Log("Contro Scheme " + CurrentControlScheme);
        }
        else return;

        //user.UnpairDevices();
        //InputUser.PerformPairingWithDevice(device, user);
    }
    public void TrigerControlScheme(ControlScheme scheme)
    {
        Debug.Log(scheme.ToString());
        Debug.Log(inputActions.controlSchemes[(int)scheme]);
        Debug.Log((int)scheme);
        //user.ActivateControlScheme(scheme.ToString());
        //user.ActivateControlScheme(inputActions.controlSchemes[(int)scheme]); // this should be faster and not vulnerable to scheme string names
    }
    public enum ControlScheme
    {
        KeyboardMouse = 0, Gamepad = 1 // just need to be same indexes as defined in inputActionAsset
    }
}