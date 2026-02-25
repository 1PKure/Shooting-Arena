using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Mensajes")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text killText;
    [SerializeField] private float messageDuration = 2f;

    [Header("UI Gameplay")]
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text reloadHintText;
    [SerializeField] private Image healthBar;

    [SerializeField] private DamageOverlayUI damageOverlay;
    private float messageTimer;
    private float killTextTimer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (killText != null)
            killText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (messageText != null && messageText.gameObject.activeSelf)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
                messageText.gameObject.SetActive(false);
        }

        if (killText != null && killText.gameObject.activeSelf)
        {
            killTextTimer -= Time.deltaTime;
            if (killTextTimer <= 0f)
                killText.gameObject.SetActive(false);
        }
    }
    public void PlayDamageOverlay()
    {
        damageOverlay?.PlayHit();
    }
    public void ShowMessage(string message)
    {
        if (messageText == null) return;

        messageText.text = message;
        messageText.gameObject.SetActive(true);
        messageTimer = messageDuration;
    }

    public void ShowKillMessage(int kills, int maxKills)
    {
        if (killText == null) return;

        killText.text = $"Enemies killed: {kills} / {maxKills}";
        killText.gameObject.SetActive(true);
        killTextTimer = messageDuration;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
            ammoText.text = $"Bullets: {currentAmmo}";

        if (reloadHintText != null)
            reloadHintText.gameObject.SetActive(currentAmmo == 0);
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBar == null) return;

        float safeMax = Mathf.Max(1, max);
        float t = Mathf.Clamp01(current / safeMax);
        healthBar.fillAmount = t;
    }
}
