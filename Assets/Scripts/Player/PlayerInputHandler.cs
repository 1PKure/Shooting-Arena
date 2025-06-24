using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //private PlayerControls controls;
    private PlayerController playerController;
    private PlayerInput playerInput;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }

    void Awake()
    {
        //controls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += ctx => playerController.SetInput(Vector2.zero);


        /*
        controls.Gameplay.Move.performed += Move();
        controls.Gameplay.Move.canceled += ctx => playerController.SetInput(Vector2.zero);

        controls.Gameplay.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => LookInput = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => JumpPressed = true;
        controls.Gameplay.Jump.canceled += ctx => JumpPressed = false;
        controls.Gameplay.Enable();
        */
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("Se movio");
        playerController.SetInput(ctx.ReadValue<Vector2>());
    }
    //void OnDisable() => controls.Gameplay.Disable();
}

