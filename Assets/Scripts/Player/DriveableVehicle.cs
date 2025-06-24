using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveableVehicle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private bool isDriving = false;
    [SerializeField] private Transform player;

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

    public void EnterVehicle(Transform playerTransform)
    {
        player = playerTransform;
        player.gameObject.SetActive(false);
        isDriving = true;
    }

    void ExitVehicle()
    {
        player.position = transform.position + transform.right * 2f;
        player.gameObject.SetActive(true);
        isDriving = false;
    }
}

