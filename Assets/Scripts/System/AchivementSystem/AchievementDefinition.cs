using UnityEngine;

[CreateAssetMenu(menuName = "Game/Achievements/Achievement Definition")]
public class AchievementDefinition : ScriptableObject
{
    [Header("Identity")]
    public string id;                 
    public string title;
    [TextArea] public string description;

    [Header("Progress")]
    public int targetValue = 1;        

    [Header("Presentation")]
    public Sprite icon;
}