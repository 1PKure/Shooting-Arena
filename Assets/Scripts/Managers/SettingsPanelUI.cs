using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown fpsDropdown;
    [SerializeField] private TMP_Dropdown vsyncDropdown;

    [Header("Sensitivity")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider gamepadSensitivitySlider;

    private readonly int[] fpsOptions = { 30, 60, 120, 144, 240, 0 };

    private void OnEnable()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsManager not found in scene.");
            return;
        }

        PopulateResolutionDropdown();
        PopulateFpsDropdown();
        PopulateVSyncDropdown();

        resolutionDropdown.value = SettingsManager.Instance.GetSavedResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = SettingsManager.Instance.GetSavedFullscreen();

        int savedFps = SettingsManager.Instance.GetSavedFpsLimit();
        fpsDropdown.value = FindFpsIndex(savedFps);
        fpsDropdown.RefreshShownValue();

        vsyncDropdown.value = Mathf.Clamp(SettingsManager.Instance.GetSavedVSync(), 0, 2);
        vsyncDropdown.RefreshShownValue();

        mouseSensitivitySlider.SetValueWithoutNotify(SettingsManager.Instance.GetSavedMouseSensitivity());
        gamepadSensitivitySlider.SetValueWithoutNotify(SettingsManager.Instance.GetSavedGamepadSensitivity());
    }

    private void PopulateResolutionDropdown()
    {
        var res = SettingsManager.Instance.AvailableResolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>(res.Length);
        for (int i = 0; i < res.Length; i++)
            options.Add($"{res[i].width} x {res[i].height}");

        resolutionDropdown.AddOptions(options);
    }

    private void PopulateFpsDropdown()
    {
        fpsDropdown.ClearOptions();

        List<string> options = new List<string>(fpsOptions.Length);
        foreach (int fps in fpsOptions)
            options.Add(fps == 0 ? "Unlimited" : fps.ToString());

        fpsDropdown.AddOptions(options);
    }

    private void PopulateVSyncDropdown()
    {
        vsyncDropdown.ClearOptions();
        vsyncDropdown.AddOptions(new List<string>
        {
            "Off",
            "On (Every VBlank)",
            "Half (Every 2 VBlanks)"
        });
    }

    public void Apply()
    {
        int resIndex = resolutionDropdown.value;
        bool fullscreen = fullscreenToggle.isOn;
        int fpsLimit = fpsOptions[Mathf.Clamp(fpsDropdown.value, 0, fpsOptions.Length - 1)];
        int vsync = vsyncDropdown.value;

        SettingsManager.Instance.ApplyResolution(resIndex, fullscreen);
        SettingsManager.Instance.ApplyVSync(vsync);
        SettingsManager.Instance.ApplyFpsLimit(fpsLimit);
        SettingsManager.Instance.ApplySensitivity(
        mouseSensitivitySlider.value,
        gamepadSensitivitySlider.value
        );
        UIManager.Instance?.ShowMessage("Settings applied");
    }

    private int FindFpsIndex(int fps)
    {
        for (int i = 0; i < fpsOptions.Length; i++)
        {
            if (fpsOptions[i] == fps)
                return i;
        }
        return 1;
    }
}