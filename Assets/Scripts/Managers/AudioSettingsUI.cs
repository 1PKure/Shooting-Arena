using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.onValueChanged.AddListener(val => AudioManager.Instance.SetMusicVolume(val));
        sfxSlider.onValueChanged.AddListener(val => AudioManager.Instance.SetSFXVolume(val));
    }
}
