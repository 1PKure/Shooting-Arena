using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform cameraHolder;

    private CharacterController controller;
    private Animator animator;
    private float xRotation = 0f;
    private int currentHealth;
    private Camera activeCamera;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private MovementLimiter movementLimiter;
    void Start()
    {
        movementLimiter = GetComponent<MovementLimiter>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetPlayer();
        UpdateActiveCamera();
    }

    public void ResetPlayer()
    {
        transform.position = playerData.spawnPosition;
        currentHealth = playerData.maxHealth;
        xRotation = 0f;
    }

    void Update()
    {
        HandleMovement();
        UpdateActiveCamera();
        MouseRotation();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        move *= playerData.moveSpeed * Time.deltaTime;

        if (movementLimiter != null)
            move = movementLimiter.LimitMovement(move);

        controller.Move(move);
    }

    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main;
    }
    private float sensitivityMultiplier;

    private void MouseRotation()
    {
        float mouseXInput = Input.GetAxis("Mouse X") * playerData.mouseSensitivity;
        float mouseYInput = Input.GetAxis("Mouse Y") * playerData.mouseSensitivity;

        mouseX += mouseXInput;
        mouseY -= mouseYInput;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        cameraHolder.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
}
