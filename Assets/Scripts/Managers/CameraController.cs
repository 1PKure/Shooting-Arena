using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;

    [Header("Referencia de cámara")]
    [SerializeField] private Transform thirdPersonFollowPoint;

    [Header("Configuración")]
    [SerializeField] private float smoothSpeed = 10f;

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
