using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string gameplayActionMap = "Gameplay";
    [SerializeField] private string uiActionMap = "UI";
    [SerializeField] private string pauseActionName = "Pause";

    [Header("UI Selection (optional)")]
    [SerializeField] private UISelectionHighlighter selectionHighlighter;
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject audioFirstSelected;
    [SerializeField] private GameObject settingsFirstSelected;

    [SerializeField] private CrosshairUI crosshair;
    private bool isPaused;
    public bool IsPaused => isPaused;

    private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        audioPanel.SetActive(false);

        playerController = FindObjectOfType<PlayerController>();

        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();

        BindPauseAction();
    }

    private void OnEnable()
    {
        BindPauseAction();
    }

    private void OnDisable()
    {
        UnbindPauseAction();
    }

    private void BindPauseAction()
    {
        if (playerInput == null) return;

        var pauseAction = playerInput.actions.FindAction(pauseActionName, true);
        pauseAction.performed -= OnPausePerformed;
        pauseAction.performed += OnPausePerformed;
    }

    private void UnbindPauseAction()
    {
        if (playerInput == null) return;

        var pauseAction = playerInput.actions.FindAction(pauseActionName, true);
        pauseAction.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
        {
            if (audioPanel.activeSelf) ReturnToPauseMenu();
            else if (settingsPanel.activeSelf) ReturnToPauseMenuFromSettings();
            else ClosePause();
        }
        else
        {
            OpenPause();
        }
    }

    private void OpenPause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (crosshair != null) crosshair.Show(false);
        pausePanel.SetActive(true);
        audioPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(uiActionMap);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = false;

        if (selectionHighlighter != null)
            selectionHighlighter.SetUIOpen(true);

        SetSelected(pauseFirstSelected);
    }

    private void ClosePause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        audioPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if (crosshair != null) crosshair.Show(true);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        if (selectionHighlighter != null)
            selectionHighlighter.SetUIOpen(false);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(gameplayActionMap);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
            playerController.enabled = true;
    }

    public void OpenAudioPanel()
    {
        pausePanel.SetActive(false);
        audioPanel.SetActive(true);
        settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetSelected(audioFirstSelected);
    }

    public void ReturnToPauseMenu()
    {
        audioPanel.SetActive(false);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetSelected(pauseFirstSelected);
    }

    public void OpenSettingsPanel()
    {
        pausePanel.SetActive(false);
        audioPanel.SetActive(false);
        settingsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetSelected(settingsFirstSelected);
    }

    public void ReturnToPauseMenuFromSettings()
    {
        settingsPanel.SetActive(false);
        audioPanel.SetActive(false);
        pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetSelected(pauseFirstSelected);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetSelected(GameObject go)
    {
        if (EventSystem.current == null || go == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(go);
    }

    public void Resume()
    {
        ClosePause();
    }
}