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

        playerController.ResetJumpIfGrounded();
        playerController.Move(inputReader.MoveInput);
        playerController.Rotate(inputReader.LookInput);

        if (inputReader.JumpPressed)
            playerController.Jump();

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

