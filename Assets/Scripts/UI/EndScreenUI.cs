using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalTimeText;

    public void SetData(int score, float remainingTime)
    {
        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {score}";

        if (finalTimeText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            finalTimeText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }
    }
}