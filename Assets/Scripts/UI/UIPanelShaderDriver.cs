using UnityEngine;

public class UIPanelShaderDriver : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private float pulseOnShow = 0.35f;
    [SerializeField] private float pulseHidden = 0.0f;

    private Material _mat;

    private void Awake()
    {
        if (image == null) image = GetComponent<UnityEngine.UI.Image>();
        if (image != null) _mat = image.material;
    }

    private void OnEnable()
    {
        if (_mat != null) _mat.SetFloat("_PulseAmount", pulseOnShow);
    }

    private void OnDisable()
    {
        if (_mat != null) _mat.SetFloat("_PulseAmount", pulseHidden);
    }
}