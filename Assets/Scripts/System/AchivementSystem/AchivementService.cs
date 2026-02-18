using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementService
{
    public event Action<AchievementDefinition> OnAchievementUnlocked;

    private readonly Dictionary<string, AchievementDefinition> _defsById;
    private readonly Dictionary<string, AchievementState> _stateById;

    public AchievementService(IEnumerable<AchievementDefinition> definitions, IEnumerable<AchievementState> savedStates = null)
    {
        _defsById = new Dictionary<string, AchievementDefinition>();
        _stateById = new Dictionary<string, AchievementState>();

        foreach (var def in definitions)
        {
            if (def == null || string.IsNullOrWhiteSpace(def.id)) continue;
            _defsById[def.id] = def;

            _stateById[def.id] = new AchievementState(def.id);
        }

        if (savedStates != null)
        {
            foreach (var s in savedStates)
            {
                if (s == null || string.IsNullOrWhiteSpace(s.id)) continue;
                if (_stateById.ContainsKey(s.id))
                    _stateById[s.id] = s;
            }
        }
    }

    public IReadOnlyCollection<AchievementState> GetAllStates() => _stateById.Values;

    public bool IsUnlocked(string id) =>
        _stateById.TryGetValue(id, out var st) && st.unlocked;

    public void AddProgress(string id, int amount = 1)
    {
        if (!_defsById.TryGetValue(id, out var def)) return;
        if (!_stateById.TryGetValue(id, out var st)) return;
        if (st.unlocked) return;

        st.currentValue = Mathf.Clamp(st.currentValue + amount, 0, def.targetValue);

        if (st.currentValue >= def.targetValue)
        {
            st.unlocked = true;
            st.unlockedUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            OnAchievementUnlocked?.Invoke(def);
            Debug.Log($"[AchievementService] Unlocked: {id}");
        }
    }

    public void SetUnlocked(string id)
    {
        if (!_defsById.TryGetValue(id, out var def)) return;
        if (!_stateById.TryGetValue(id, out var st)) return;
        if (st.unlocked) return;

        st.currentValue = def.targetValue;
        st.unlocked = true;
        st.unlockedUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        OnAchievementUnlocked?.Invoke(def);
        Debug.Log($"[AchievementService] Unlocked: {id}");
    }
}