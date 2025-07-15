using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerController playerControl;
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance!= null &&_instance!= this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControl = new PlayerController();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControl.Player.Movement.ReadValue<Vector2>();
    }
    public bool GetPlayerAttackHead()
    {
        return playerControl.Player.AttackHead.WasPressedThisFrame();
    }
    public bool GetPlayerAttackBody()
    {
        return playerControl.Player.AttackBody.WasPressedThisFrame();
    }
    public void SubPlayerDefOn(Action<InputAction.CallbackContext> action)
    {
        playerControl.Player.Def.performed += action;
    }
    public void UnsubPlayerDefOn(Action<InputAction.CallbackContext> action)
    {
        playerControl.Player.Def.performed -= action;
    }
    public void SubPlayerDefOff(Action<InputAction.CallbackContext> action)
    {
        playerControl.Player.Def.canceled += action;
    }
    public void UnsubPlayerDefOff(Action<InputAction.CallbackContext> action)
    {
        playerControl.Player.Def.canceled -= action;
    }


}
