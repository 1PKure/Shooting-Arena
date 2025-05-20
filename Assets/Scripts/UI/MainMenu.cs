using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGameEasy() => StartGameWithDifficulty(GameManager.Difficulty.Easy);
    public void StartGameNormal() => StartGameWithDifficulty(GameManager.Difficulty.Medium);
    public void StartGameHard() => StartGameWithDifficulty(GameManager.Difficulty.Hard);

    void StartGameWithDifficulty(GameManager.Difficulty diff)
    {
        PlayerPrefs.SetInt("SelectedDifficulty", (int)diff);
        SceneManager.LoadScene("Gameplay");
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
