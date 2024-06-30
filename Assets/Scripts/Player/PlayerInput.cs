using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputAction _playerInputAction;
    private InputAction _move;
    private InputAction _jump;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        
        _move = _playerInputAction.Player.Move;
        _jump = _playerInputAction.Player.Jump;
    }

    private void OnEnable()
    {
        _playerInputAction.Enable(); 
    }

    private void Update()
    {
        FrameInput = GatherInput(); 
    }

    private void OnDisable()
    {
        _playerInputAction.Disable();
    }
    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame()
        };
    }
}

public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}