using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    public event Action OnCloseButton;
    public event Action ReInitMic;

    [SerializeField] private UnityEngine.UI.Dropdown MicDropdown;
    [SerializeField] private UnityEngine.UI.Slider Volume;
    [SerializeField] private UnityEngine.UI.Toggle usingMic;

    private void OnEnable()
    {
        MicDropdown.options.Clear();
        foreach (var micDevice in Microphone.devices)
        {
            MicDropdown.options.Add(new Dropdown.OptionData());
            Debug.Log(MicDropdown.options.Count);
            MicDropdown.options[MicDropdown.options.Count-1].text = micDevice;
        }

        if (PlayerPrefs.HasKey("microphone"))
        {
            foreach (var micDevice in MicDropdown.options)
            {
                if (micDevice.text == PlayerPrefs.GetString("microphone"))
                {
                    MicDropdown.value = MicDropdown.options.IndexOf(micDevice);
                }
            }
        }
        else
        {
            MicDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("Volume"))
        {
            Volume.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            Volume.value = 1f;
        }

        if (PlayerPrefs.HasKey("useMic"))
        {
            usingMic.isOn = PlayerPrefs.GetInt("useMic") == 1 ? true : false;
        }
        else
        {
            usingMic.isOn = true;
        }
    }

    public void CloseSettingsWindow()
    {
        ResetSettings();
        OnCloseButton?.Invoke();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("microphone", MicDropdown.options[MicDropdown.value].text);
        PlayerPrefs.SetFloat("Volume", Volume.value);
        PlayerPrefs.SetInt("useMic", usingMic.isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        if (SceneManager.GetActiveScene().name == "Game")
        {
            ReInitMic?.Invoke();
            Debug.Log("Saved");
        }
    }

    public void OnValueChanged()
    {
        AudioListener.volume = Volume.value;
    }

    public void ResetSettingsButton()
    {
        ResetSettings();
        Volume.value = AudioListener.volume;
        if (PlayerPrefs.HasKey("microphone"))
        {
            foreach (var micDevice in MicDropdown.options)
            {
                if (micDevice.text == PlayerPrefs.GetString("microphone"))
                {
                    MicDropdown.value = MicDropdown.options.IndexOf(micDevice);
                }
            }
        }
        else
        {
            MicDropdown.value = 0;
        }

        if (PlayerPrefs.HasKey("useMic"))
        {
            usingMic.isOn = PlayerPrefs.GetInt("useMic") == 1 ? true : false;
        }
        else
        {
            usingMic.isOn = true;
        }
    }
    private void ResetSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        else
            AudioListener.volume = 1f;
    }
}
