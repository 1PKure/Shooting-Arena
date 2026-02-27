using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CrosshairUI : MonoBehaviour
{
    private CanvasGroup _cg;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        Show(true);
    }

    public void Show(bool visible)
    {
        _cg.alpha = visible ? 1f : 0f;
        _cg.blocksRaycasts = false;   
        _cg.interactable = false;
    }
}