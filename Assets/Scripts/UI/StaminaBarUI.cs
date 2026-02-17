using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Image fillImage;

    [Header("Smoothing")]
    [SerializeField] private bool smooth = true;
    [SerializeField] private float smoothSpeed = 12f;

    private float currentFill = 1f;
    private float targetFill = 1f;

    private void Awake()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();

        if (player != null)
        {
            player.OnStaminaNormalizedChanged += HandleStaminaChanged;
            targetFill = player.StaminaNormalized;
            currentFill = targetFill;
            Apply(currentFill);
        }
    }

    private void OnDisable()
    {
        if (player != null)
            player.OnStaminaNormalizedChanged -= HandleStaminaChanged;
    }

    private void Update()
    {
        if (!smooth || fillImage == null) return;

        currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * smoothSpeed);
        Apply(currentFill);
    }

    private void HandleStaminaChanged(float normalized)
    {
        targetFill = Mathf.Clamp01(normalized);

        if (!smooth)
        {
            currentFill = targetFill;
            Apply(currentFill);
        }
    }

    private void Apply(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }
}