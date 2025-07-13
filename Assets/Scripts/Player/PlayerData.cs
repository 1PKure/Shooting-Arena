using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public int maxHealth = 100;
    public float moveSpeed = 5f;
    public float baseSpeed = 5f;
    public float mouseSensitivity = 3f;
    public Vector3 spawnPosition;
}