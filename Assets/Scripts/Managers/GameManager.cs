using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Difficulty currentDifficulty;
    [SerializeField] private EnemySpawner spawner;

    [SerializeField] private int score = 0;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    [SerializeField] private float gameTime = 60f;
    private float remainingTime;
    [SerializeField] private TMP_Text timeText;

    public static GameManager Instance;
    private bool isGameOver = false;
    private int victoryScore = 100;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        score = 0;
        remainingTime = gameTime;
        int storedDiff = PlayerPrefs.GetInt("SelectedDifficulty", 1);
        currentDifficulty = (Difficulty)storedDiff;

        spawner.SetDifficulty(currentDifficulty);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        UpdateTimeUI();
        if (remainingTime <= 0)
        {
            GameOver();
        }

        if (score >= victoryScore)
        {
            Victory();
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score.ToString();
        }
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = string.Format("Tiempo: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (finalScoreText != null)
            {
                finalScoreText.text = "Puntuación final: " + score.ToString();
            }
        }

        Cursor.lockState = CursorLockMode.None;
    }

    void Victory()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}