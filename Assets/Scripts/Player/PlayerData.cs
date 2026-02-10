using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Vitals")]
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float baseSpeed = 5f;
    public float mouseSensitivity = 3f;

    [Header("Sprint")]
    [Min(1f)] public float sprintMultiplier = 1.6f;
    [Min(0f)] public float maxStamina = 5f;
    [Min(0f)] public float staminaDrainPerSecond = 1f;
    [Min(0f)] public float staminaRegenPerSecond = 1.25f;
    [Min(0f)] public float staminaRegenDelay = 0.25f;
    [Range(0f, 1f)] public float minStaminaToStartSprint = 0.05f;

    [Header("Spawn")]
    public Vector3 spawnPosition;
}