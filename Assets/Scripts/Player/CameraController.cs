using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera firstPersonVcam;
    //[SerializeField] private CinemachineVirtualCamera thirdPersonVcam;

    [Header("Priorities")]
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 10;

    [Header("Optional: default mode")]
    [SerializeField] private bool startFirstPerson = true;

    [SerializeField] private PlayerController player;

    private bool _isFirstPerson;

    private void Awake()
    {
        _isFirstPerson = startFirstPerson;
        ApplyMode();

    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleMode();
        }
        */
    }

    public void ToggleMode()
    {
        _isFirstPerson = !_isFirstPerson;
        ApplyMode();

    }

    private void ApplyMode()
    {
        /*
        if (firstPersonVcam == null || thirdPersonVcam == null)
        {
            Debug.LogError("CameraController: Virtual Cameras are not assigned.");
            return;
        }
        */

        if (_isFirstPerson)
        {
            firstPersonVcam.Priority = activePriority;
            //thirdPersonVcam.Priority = inactivePriority;
        }
        else
        {
            //firstPersonVcam.Priority = inactivePriority;
            //thirdPersonVcam.Priority = activePriority;
        }

        if (player != null)
            player.SetFirstPerson(_isFirstPerson);

    }
}