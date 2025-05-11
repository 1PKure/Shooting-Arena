using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Transform weaponHolder; 
    private GameObject currentWeapon; 
    private GameObject weaponInRange; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && weaponInRange != null)
        {
            PickupWeapon();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            weaponInRange = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Salir del rango del arma
        if (other.CompareTag("Weapon"))
        {
            weaponInRange = null;
        }
    }

    void PickupWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.transform.position = weaponInRange.transform.position;
            currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        }

        currentWeapon = weaponInRange;
        currentWeapon.transform.SetParent(weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        weaponInRange = null; 
    }
}