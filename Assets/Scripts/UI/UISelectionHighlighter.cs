using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class UISelectionHighlighter : MonoBehaviour
{
    [Header("Frame")]
    [SerializeField] private RectTransform frame;
    [SerializeField] private bool hideWhenNoSelection = true;

    [Header("Mode")]
    [Tooltip("UI open state is detected automatically")]
    [SerializeField] private bool autoDetectUIOpen = true;

    [Tooltip("frame only shows when something is selected.")]
    [SerializeField] private bool requireSelectionToShow = true;

    [Header("UI Context (Recommended)")]
    [Tooltip("If true, the frame will only show when UIContext says UI is active (menus opened).")]
    [SerializeField] private bool requireUIContext = true;

    [Header("Input Rules")]
    [SerializeField] private bool onlyForGamepad = true;
    [SerializeField] private float gamepadGraceTime = 0.25f;

    private bool uiOpenOverrideEnabled;
    private bool uiOpenOverrideValue;

    private float lastGamepadTime;
    private float lastMouseTime;

    /// <summary>
    /// Optional manual override. If you never call this, it will auto-detect.
    /// </summary>
    public void SetUIOpen(bool open)
    {
        uiOpenOverrideEnabled = true;
        uiOpenOverrideValue = open;

        if (!open && frame != null)
            frame.gameObject.SetActive(false);
    }

    /// <summary>
    /// If you want to return to auto mode.
    /// </summary>
    public void ClearUIOverride()
    {
        uiOpenOverrideEnabled = false;
    }

    private void Awake()
    {
        if (frame != null)
            frame.gameObject.SetActive(false);

        // If the project starts in a scene without UIContext, create it.
        if (requireUIContext && UIContext.Instance == null)
        {
            var go = new GameObject("UIContext");
            go.AddComponent<UIContext>();
        }
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        DetectLastInput();
#endif
    }

    private void LateUpdate()
    {
        if (frame == null) return;

        // No EventSystem -> no UI navigation -> hide
        if (EventSystem.current == null)
        {
            frame.gameObject.SetActive(false);
            return;
        }

        // NEW: Block in gameplay unless UI is actually active
        if (requireUIContext && UIContext.Instance != null && !UIContext.Instance.IsUIActive)
        {
            frame.gameObject.SetActive(false);
            return;
        }

        bool uiOpen = GetUIOpenState();

        if (!uiOpen)
        {
            frame.gameObject.SetActive(false);
            return;
        }

        if (onlyForGamepad && !ShouldShowForGamepad())
        {
            frame.gameObject.SetActive(false);
            return;
        }

        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
        {
            if (hideWhenNoSelection) frame.gameObject.SetActive(false);
            return;
        }

        var rt = selected.GetComponent<RectTransform>();
        if (rt == null)
        {
            if (hideWhenNoSelection) frame.gameObject.SetActive(false);
            return;
        }

        frame.gameObject.SetActive(true);
        frame.position = rt.position;
    }

    private bool GetUIOpenState()
    {
        if (uiOpenOverrideEnabled)
            return uiOpenOverrideValue;

        if (!autoDetectUIOpen)
            return true;

        if (requireSelectionToShow)
            return EventSystem.current.currentSelectedGameObject != null;

        return true;
    }

#if ENABLE_INPUT_SYSTEM
    private void DetectLastInput()
    {
        if (Mouse.current != null)
        {
            if (Mouse.current.delta.ReadValue() != Vector2.zero ||
                Mouse.current.leftButton.wasPressedThisFrame ||
                Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame ||
                Mouse.current.scroll.ReadValue() != Vector2.zero)
            {
                lastMouseTime = Time.unscaledTime;
            }
        }

        if (Gamepad.current != null)
        {
            var g = Gamepad.current;

            bool stickMoved =
                g.leftStick.ReadValue().sqrMagnitude > 0.01f ||
                g.rightStick.ReadValue().sqrMagnitude > 0.01f;

            bool dpadMoved = g.dpad.ReadValue().sqrMagnitude > 0.01f;

            bool pressed =
                g.buttonSouth.wasPressedThisFrame ||
                g.buttonNorth.wasPressedThisFrame ||
                g.buttonEast.wasPressedThisFrame ||
                g.buttonWest.wasPressedThisFrame ||
                g.leftShoulder.wasPressedThisFrame ||
                g.rightShoulder.wasPressedThisFrame ||
                g.startButton.wasPressedThisFrame ||
                g.selectButton.wasPressedThisFrame;

            if (stickMoved || dpadMoved || pressed)
                lastGamepadTime = Time.unscaledTime;
        }
    }

    private bool ShouldShowForGamepad()
    {
        bool recentGamepad = (Time.unscaledTime - lastGamepadTime) <= gamepadGraceTime;
        bool mouseMoreRecent = lastMouseTime > lastGamepadTime;
        return recentGamepad && !mouseMoreRecent;
    }
#endif
}