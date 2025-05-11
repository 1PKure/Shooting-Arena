using UnityEngine;

public class StaticEnemy : Enemy
{
    protected override void Start()
    {
        //puedo usar ScriptableObject aca
        base.Start();
        maxHealth = 50;
        currentHealth = maxHealth;
        pointValue = 10;
    }
}