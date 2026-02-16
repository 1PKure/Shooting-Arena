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
    private int killGoal = 200;
    private bool isGodMode = false;
    [SerializeField] private UIManager uiManager;
    private bool victoryTriggered = false;
    private bool isTutorialLevel = false;
    private bool tutorialReadyToAdvance;
    [SerializeField] private GameObject nextLevel;
    [SerializeField] private EndScreenFader endScreenFader;

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
        remainingTime = gameTime;
        score = 0;

        isTutorialLevel = SceneManager.GetActiveScene().name == "Training";

        if (!isTutorialLevel)
        {
            int storedDiff = PlayerPrefs.GetInt("SelectedDifficulty", 1);
            currentDifficulty = (Difficulty)storedDiff;
            if (spawner != null) spawner.SetDifficulty(currentDifficulty);
        }

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        if (!isTutorialLevel)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimeUI();

            if (remainingTime <= 0)
                GameOver();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.T) && !tutorialReadyToAdvance)
            {
                tutorialReadyToAdvance = true;
                if (spawner != null) spawner.StopSpawning();
                if (nextLevel != null) nextLevel.SetActive(true);
            }
        }
    }

    public void AddScore(int points)
    {
        if (isTutorialLevel) return;
        if (isGameOver) return;

        score += points;
        UpdateScoreUI();
        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.ScoreChanged>()?.Invoke(score);
        if (!isTutorialLevel && score >= killGoal)
        {
            Victory();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.TimeChanged>()?.Invoke(remainingTime);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        SetGameEndState(true, gameOverPanel);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score.ToString();

        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.GameOver>()?.Invoke(score);
        FeedbackManager.Instance.PlayLoseFeedback();
    }

    public void RestartGame()
    {
        FullReset();
        SceneManager.LoadScene("MainMenu");
    }
    public void FullReset()
    {
        isGameOver = false;
        victoryTriggered = false;          
        tutorialReadyToAdvance = false;    

        score = 0;
        remainingTime = gameTime;
        Time.timeScale = 1f;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimeUI();

        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = true;         
            player.ResetPlayer();
        }

        foreach (var bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(bullet);

        if (spawner != null) spawner.ResetSpawner();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;            
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

        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.GameSaved>()?.Invoke();
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

        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.GameLoaded>()?.Invoke(data);
    }

    public void DeleteSave() => SaveSystem.DeleteSave();
    public bool IsGodMode() => isGodMode;

    private void HandleVictory()
    {
        if (victoryTriggered || isGameOver) return;

        victoryTriggered = true;
        SetGameEndState(true, victoryPanel);
        var events = Code.Service.ServiceProvider.Instance.GetService<Systems.CentralizeEventSystem.CentralizeEventSystem>();
        events?.Get<GameEvents.Victory>()?.Invoke(score);
        FeedbackManager.Instance.PlayWinFeedback();
    }
    private void SetGameEndState(bool showPanel, GameObject panel)
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (panel != null)
        {
            var fader = panel.GetComponent<EndScreenFader>();

            if (fader != null)
            {
                if (showPanel) fader.Show();
                else fader.HideInstant();
            }
            else
            {
                panel.SetActive(showPanel);
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var player = FindObjectOfType<PlayerController>();
        if (player != null) player.enabled = false;
    }
    public void ForceVictory() => HandleVictory();
    private void Victory() => HandleVictory();

    public bool IsTutorialLevel()
    {
        return isTutorialLevel;
    }

    public bool IsTutorialCompleted()
    {
        return tutorialReadyToAdvance;
    }
}