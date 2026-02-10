using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private float fallbackVfxLifetime = 2f;

    [Header("Audio")]
    public AudioClip hitSFX;
    public AudioClip missSFX;
    public AudioClip deathSFX;
    public AudioClip jumpSFX;
    public AudioClip winSFX;
    public AudioClip loseSFX;
    public AudioClip shootSFX;

    [Header("VFX")]
    public GameObject hitVFX;
    public GameObject missVFX;
    public GameObject deathVFX;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
    private void SpawnVFX(GameObject vfxPrefab, Vector3 position)
    {
        if (vfxPrefab == null) return;

        GameObject vfx = Instantiate(vfxPrefab, position, Quaternion.identity);

        var ps = vfx.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;

            float lifetime = main.duration;

            lifetime += GetMaxStartLifetime(main);

            if (main.loop) lifetime = fallbackVfxLifetime;

            Destroy(vfx, Mathf.Max(0.1f, lifetime));
            return;
        }
        Destroy(vfx, fallbackVfxLifetime);
    }

    private float GetMaxStartLifetime(ParticleSystem.MainModule main)
    {
        var lt = main.startLifetime;
        if (lt.mode == ParticleSystemCurveMode.Constant) return lt.constant;
        if (lt.mode == ParticleSystemCurveMode.TwoConstants) return lt.constantMax;

        return lt.constantMax;
    }
    public void PlayHitFeedback(Vector3 position, AudioClip sfxOverride, GameObject vfxOverride)
    {
        AudioClip sfx = sfxOverride != null ? sfxOverride : hitSFX;
        GameObject vfx = vfxOverride != null ? vfxOverride : hitVFX;

        if (sfx) AudioManager.Instance.PlaySFX(sfx);
        if (vfx) SpawnVFX(vfx, position);
    }

    public void PlayMissFeedback(Vector3 position, AudioClip sfxOverride, GameObject vfxOverride)
    {
        AudioClip sfx = sfxOverride != null ? sfxOverride : missSFX;
        GameObject vfx = vfxOverride != null ? vfxOverride : missVFX;

        if (sfx) AudioManager.Instance.PlaySFX(sfx);
        if (vfx) SpawnVFX(vfx, position);
    }

    public void PlayDeathFeedback(Vector3 position)
    {
        if (deathSFX) AudioManager.Instance.PlaySFX(deathSFX);
        if (deathVFX) Instantiate(deathVFX, position, Quaternion.identity);
    }

    public void PlayJumpFeedback(Vector3 position)
    {
        if (jumpSFX) AudioManager.Instance.PlaySFX(jumpSFX);
    }

    public void PlayWinFeedback()
    {
        if (winSFX) AudioManager.Instance.PlaySFX(winSFX);
    }

    public void PlayLoseFeedback()
    {
        if (loseSFX) AudioManager.Instance.PlaySFX(loseSFX);
    }

    public void PlayShootFeedback(AudioClip sfxOverride)
    {
        AudioClip sfx = sfxOverride != null ? sfxOverride : shootSFX;
        if (sfx) AudioManager.Instance.PlaySFX(sfx);
    }
}

