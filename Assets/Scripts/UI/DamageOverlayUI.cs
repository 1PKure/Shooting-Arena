using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlayUI : MonoBehaviour
{
    [SerializeField] private Image overlay;
    [SerializeField, Range(0f, 1f)] private float maxAlpha = 0.45f;
    [SerializeField] private float fadeInTime = 0.05f;
    [SerializeField] private float fadeOutTime = 0.25f;

    private Coroutine routine;
    [ContextMenu("TEST/Play Hit")]
    private void TestPlayHit()
    {
        PlayHit();
    }
    private void Reset()
    {
        overlay = GetComponent<Image>();
    }

    public void PlayHit()
    {
        if (overlay == null) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        yield return FadeTo(maxAlpha, fadeInTime);
        yield return FadeTo(0f, fadeOutTime);
        routine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, float time)
    {
        Color c = overlay.color;
        float startAlpha = c.a;

        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / time);
            overlay.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        overlay.color = new Color(c.r, c.g, c.b, targetAlpha);
    }
}
