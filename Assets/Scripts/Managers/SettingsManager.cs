using System.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private const string PrefResIndex = "Settings_ResolutionIndex";
    private const string PrefFullscreen = "Settings_Fullscreen";
    private const string PrefFps = "Settings_FpsLimit";
    private const string PrefVsync = "Settings_Vsync";
    private const string PrefMouseSens = "Settings_MouseSensitivity";
    private const string PrefGamepadSens = "Settings_GamepadSensitivity";

    [SerializeField] private PlayerData playerData;

    public Resolution[] AvailableResolutions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AvailableResolutions = Screen.resolutions;
            RefreshAvailableResolutions();
            ApplySavedSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void RefreshAvailableResolutions()
    {
        AvailableResolutions = Screen.resolutions
            .GroupBy(r => (r.width, r.height))
            .Select(g => g.OrderByDescending(x => x.refreshRateRatio.value).First())
            .OrderBy(r => r.width)
            .ThenBy(r => r.height)
            .ToArray();
    }
    public void ApplySavedSettings()
    {
        if (AvailableResolutions == null || AvailableResolutions.Length == 0)
            RefreshAvailableResolutions();

        int resIndex = PlayerPrefs.GetInt(PrefResIndex, GetDefaultResolutionIndex());
        bool fullscreen = PlayerPrefs.GetInt(PrefFullscreen, 1) == 1;
        int fpsLimit = PlayerPrefs.GetInt(PrefFps, 60);
        int vsync = PlayerPrefs.GetInt(PrefVsync, 0);

        resIndex = Mathf.Clamp(resIndex, 0, AvailableResolutions.Length - 1);

        ApplyResolution(resIndex, fullscreen);
        ApplyVSync(vsync);
        ApplyFpsLimit(fpsLimit);
        ApplySensitivity(
        GetSavedMouseSensitivity(),
        GetSavedGamepadSensitivity()
        );
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
        int w = Screen.width;
        int h = Screen.height;

        for (int i = 0; i < AvailableResolutions.Length; i++)
        {
            if (AvailableResolutions[i].width == w && AvailableResolutions[i].height == h)
                return i;
        }
        int bestIndex = 0;
        int bestDiff = int.MaxValue;
        int targetArea = w * h;

        for (int i = 0; i < AvailableResolutions.Length; i++)
        {
            int area = AvailableResolutions[i].width * AvailableResolutions[i].height;
            int diff = Mathf.Abs(area - targetArea);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    public float GetSavedMouseSensitivity()
    {
        if (playerData == null) return PlayerPrefs.GetFloat(PrefMouseSens, 1f);
        return PlayerPrefs.GetFloat(PrefMouseSens, playerData.mouseSensitivity);
    }

    public float GetSavedGamepadSensitivity()
    {
        if (playerData == null) return PlayerPrefs.GetFloat(PrefGamepadSens, 180f);
        return PlayerPrefs.GetFloat(PrefGamepadSens, playerData.gamepadLookSensitivity);
    }

    public void ApplySensitivity(float mouseSens, float gamepadSens)
    {
        if (playerData != null)
        {
            playerData.mouseSensitivity = mouseSens;
            playerData.gamepadLookSensitivity = gamepadSens;
        }

        PlayerPrefs.SetFloat(PrefMouseSens, mouseSens);
        PlayerPrefs.SetFloat(PrefGamepadSens, gamepadSens);
        PlayerPrefs.Save();
    }
}