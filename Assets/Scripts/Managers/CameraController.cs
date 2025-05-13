using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;

    [Header("Referencia de cámara")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform thirdPersonFollowPoint;

    [Header("Configuración")]
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float verticalLimit = 80f;
    [SerializeField] private float smoothSpeed = 10f;

    private float xRotation = 0f;
    private bool isFirstPerson = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetCameraMode(true);
    }

    void Update()
    {
        HandleCameraSwitch();
        HandleRotation();
        if (!isFirstPerson) FollowThirdPerson();
    }

    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            SetCameraMode(isFirstPerson);
        }
    }

    void HandleRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLimit, verticalLimit);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void FollowThirdPerson()
    {
        if (thirdPersonCamera != null && thirdPersonFollowPoint != null)
        {
            Vector3 targetPos = thirdPersonFollowPoint.position;
            thirdPersonCamera.transform.position = Vector3.Lerp(thirdPersonCamera.transform.position, targetPos, Time.deltaTime * smoothSpeed);
            thirdPersonCamera.transform.LookAt(thirdPersonFollowPoint.parent.position + Vector3.up * 1.5f); 
        }
    }

    void SetCameraMode(bool firstPerson)
    {
        firstPersonCamera.gameObject.SetActive(firstPerson);
        thirdPersonCamera.gameObject.SetActive(!firstPerson);
    }
}
