using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveableVehicle : MonoBehaviour
{
    public Transform vehicleCamera;
    private GameObject player;
    private Transform playerCamera;
    private bool isDriving = false;

    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    void Update()
    {
        if (isDriving)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * h * turnSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitVehicle();
            }
        }
    }

    public void EnterVehicle(GameObject playerObj)
    {
        player = playerObj;
        playerCamera = Camera.main.transform;

        player.SetActive(false);
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);
        if (vehicleCamera != null)
            vehicleCamera.gameObject.SetActive(true);

        isDriving = true;
    }

    public void ExitVehicle()
    {
        if (player != null)
        {
            player.transform.position = transform.position + transform.right * 2f;
            player.SetActive(true);

            Camera.main.gameObject.SetActive(true);
        }

        if (vehicleCamera != null)
            vehicleCamera.gameObject.SetActive(false);

        isDriving = false;
    }
}


