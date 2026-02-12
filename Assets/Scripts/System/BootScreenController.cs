using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScreenController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 0.35f;
    [SerializeField] private float holdTime = 1.25f;
    [SerializeField] private float fadeOutTime = 0.35f;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "MainMenu";

    private void Awake()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(RunBootSequence());
    }

    private IEnumerator RunBootSequence()
    {
        yield return Fade(0f, 1f, fadeInTime);
        yield return WaitUnscaled(holdTime);
        yield return Fade(1f, 0f, fadeOutTime);

        var op = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;
    }

    private IEnumerator Fade(float from, float to, float time)
    {
        if (canvasGroup == null) yield break;

        float t = 0f;
        canvasGroup.alpha = from;

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(t / time));
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private static IEnumerator WaitUnscaled(float seconds)
    {
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}