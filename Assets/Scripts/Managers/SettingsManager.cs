using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private const string PrefResIndex = "Settings_ResolutionIndex";
    private const string PrefFullscreen = "Settings_Fullscreen";
    private const string PrefFps = "Settings_FpsLimit";
    private const string PrefVsync = "Settings_Vsync";

    public Resolution[] AvailableResolutions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AvailableResolutions = Screen.resolutions;
            ApplySavedSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySavedSettings()
    {
        int resIndex = PlayerPrefs.GetInt(PrefResIndex, GetDefaultResolutionIndex());
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreen, 1) == 1;
        int fpsLimit = PlayerPrefs.GetInt(PrefFps, 60);
        int vsync = PlayerPrefs.GetInt(PrefVsync, 0);

        ApplyResolution(resIndex, fullscreen);
        ApplyVSync(vsync);
        ApplyFpsLimit(fpsLimit);
    }

    public int GetSavedResolutionIndex() => PlayerPrefs.GetInt(PrefResIndex, GetDefaultResolutionIndex());
    public bool GetSavedFullscreen() => PlayerPrefs.GetInt(PrefFullscreen, 1) == 1;
    public int GetSavedFpsLimit() => PlayerPrefs.GetInt(PrefFps, 60);
    public int GetSavedVSync() => PlayerPrefs.GetInt(PrefVsync, 0);

    public void ApplyResolution(int resolutionIndex, bool fullscreen)
    {
        if (AvailableResolutions == null || AvailableResolutions.Length == 0)
            AvailableResolutions = Screen.resolutions;

        resolutionIndex = Mathf.Clamp(resolutionIndex, 0, AvailableResolutions.Length - 1);
        Resolution r = AvailableResolutions[resolutionIndex];

        Screen.SetResolution(r.width, r.height, fullscreen);

        PlayerPrefs.SetInt(PrefResIndex, resolutionIndex);
        PlayerPrefs.SetInt(PrefFullscreen, fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplyFpsLimit(int fpsLimit)
    {
        Application.targetFrameRate = fpsLimit <= 0 ? -1 : fpsLimit;

        PlayerPrefs.SetInt(PrefFps, fpsLimit);
        PlayerPrefs.Save();
    }

    public void ApplyVSync(int vsyncCount)
    {
        QualitySettings.vSyncCount = Mathf.Clamp(vsyncCount, 0, 2);

        PlayerPrefs.SetInt(PrefVsync, QualitySettings.vSyncCount);
        PlayerPrefs.Save();
    }

    private int GetDefaultResolutionIndex()
    {
        Resolution current = Screen.currentResolution;
        Resolution[] res = Screen.resolutions;

        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].width == current.width && res[i].height == current.height)
                return i;
        }
        return Mathf.Max(0, res.Length - 1);
    }
}