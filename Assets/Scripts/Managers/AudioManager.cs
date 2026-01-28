using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("One Shot SFX")]
    [SerializeField] private AudioSource sfxSource;

    private const string MasterKey = "vol_master";
    private const string MusicKey = "vol_music";
    private const string SfxKey = "vol_sfx";

    private const string MasterParam = "MasterVolume";
    private const string MusicParam = "MusicVolume";
    private const string SfxParam = "SfxVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAndApply();
    }

    private void LoadAndApply()
    {
        float master = PlayerPrefs.GetFloat(MasterKey, 1f);
        float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SfxKey, 1f);

        ApplyVolume(MasterParam, master);
        ApplyVolume(MusicParam, music);
        ApplyVolume(SfxParam, sfx);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (sfxSource == null)
        {
            Debug.LogWarning("AudioManager: sfxSource is null.");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public float GetMaster() => PlayerPrefs.GetFloat(MasterKey, 1f);
    public float GetMusic() => PlayerPrefs.GetFloat(MusicKey, 1f);
    public float GetSfx() => PlayerPrefs.GetFloat(SfxKey, 1f);

    public void SetMaster(float value)
    {
        PlayerPrefs.SetFloat(MasterKey, value);
        ApplyVolume(MasterParam, value);
    }

    public void SetMusic(float value)
    {
        PlayerPrefs.SetFloat(MusicKey, value);
        ApplyVolume(MusicParam, value);
    }

    public void SetSfx(float value)
    {
        PlayerPrefs.SetFloat(SfxKey, value);
        ApplyVolume(SfxParam, value);
    }

    private void ApplyVolume(string param, float linear01)
    {
        float db = (linear01 <= 0.0001f) ? -80f : Mathf.Log10(linear01) * 20f;
        audioMixer.SetFloat(param, db);
    }
}
