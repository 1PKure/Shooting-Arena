using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform cameraHolder;

    private CharacterController controller;
    private PlayerInputHandler input;
    private Animator animator;
    private float xRotation = 0f;
    private int currentHealth;
    private Camera activeCamera;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 moveInput;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        input = GetComponent<PlayerInputHandler>();
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Move();
    }

    private void Move()
    {
        
        Debug.Log("Move Input: " + moveInput);
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (input.JumpPressed && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = move * playerData.moveSpeed + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }

    public void SetInput(Vector2 move)
    {
        moveInput = move;
    }

    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main;
    }
    private float sensitivityMultiplier;

    private void MouseRotation()
    {
        Vector2 lookInput = input.LookInput;
        lookInput *= playerData.mouseSensitivity * Time.deltaTime;
        mouseX += lookInput.x;
        mouseY -= lookInput.y;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        cameraHolder.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
}
