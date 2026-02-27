using UnityEngine;

public class UIContextMarker : MonoBehaviour
{
    private bool registered;

    private void OnEnable()
    {
        if (registered) return;

        EnsureContextExists();
        UIContext.Instance.PushUI();
        registered = true;
    }

    private void OnDisable()
    {
        if (!registered) return;

        if (UIContext.Instance != null)
            UIContext.Instance.PopUI();

        registered = false;
    }

    private static void EnsureContextExists()
    {
        if (UIContext.Instance != null) return;

        var go = new GameObject("UIContext");
        go.AddComponent<UIContext>();
    }
}