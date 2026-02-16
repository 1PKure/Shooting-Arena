using System.Collections;
using UnityEngine;

public class EndScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;
    [SerializeField] private bool disableOnHide = false;

    private void OnEnable()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f, true));
    }

    public void HideInstant()
    {
        StopAllCoroutines();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (disableOnHide) gameObject.SetActive(false);
    }

    private IEnumerator Fade(float from, float to, bool enableInput)
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = from;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
        canvasGroup.interactable = enableInput;
        canvasGroup.blocksRaycasts = enableInput;
    }
}