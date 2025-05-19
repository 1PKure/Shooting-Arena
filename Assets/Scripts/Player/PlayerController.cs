using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform cameraHolder;

    private CharacterController controller;
    private Animator animator;
    private float xRotation = 0f;
    private int currentHealth;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetPlayer();
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
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerData.moveSpeed * Time.deltaTime);
    }
}
