using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    public float range = 100f; // Alcance del arma
    public float damage = 25f; // Daño que inflige el arma
    public Camera playerCamera; // Cámara del jugador para determinar la dirección del disparo
    public ParticleSystem muzzleFlash; // Efecto visual del disparo
    public GameObject impactEffect; // Efecto visual al impactar un objeto

    void Update()
    {
        // Detectar si el jugador presiona el botón de disparo
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Mostrar el efecto de disparo
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Realizar un Raycast desde la cámara del jugador
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // Verificar si el objeto impactado tiene un componente de salud
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Aplicar daño al enemigo
            }

            // Crear un efecto de impacto en el punto de colisión
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}