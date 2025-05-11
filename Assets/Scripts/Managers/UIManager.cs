using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Slider healthBar;
    public Text killText;
    public Text timerText;
    public GameObject endGamePanel;
    public Text endText;

    void Awake() => instance = this;

    public void UpdateHealth(float hp)
    {
        healthBar.value = hp;
    }

    public void UpdateKills(int k)
    {
        killText.text = "Kills: " + k;
    }

    public void UpdateTimer(float t)
    {
        timerText.text = "Time: " + Mathf.Ceil(t);
    }

    public void ShowEndPanel(bool won)
    {
        endGamePanel.SetActive(true);
        endText.text = won ? "¡Sobreviviste!" : "¡Te eliminaron!";
    }
}

