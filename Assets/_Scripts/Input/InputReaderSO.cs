using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReaderSO : ScriptableObject, PlayerControls.IGameplayActions
{
    private PlayerControls _playerControls;
    
    // Gameplay delegates
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };
    public event UnityAction ActionEvent = delegate { };

    
    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Gameplay.SetCallbacks(this);
        }
        _playerControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Gameplay.Disable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove");
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("OnJump");
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                JumpEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                JumpCanceledEvent.Invoke();
                break;
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        ActionEvent.Invoke();
    }
}
