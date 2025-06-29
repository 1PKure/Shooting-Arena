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
    [SerializeField] private TMP_Text killText;
    private int killCount = 0;
    private int killGoal = 200;
    private bool isGodMode = false;
    [SerializeField] private UIManager uiManager;
    //[SerializeField] private GameObject bonusPickupPrefab;
    //[SerializeField] private Transform bonusSpawnPoint;
    private bool victoryTriggered = false;


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

        if (score >= killGoal)
        {
            Victory();
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        killCount++;
        if (killText != null)
            killText.text = $"Enemigos derrotados: {killCount} / {killGoal}";

        UpdateScoreUI();

        if (killCount >= killGoal)
        {
            Victory();
        }
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
        FullReset();
        SceneManager.LoadScene("MainMenu");
    }
    public void FullReset()
    {
        isGameOver = false;
        score = 0;
        remainingTime = gameTime;
        Time.timeScale = 1f;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateScoreUI();
        UpdateTimeUI();

        var player = FindObjectOfType<PlayerController>();
        if (player != null) player.ResetPlayer();

        foreach (var bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(bullet);

        if (spawner != null) spawner.ResetSpawner();

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SaveGame()
    {
        var player = FindObjectOfType<PlayerController>();

        GameData data = new GameData
        {
            score = score,
            remainingTime = remainingTime,
            difficulty = (int)currentDifficulty,
            equippedWeapon = PlayerPrefs.GetString("EquippedWeapon", "DefaultGun"),
            playerPosition = new SerializableVector3(player.transform.position)
        };

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.Load();
        if (data == null) return;

        score = data.score;
        remainingTime = data.remainingTime;
        currentDifficulty = (Difficulty)data.difficulty;
        PlayerPrefs.SetString("EquippedWeapon", data.equippedWeapon);
        FindObjectOfType<PlayerController>().transform.position = data.playerPosition.ToVector3();

        UpdateScoreUI();
        UpdateTimeUI();
    }

    public void DeleteSave() => SaveSystem.DeleteSave();
    public void SetGodMode(bool active)
    {
        isGodMode = active;
    }
    public void ForceVictory()
    {
        if (!victoryTriggered)
        {
            victoryTriggered = true;
            TriggerVictory();
        }
    }
    private void TriggerVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        //if (bonusPickupPrefab != null && bonusSpawnPoint != null)
            //Instantiate(bonusPickupPrefab, bonusSpawnPoint.position, Quaternion.identity);
    }
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}