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
    private AudioSource audioSource;

    [Header("VFX")]
    public GameObject hitVFX;
    public GameObject missVFX;
    public GameObject deathVFX;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitFeedback(Vector3 position)
    {
        if (hitSFX) audioSource.PlayOneShot(hitSFX);
        if (hitVFX) Instantiate(hitVFX, position, Quaternion.identity);
    }

    public void PlayMissFeedback(Vector3 position)
    {
        if (missSFX) audioSource.PlayOneShot(missSFX);
        if (missVFX) Instantiate(missVFX, position, Quaternion.identity);
    }

    public void PlayDeathFeedback(Vector3 position)
    {
        if (deathSFX) audioSource.PlayOneShot(deathSFX);
        if (deathVFX) Instantiate(deathVFX, position, Quaternion.identity);
    }
}

