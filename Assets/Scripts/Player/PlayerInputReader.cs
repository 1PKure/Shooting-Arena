using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool ShootPressed { get; private set; }
    public bool ReloadPressed { get; private set; }

    public bool SwitchWeaponPressed { get; private set; }
    public bool[] WeaponKeys { get; private set; } = new bool[2];

    void Update()
    {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        LookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        JumpPressed = Input.GetButtonDown("Jump");
        ShootPressed = Input.GetButton("Fire1");
        ReloadPressed = Input.GetKeyDown(KeyCode.R);

        SwitchWeaponPressed = Input.GetKeyDown(KeyCode.Q);
        WeaponKeys[0] = Input.GetKeyDown(KeyCode.Alpha1);
        WeaponKeys[1] = Input.GetKeyDown(KeyCode.Alpha2);
    }

    public bool IsShooting(bool isAutomatic)
    {
        return isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
    }
}

