using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPopupUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform popupRoot;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Timings")]
    [SerializeField] private float fadeInSeconds = 0.20f;
    [SerializeField] private float holdSeconds = 2.25f;
    [SerializeField] private float fadeOutSeconds = 0.25f;

    [Header("Slide")]
    [SerializeField] private bool useSlide = true;
    [SerializeField] private Vector2 hiddenAnchoredPos = new Vector2(420f, 0f);
    [SerializeField] private Vector2 shownAnchoredPos = Vector2.zero;
    [SerializeField] private float slideSeconds = 0.20f;

    private readonly Queue<AchievementDefinition> _queue = new Queue<AchievementDefinition>();
    private Coroutine _runner;
    private AchievementService _service;
    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponentInChildren<CanvasGroup>(true);
        if (popupRoot == null) popupRoot = GetComponentInChildren<RectTransform>(true);

        HideInstant();
    }

    private void OnDestroy()
    {
        if (_service != null)
            _service.OnAchievementUnlocked -= HandleUnlocked;
    }

    public void Bind(AchievementService service)
    {
        if (_service != null)
            _service.OnAchievementUnlocked -= HandleUnlocked;

        _service = service;

        if (_service != null)
            _service.OnAchievementUnlocked += HandleUnlocked;

        Debug.Log($"[AchievementsPopupUI] Bind OK. instance={GetInstanceID()} serviceNull={(service == null)}");
    }

    private void HandleUnlocked(AchievementDefinition def)
    {
        if (def == null) return;

        _queue.Enqueue(def);

        if (_runner == null)
            _runner = StartCoroutine(RunQueue());

        Debug.Log($"[AchievementsPopupUI] HandleUnlocked: {def.id} (instance={GetInstanceID()})");
    }

    private IEnumerator RunQueue()
    {
        while (_queue.Count > 0)
        {
            var def = _queue.Dequeue();
            Apply(def);

            yield return AnimateShow();
            yield return new WaitForSeconds(holdSeconds);
            yield return AnimateHide();
        }

        _runner = null;
    }

    private void Apply(AchievementDefinition def)
    {
        if (titleText != null) titleText.text = def.title;
        if (descriptionText != null) descriptionText.text = def.description;

        if (iconImage != null)
        {
            if (def.icon != null)
            {
                iconImage.sprite = def.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.enabled = false;
            }
        }
    }

    private IEnumerator AnimateShow()
    {
        float t = 0f;

        if (useSlide && popupRoot != null)
            popupRoot.anchoredPosition = hiddenAnchoredPos;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / Mathf.Max(0.001f, fadeInSeconds);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.SmoothStep(0f, 1f, t));

            if (useSlide && popupRoot != null)
            {
                float s = Mathf.Clamp01((fadeInSeconds > 0f) ? (t * (fadeInSeconds / Mathf.Max(0.001f, slideSeconds))) : 1f);
                popupRoot.anchoredPosition = Vector2.Lerp(hiddenAnchoredPos, shownAnchoredPos, Mathf.SmoothStep(0f, 1f, s));
            }

            yield return null;
        }

        canvasGroup.alpha = 1f;

        if (useSlide && popupRoot != null)
            popupRoot.anchoredPosition = shownAnchoredPos;
    }

    private IEnumerator AnimateHide()
    {
        float t = 0f;
        float alphaStart = canvasGroup.alpha;
        float alphaEnd = 0f;

        Vector2 startPos = popupRoot != null ? popupRoot.anchoredPosition : Vector2.zero;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / Mathf.Max(0.001f, fadeOutSeconds);
            float a = Mathf.Lerp(alphaStart, alphaEnd, Mathf.SmoothStep(0f, 1f, t));
            canvasGroup.alpha = a;

            if (useSlide && popupRoot != null)
            {
                float s = Mathf.Clamp01((fadeOutSeconds > 0f) ? (t * (fadeOutSeconds / Mathf.Max(0.001f, slideSeconds))) : 1f);
                popupRoot.anchoredPosition = Vector2.Lerp(startPos, hiddenAnchoredPos, Mathf.SmoothStep(0f, 1f, s));
            }

            yield return null;
        }

        HideInstant();
    }

    private void HideInstant()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (useSlide && popupRoot != null)
            popupRoot.anchoredPosition = hiddenAnchoredPos;
    }
}