using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager not found in scene.");
            return;
        }

        masterSlider.SetValueWithoutNotify(AudioManager.Instance.GetMaster());
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusic());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfx());

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMaster);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusic);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSfx);
    }
}
