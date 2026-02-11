using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject settingsPanel;

    private bool isPaused = false;
    public bool IsPaused => isPaused;
    private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        audioPanel.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (audioPanel.activeSelf)
                ReturnToPauseMenu();
            else if (settingsPanel.activeSelf)
                ReturnToPauseMenuFromSettings();
            else
                TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (playerController != null)
            playerController.enabled = !isPaused;
    }

    public void OpenAudioPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(false);
        audioPanel.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSettingsPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ReturnToPauseMenuFromSettings()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
