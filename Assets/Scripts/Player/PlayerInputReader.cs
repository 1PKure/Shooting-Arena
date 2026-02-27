using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool ShootPressedThisFrame { get; private set; }
    public bool ShootHeld { get; private set; }
    public bool ReloadPressed { get; private set; }

    public bool IsLookFromGamepad { get; private set; }
    public bool PausePressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool SwitchWeaponPressed { get; private set; }
    public bool[] WeaponKeys { get; private set; } = new bool[2];
    public bool InteractPressed { get; private set; }
    public bool ToggleCameraPressed { get; private set; }
    public int ToggleCameraCount { get; private set; }
    public bool AimHeld { get; private set; }
    private float lastToggleCameraTime;
    private const float ToggleCameraCooldown = 0.20f;
    private PlayerControls controls;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Pause.performed += _ => PausePressed = true;
        controls.Gameplay.Aim.performed += _ => AimHeld = true;
        controls.Gameplay.Aim.canceled += _ => AimHeld = false;
        controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += _ => MoveInput = Vector2.zero;
        controls.Gameplay.Look.performed += ctx =>
        {
            LookInput = ctx.ReadValue<Vector2>();
            IsLookFromGamepad = ctx.control.device is Gamepad;
        };
        controls.Gameplay.Look.canceled += _ => LookInput = Vector2.zero;

        controls.Gameplay.Jump.performed += _ => JumpPressed = true;

        controls.Gameplay.Fire.performed += _ =>
        {
            ShootHeld = true;
            ShootPressedThisFrame = true;
        };
        controls.Gameplay.Fire.canceled += _ => ShootHeld = false;


        controls.Gameplay.Reload.performed += _ => ReloadPressed = true;

        controls.Gameplay.Sprint.performed += _ => SprintHeld = true;
        controls.Gameplay.Sprint.canceled += _ => SprintHeld = false;

        controls.Gameplay.SwitchWeapon.performed += _ => SwitchWeaponPressed = true;
        controls.Gameplay.Interact.performed += _ => InteractPressed = true;

        /*
        controls.Gameplay.ToggleCamera.performed += _ =>
        {
            if (Time.unscaledTime - lastToggleCameraTime < ToggleCameraCooldown) return;
            lastToggleCameraTime = Time.unscaledTime;
            ToggleCameraCount++;
        }
        */



    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void LateUpdate()
    {
        JumpPressed = false;
        ReloadPressed = false;
        SwitchWeaponPressed = false;
        InteractPressed = false;
        PausePressed = false;
        ToggleCameraPressed = false;
        ShootPressedThisFrame = false;

        WeaponKeys[0] = false;
        WeaponKeys[1] = false;
    }

    public bool IsShooting(bool isAutomatic)
    {
        return isAutomatic ? ShootHeld : ShootPressedThisFrame;
    }
}