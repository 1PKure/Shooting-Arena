using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStatsData : ScriptableObject
{
    public int maxHealth;
    public int pointValue;
}
