using System.Collections.Generic;
using UnityEngine;
using Code.Service;
using Systems.CentralizeEventSystem;

public class AchievementsManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private AchievementDatabase database;

    [Header("UI")]
    [SerializeField] private AchievementsPopupUI popupUI;

    private CentralizeEventSystem _events;
    private AchievementService _service;

    private void Awake()
    {
        _events = ServiceProvider.Instance.GetService<CentralizeEventSystem>();

        List<AchievementState> savedStates = AchievementsStorage.LoadOrEmpty();
        _service = new AchievementService(database.definitions, savedStates);

        _service.OnAchievementUnlocked += OnUnlocked;

        if (popupUI != null)
            popupUI.Bind(_service);
        else
            Debug.LogWarning("[AchievementsManager] Popup UI reference is NULL. No popup will be shown.");

        _events.AddListener<GameEvents.ScoreChanged>(OnScoreChanged);
        _events.AddListener<GameEvents.Victory>(OnVictory);
        _events.AddListener<GameEvents.GameLoaded>(OnGameLoaded);
        Debug.Log("[AchievementsManager] Service type: " + _service.GetType().FullName);
        Debug.Log($"[AchievementsManager] Awake. instance={GetInstanceID()} popupUI={(popupUI ? popupUI.name : "NULL")}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            _service.SetUnlocked("test_y");
            Debug.Log("[AchievementsManager] Test achievement unlocked: test_y");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            AchievementsStorage.Delete();
            RecreateService();
            Debug.Log("[AchievementsManager] Achievements save deleted + service reset.");
        }
    }
    private void RecreateService()
    {
        var savedStates = AchievementsStorage.LoadOrEmpty();
        _service = new AchievementService(database.definitions, savedStates);

        _service.OnAchievementUnlocked += OnUnlocked;

        if (popupUI != null)
            popupUI.Bind(_service);

        Debug.Log("[AchievementsManager] Service recreated.");
    }
    private void OnDestroy()
    {
        if (_events != null)
        {
            _events.RemoveListener<GameEvents.ScoreChanged>(OnScoreChanged);
            _events.RemoveListener<GameEvents.Victory>(OnVictory);
            _events.RemoveListener<GameEvents.GameLoaded>(OnGameLoaded);
        }

        if (_service != null)
            _service.OnAchievementUnlocked -= OnUnlocked;
    }

    private void OnScoreChanged(int newScore)
    {
        _service.AddProgress("kill_5", 1);
        _service.AddProgress("kill_50", 1);

        AchievementsStorage.Save(_service.GetAllStates());
    }

    private void OnVictory(int finalScore)
    {
        _service.SetUnlocked("first_win");
        AchievementsStorage.Save(_service.GetAllStates());
    }

    private void OnGameLoaded(GameData data)
    {
       
    }

    private void OnUnlocked(AchievementDefinition def)
    {
        _events?.Get<GameEvents.AchievementUnlocked>()?.Invoke(def.id);

        AchievementsStorage.Save(_service.GetAllStates());
    }
}