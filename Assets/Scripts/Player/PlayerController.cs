using System.Threading;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform cameraHolder;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Jumps")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int godModeMaxJumps = 10;
    [SerializeField] private float normalJumpHeight = 2f;
    [SerializeField] private float godJumpHeight = 5f;

    [Header("God Mode")]
    [SerializeField] private float godSpeedMultiplier = 2f;


    [Header("Look Limits")]
    [SerializeField] private float firstPersonMinPitch = -80f;
    [SerializeField] private float firstPersonMaxPitch = 80f;
    [SerializeField] private float thirdPersonMinPitch = -25f;
    [SerializeField] private float thirdPersonMaxPitch = 60f;

    [Header("Look Sensitivity")]
    [SerializeField] private float thirdPersonSensitivityMultiplier = 0.6f;

    private bool isFirstPerson = true;
    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;
    }
    private CharacterController controller;
    private Animator animator;

    private float xRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;

    private Vector3 velocity;
    private bool isGrounded;
    private int currentJumps = 0;

    private float stamina;
    private float lastSprintTime;
    private bool isSprinting;

    private Camera activeCamera;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int VelocityYHash = Animator.StringToHash("VelocityY");




    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetSpawnPointByScene();
        ResetPlayer();
        UpdateActiveCamera();

        stamina = Mathf.Max(0f, playerData.maxStamina);
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
    public void Move(Vector2 moveInput, bool sprintHeld)
    {
        UpdateGrounded();

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float inputMagnitude = Mathf.Clamp01(moveInput.magnitude);

        float speed = playerData.moveSpeed;
        if (GameManager.Instance.IsGodMode())
            speed *= godSpeedMultiplier;

        bool wantsSprint = sprintHeld && inputMagnitude > 0.01f;
        isSprinting = wantsSprint && CanSprint();

        if (isSprinting)
        {
            speed *= playerData.sprintMultiplier;
            DrainStamina();
            lastSprintTime = Time.time;
        }
        else
        {
            RegenStamina();
        }

        move *= speed;

        velocity.y += gravity * Time.deltaTime;

        Vector3 horizontalMovement = move * Time.deltaTime;
        Vector3 verticalMovement = new Vector3(0f, velocity.y, 0f) * Time.deltaTime;
        controller.Move(horizontalMovement + verticalMovement);

        UpdateAnimator(inputMagnitude, speed);
    }
    private void UpdateGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            currentJumps = 0;
        }
    }
    public void Jump()
    {
        int jumpLimit = GameManager.Instance.IsGodMode() ? godModeMaxJumps : maxJumps;
        float jumpForce = GameManager.Instance.IsGodMode() ? godJumpHeight : normalJumpHeight;

        if (currentJumps < jumpLimit)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            currentJumps++;

            if (FeedbackManager.Instance != null)
                FeedbackManager.Instance.PlayJumpFeedback(transform.position);

            if (animator != null)
                animator.SetFloat(VelocityYHash, velocity.y);
        }
    }


    public void ResetJumpIfGrounded()
    {
        UpdateGrounded();
        if (isGrounded)
            currentJumps = 0;
    }

    public void Rotate(Vector2 lookInput, bool isGamepadLook)
    {
        float x;
        float y;

        if (isGamepadLook)
        {
            x = lookInput.x * playerData.gamepadLookSensitivity * Time.deltaTime;
            y = lookInput.y * playerData.gamepadLookSensitivity * Time.deltaTime;
        }
        else
        {
            float sens = playerData.mouseSensitivity;
            if (!isFirstPerson) sens *= thirdPersonSensitivityMultiplier;

            x = lookInput.x * sens;
            y = lookInput.y * sens;
        }

        mouseX += x;
        mouseY -= y;

        float minPitch = isFirstPerson ? firstPersonMinPitch : thirdPersonMinPitch;
        float maxPitch = isFirstPerson ? firstPersonMaxPitch : thirdPersonMaxPitch;
        mouseY = Mathf.Clamp(mouseY, minPitch, maxPitch);

        cameraHolder.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
    public void ResetPlayer()
    {
        transform.position = playerData.spawnPosition;

        mouseX = transform.eulerAngles.y;

        float pitch = cameraHolder.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
        mouseY = pitch;

        velocity = Vector3.zero;
        currentJumps = 0;

        stamina = Mathf.Max(0f, playerData.maxStamina);
        isSprinting = false;

        cameraHolder.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }
    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main;
    }
    private bool CanSprint()
    {
        if (GameManager.Instance.IsGodMode())
            return true;

        if (playerData.maxStamina <= 0f)
            return false;

        float minToStart = playerData.maxStamina * playerData.minStaminaToStartSprint;
        return stamina > minToStart;
    }

    private void DrainStamina()
    {
        if (GameManager.Instance.IsGodMode())
            return;

        stamina -= playerData.staminaDrainPerSecond * Time.deltaTime;
        if (stamina < 0f) stamina = 0f;
    }

    private void RegenStamina()
    {
        if (GameManager.Instance.IsGodMode())
            return;

        if (Time.time - lastSprintTime < playerData.staminaRegenDelay)
            return;

        stamina += playerData.staminaRegenPerSecond * Time.deltaTime;
        if (stamina > playerData.maxStamina) stamina = playerData.maxStamina;
    }

    private void UpdateAnimator(float inputMagnitude, float currentSpeed)
    {
        if (animator == null)
            return;

        animator.SetBool(GroundedHash, isGrounded);
        animator.SetFloat(VelocityYHash, velocity.y);

        float baseSpeed = Mathf.Max(0.01f, playerData.baseSpeed);
        float normalized = (currentSpeed / baseSpeed) * inputMagnitude;
        animator.SetFloat(SpeedHash, normalized);
    }

    public float StaminaNormalized
    {
        get
        {
            if (playerData == null || playerData.maxStamina <= 0f) return 1f;
            return Mathf.Clamp01(stamina / playerData.maxStamina);
        }
    }

    public bool IsSprinting => isSprinting;
}
