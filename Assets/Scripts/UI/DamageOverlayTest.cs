using UnityEngine;

public class DamageOverlayTest : MonoBehaviour
{
    [SerializeField] private DamageOverlayUI overlay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            overlay.PlayHit();
    }
}