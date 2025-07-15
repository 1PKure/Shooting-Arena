using System.Threading;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;
    private float xRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private float godSpeedMultiplier = 2f;
    private Camera activeCamera;
    private int currentJumps = 0;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int godModeMaxJumps = 10;

    [SerializeField] private float normalJumpHeight = 2f;
    [SerializeField] private float godJumpHeight = 5f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetSpawnPointByScene();
        ResetPlayer();
        UpdateActiveCamera();
    }

    void Update()
    {
        UpdateActiveCamera();

    }

    void SetSpawnPointByScene()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (sceneName == "Training")
            playerData.spawnPosition = new Vector3(-63f, 25f, -132f);
        else
            playerData.spawnPosition = new Vector3(0f, 1.2f, -5f);
    }
    public void Move(Vector2 moveInput)
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            currentJumps = 0; 
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        float speed = playerData.moveSpeed;
        if (GameManager.Instance.IsGodMode())
            speed *= godSpeedMultiplier;

        move *= speed;

        velocity.y += gravity * Time.deltaTime;

        Vector3 horizontalMovement = move * Time.deltaTime;
        Vector3 verticalMovement = new Vector3(0f, velocity.y, 0f) * Time.deltaTime;

        controller.Move(horizontalMovement + verticalMovement);
    }

    public void Jump()
    {
        int jumpLimit = GameManager.Instance.IsGodMode() ? godModeMaxJumps : maxJumps;
        float jumpForce = GameManager.Instance.IsGodMode() ? godJumpHeight : normalJumpHeight;

        if (currentJumps < jumpLimit)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            currentJumps++;
            FeedbackManager.Instance.PlayJumpFeedback(transform.position);
        }

    }

    public void ResetJumpIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
            currentJumps = 0;
    }

    public void Rotate(Vector2 lookInput)
    {
        float mouseXInput = lookInput.x * playerData.mouseSensitivity;
        float mouseYInput = lookInput.y * playerData.mouseSensitivity;

        mouseX += mouseXInput;
        mouseY -= mouseYInput;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
    public void ResetPlayer()
    {
        transform.position = playerData.spawnPosition;
        xRotation = 0f;
    }
    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main;
    }
}
