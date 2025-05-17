using UnityEngine;

public enum EnemyTypeEnum { Static, Mover, Shooter }

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public EnemyTypeEnum enemyType;
    public int weight = 1;
}
