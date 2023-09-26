using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown aaDropdown;

    private void Start() {
         aaDropdown.onValueChanged.AddListener(SetAntiAliasingFromDropdown);
    }

    public void SetAntiAliasingFromDropdown(int dropdownValue)
    {
        switch(dropdownValue)
        {
            case 0: // Disabled
                SetAntiAliasing(0);
                break;
            case 1: // 2x
                SetAntiAliasing(2);
                break;
            case 2: // 4x
                SetAntiAliasing(4);
                break;
            case 3: // 8x
                SetAntiAliasing(8);
                break;
            default:
                Debug.LogError("Unexpected dropdown value for anti-aliasing: " + dropdownValue);
                break;
        }
    }

    public void SetAntiAliasing(int level)
    {
        switch(level)
        {
            case 0:
            case 2:
            case 4:
            case 8:
                QualitySettings.antiAliasing = level;
                break;
            default:
                Debug.LogError("Invalid anti-aliasing level set: " + level);
                break;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainMixerVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
