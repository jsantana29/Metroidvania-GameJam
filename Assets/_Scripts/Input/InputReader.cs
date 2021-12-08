using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerControls.IGameplayActions
{
    private PlayerControls playerControls;
    
    // Gameplay delegates
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };
    public event UnityAction DashEvent = delegate { };
    
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.SetCallbacks(this);
        }
        playerControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
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

    public void OnDash(InputAction.CallbackContext context)
    {
        DashEvent?.Invoke();
    }
}
