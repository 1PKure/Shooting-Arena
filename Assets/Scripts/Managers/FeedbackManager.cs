using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

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

    public void PlayHitFeedback(Vector3 position)
    {
        if (hitSFX) AudioManager.Instance.PlaySFX(hitSFX);
        if (hitVFX) Instantiate(hitVFX, position, Quaternion.identity);
    }

    public void PlayMissFeedback(Vector3 position)
    {
        if (missSFX) AudioManager.Instance.PlaySFX(missSFX);
        if (missVFX) Instantiate(missVFX, position, Quaternion.identity);
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

    public void PlayShootFeedback()
    {
        if (shootSFX) AudioManager.Instance.PlaySFX(shootSFX);
    }
}

