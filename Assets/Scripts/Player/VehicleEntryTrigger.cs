using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleEntryTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            DriveableVehicle vehicle = GetComponentInParent<DriveableVehicle>();
            if (vehicle != null)
            {
                vehicle.EnterVehicle(other.transform);
            }
        }
    }
}

