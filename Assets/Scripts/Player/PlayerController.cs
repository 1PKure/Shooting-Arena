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
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private PlayerInputReader input;


    void Start()
    {
        input = GetComponent<PlayerInputReader>();
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
        if (input == null) return;
        HandleMovement();
        UpdateActiveCamera();
        MouseRotation();
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        Vector2 moveInput = input.MoveInput;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= playerData.moveSpeed * Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        controller.Move(velocity * Time.deltaTime);
        controller.Move(move);
    }

    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main;
    }

    private float sensitivityMultiplier;

    private void MouseRotation()
    {
        Vector2 lookInput = input.LookInput;

        float mouseXInput = lookInput.x * playerData.mouseSensitivity;
        float mouseYInput = lookInput.y * playerData.mouseSensitivity;

        mouseX += mouseXInput;
        mouseY -= mouseYInput;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
}
