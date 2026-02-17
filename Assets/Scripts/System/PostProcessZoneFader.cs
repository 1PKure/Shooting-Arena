using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessZoneFader : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private string playerTag = "Player";

    [Header("Fade")]
    [SerializeField] private float fadeInTime = 0.35f;
    [SerializeField] private float fadeOutTime = 0.35f;

    private PostProcessVolume volume;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.isGlobal = false;
        volume.weight = 0f;           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(1f, fadeInTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartFade(0f, fadeOutTime);
    }

    private void StartFade(float target, float duration)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeWeight(target, duration));
    }

    private IEnumerator FadeWeight(float target, float duration)
    {
        float start = volume.weight;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = duration <= 0f ? 1f : Mathf.Clamp01(t / duration);
            volume.weight = Mathf.Lerp(start, target, k);
            yield return null;
        }

        volume.weight = target;
        fadeRoutine = null;
    }
}