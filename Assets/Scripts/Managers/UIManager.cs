using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text messageText;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] public TMP_Text killText;

    private float timer;
    public static UIManager Instance;
    void Awake()
    {
        Instance = this;
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (messageText != null && messageText.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                messageText.gameObject.SetActive(false);
        }

        if (killText.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                killText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (messageText == null) return;

        messageText.text = message;
        messageText.gameObject.SetActive(true);
        timer = messageDuration;
    }

    public void ShowKillMessage(int kills, int maxKills)
    {
        killText.text = $"Enemigos derrotados: {kills} / {maxKills}";
        killText.gameObject.SetActive(true);
        timer = messageDuration;
    }
}

