using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    private bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject audioPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (audioPanel.activeSelf)
            {
                audioPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            else if (pausePanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        audioPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OpenAudioPanel()
    {
        pausePanel.SetActive(false);
        audioPanel.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        audioPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

}