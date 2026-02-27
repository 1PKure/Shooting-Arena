using UnityEngine;

public class UIContext : MonoBehaviour
{
    public static UIContext Instance { get; private set; }

    private int uiOpenCount;

    public bool IsUIActive => uiOpenCount > 0;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PushUI()
    {
        uiOpenCount++;
        if (uiOpenCount < 0) uiOpenCount = 0;
    }

    public void PopUI()
    {
        uiOpenCount--;
        if (uiOpenCount < 0) uiOpenCount = 0;
    }
}