using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPopup;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OnDifficultySelected(int difficulty)
    {
        PlayerPrefs.SetInt("SelectedDifficulty", difficulty);
        tutorialPopup.SetActive(true);
    }

    public void OnTutorialResponse(bool doTutorial)
    {
        tutorialPopup.SetActive(false);
        if (doTutorial)
            SceneManager.LoadScene("Training");
        else
            SceneManager.LoadScene("Gameplay");
    }


    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
