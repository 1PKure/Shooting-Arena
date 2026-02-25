using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsPanelUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        StartCoroutine(InitRoutine());
    }

    private IEnumerator InitRoutine()
    {
        while (AudioManager.Instance == null)
            yield return null;

        masterSlider.SetValueWithoutNotify(AudioManager.Instance.GetMaster());
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusic());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfx());

        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Save();
    }

    private void OnMasterChanged(float v) => AudioManager.Instance.SetMaster(v);
    private void OnMusicChanged(float v) => AudioManager.Instance.SetMusic(v);
    private void OnSfxChanged(float v) => AudioManager.Instance.SetSfx(v);
}