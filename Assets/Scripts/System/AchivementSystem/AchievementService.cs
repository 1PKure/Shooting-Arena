using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementService
{
    public event Action<AchievementDefinition> OnAchievementUnlocked;

    private readonly Dictionary<string, AchievementDefinition> _defsById = new Dictionary<string, AchievementDefinition>();
    private readonly Dictionary<string, AchievementState> _stateById = new Dictionary<string, AchievementState>();

    public AchievementService(List<AchievementDefinition> definitions, List<AchievementState> savedStates)
    {
        if (definitions == null) throw new ArgumentNullException(nameof(definitions));

        _defsById.Clear();
        for (int i = 0; i < definitions.Count; i++)
        {
            var def = definitions[i];
            if (def == null) continue;

            if (string.IsNullOrWhiteSpace(def.id))
            {
                Debug.LogWarning("[AchievementService] Found definition with empty id. Skipping.");
                continue;
            }

            if (_defsById.ContainsKey(def.id))
            {
                Debug.LogWarning($"[AchievementService] Duplicate definition id: {def.id}. Keeping first.");
                continue;
            }

            _defsById.Add(def.id, def);
        }

        _stateById.Clear();
        if (savedStates != null)
        {
            for (int i = 0; i < savedStates.Count; i++)
            {
                var st = savedStates[i];
                if (st == null) continue;

                if (string.IsNullOrWhiteSpace(st.id))
                    continue;

                if (_stateById.ContainsKey(st.id))
                    continue;

                _stateById.Add(st.id, st);
            }
        }

        foreach (var kvp in _defsById)
        {
            string id = kvp.Key;
            var def = kvp.Value;

            if (!_stateById.TryGetValue(id, out var st))
            {
                st = new AchievementState
                {
                    id = id,
                    currentValue = 0,
                    unlocked = false,
                    unlockedUnixTime = 0
                };
                _stateById.Add(id, st);
            }

            int target = Mathf.Max(1, def.targetValue);
            st.currentValue = Mathf.Clamp(st.currentValue, 0, target);

            if (st.unlocked)
                st.currentValue = Mathf.Max(st.currentValue, target);
        }
    }

    public void AddProgress(string id, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        if (!_defsById.TryGetValue(id, out var def)) return;
        if (!_stateById.TryGetValue(id, out var st)) return;
        if (st.unlocked) return;

        int target = Mathf.Max(1, def.targetValue);
        st.currentValue = Mathf.Clamp(st.currentValue + Mathf.Max(0, amount), 0, target);

        if (st.currentValue >= target)
            UnlockInternal(def, st);
    }

    public void SetUnlocked(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        if (!_defsById.TryGetValue(id, out var def))
        {
            Debug.LogWarning($"[AchievementService] Definition not found: {id}");
            return;
        }
        if (!_stateById.TryGetValue(id, out var st))
        {
            Debug.LogWarning($"[AchievementService] State not found: {id}");
            return;
        }
        if (st.unlocked) return;

        UnlockInternal(def, st);
    }

    public bool IsUnlocked(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return false;
        return _stateById.TryGetValue(id, out var st) && st.unlocked;
    }

    public int GetProgress(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return 0;
        return _stateById.TryGetValue(id, out var st) ? st.currentValue : 0;
    }

    public List<AchievementState> GetAllStates()
    {
        var list = new List<AchievementState>(_stateById.Count);

        foreach (var kvp in _stateById)
        {
            var s = kvp.Value;
            list.Add(new AchievementState
            {
                id = s.id,
                currentValue = s.currentValue,
                unlocked = s.unlocked,
                unlockedUnixTime = s.unlockedUnixTime
            });
        }

        return list;
    }

    private void UnlockInternal(AchievementDefinition def, AchievementState st)
    {
        int target = Mathf.Max(1, def.targetValue);

        st.unlocked = true;
        st.currentValue = Mathf.Max(st.currentValue, target);
        st.unlockedUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        Debug.Log($"[AchievementService] Unlocked: {def.id}");
        OnAchievementUnlocked?.Invoke(def);
    }
}