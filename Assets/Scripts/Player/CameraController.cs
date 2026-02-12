using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform firstPersonTarget;
    [SerializeField] private Transform thirdPersonTarget;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float rotateSpeed = 12f;

    [SerializeField] private float firstPersonFov = 75f;
    [SerializeField] private float thirdPersonFov = 65f;

    private Camera _cam;
    private Transform _currentTarget;
    private bool _isFirstPerson = true;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _currentTarget = firstPersonTarget;
        _cam.fieldOfView = firstPersonFov;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleMode();
        }

        if (_currentTarget == null) return;

        transform.position = Vector3.Lerp(transform.position, _currentTarget.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _currentTarget.rotation, rotateSpeed * Time.deltaTime);
    }

    public void ToggleMode()
    {
        _isFirstPerson = !_isFirstPerson;

        _currentTarget = _isFirstPerson ? firstPersonTarget : thirdPersonTarget;
        _cam.fieldOfView = _isFirstPerson ? firstPersonFov : thirdPersonFov;
    }
}