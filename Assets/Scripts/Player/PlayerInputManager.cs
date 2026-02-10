using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponSystem weaponSystem;

    void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;
        if (inputReader == null || playerController == null) return;

        playerController.ResetJumpIfGrounded();
        playerController.Move(inputReader.MoveInput, inputReader.SprintHeld);
        playerController.Rotate(inputReader.LookInput);

        if (inputReader.JumpPressed)
            playerController.Jump();

        if (weaponSystem == null) return;

        bool shouldShoot = inputReader.IsShooting(weaponSystem.IsCurrentWeaponAutomatic());
        if (shouldShoot)
            weaponSystem.TryShoot();

        if (inputReader.ReloadPressed)
            weaponSystem.Reload();

        if (inputReader.SwitchWeaponPressed)
            weaponSystem.SwitchWeapon();

        if (inputReader.WeaponKeys[0]) weaponSystem.EquipWeapon(0);
        if (inputReader.WeaponKeys[1]) weaponSystem.EquipWeapon(1);
    }

}

