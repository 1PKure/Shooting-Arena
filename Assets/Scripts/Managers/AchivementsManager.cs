using System.Collections.Generic;
using UnityEngine;
using Code.Service;
using Systems.CentralizeEventSystem;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField] private AchievementDatabase database;

    [Header("UI")]
    [SerializeField] private AchievementsPopupUI popupUI;
    private CentralizeEventSystem _events;
    private AchievementService _service;
    private bool _testUnlocked;

    private void Awake()
    {
        _events = ServiceProvider.Instance.GetService<CentralizeEventSystem>();

        List<AchievementState> savedStates = AchievementsStorage.LoadOrEmpty();
        _service = new AchievementService(database.definitions, savedStates);
        _service.OnAchievementUnlocked += OnUnlocked;

        if (popupUI != null)
            popupUI.Bind(_service);

        _events.AddListener<GameEvents.ScoreChanged>(OnScoreChanged);
        _events.AddListener<GameEvents.Victory>(OnVictory);
        _events.AddListener<GameEvents.GameLoaded>(OnGameLoaded);
    }
    private void Update()
    {
        if (_testUnlocked) return;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _testUnlocked = true;

            _service.SetUnlocked("test_y");
            AchievementsStorage.Save(_service.GetAllStates());

            Debug.Log("[AchievementsManager] Test achievement unlocked: test_y");
        }
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