using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public float remainingTime;
    public int difficulty;
    public SerializableVector3 playerPosition;
    public string equippedWeapon;
}
