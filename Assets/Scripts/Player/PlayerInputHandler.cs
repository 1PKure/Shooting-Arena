using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => MoveInput = Vector2.zero;

        controls.Gameplay.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => LookInput = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => JumpPressed = true;
        controls.Gameplay.Jump.canceled += ctx => JumpPressed = false;
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();
}

