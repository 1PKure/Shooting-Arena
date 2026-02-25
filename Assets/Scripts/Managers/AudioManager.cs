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

    private const string masterParam = "MasterVolume";
    private const string musicParam = "MusicVolume";
    private const string sfxParam = "SfxVolume";

    private float master = 1f;
    private float music = 1f;
    private float sfx = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
        ApplyAll();
        ResetAudioPrefs();
    }



    public float GetMaster() => master;
    public float GetMusic() => music;
    public float GetSfx() => sfx;
    public void ResetAudioPrefs()
    {
        PlayerPrefs.DeleteKey("vol_master");
        PlayerPrefs.DeleteKey("vol_music");
        PlayerPrefs.DeleteKey("vol_sfx");
        PlayerPrefs.Save();

        master = music = sfx = 1f;
        ApplyAll();
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

    public void SetMaster(float value01)
    {
        master = Mathf.Clamp01(value01);
        Apply(masterParam, master);
        PlayerPrefs.SetFloat(MasterKey, master);
    }

    public void SetMusic(float value01)
    {
        music = Mathf.Clamp01(value01);
        Apply(musicParam, music);
        PlayerPrefs.SetFloat(MusicKey, music);
    }

    public void SetSfx(float value01)
    {
        sfx = Mathf.Clamp01(value01);
        Apply(sfxParam, sfx);
        PlayerPrefs.SetFloat(SfxKey, sfx);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    private void Load()
    {
        master = PlayerPrefs.GetFloat(MasterKey, 1f);
        music = PlayerPrefs.GetFloat(MusicKey, 1f);
        sfx = PlayerPrefs.GetFloat(SfxKey, 1f);
    }

    private void ApplyAll()
    {
        Apply(masterParam, master);
        Apply(musicParam, music);
        Apply(sfxParam, sfx);
    }

    private void Apply(string param, float value01)
    {
        float v = Mathf.Clamp(value01, 0.0001f, 1f);
        float db = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat(param, db);
    }
}
