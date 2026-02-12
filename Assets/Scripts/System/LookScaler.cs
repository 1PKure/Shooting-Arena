using UnityEngine;
using UnityEngine.InputSystem;

public class LookScaler : MonoBehaviour
{
    [Header("Sensitivities")]
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float gamepadSensitivity = 120f;

    public Vector2 ScaleLook(Vector2 rawLook)
    {
        bool usingGamepad = Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame;

        if (usingGamepad)
            return rawLook * gamepadSensitivity * Time.deltaTime;

        return rawLook * mouseSensitivity;
    }
}