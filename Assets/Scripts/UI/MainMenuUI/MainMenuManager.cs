using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public MainMenuWindow MainMenuWindow;
    public SettingWindow SettingWindow;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioSource AudioSource;
    private void Start()
    {
        MainMenuWindow.OnStartGameButton += () =>
        {
            PlayClip(clickSound);
            SceneManager.LoadScene("Game");
        };

        MainMenuWindow.OnSettingsGameButton += () =>
        {
            PlayClip(clickSound);
            SettingWindow.gameObject.SetActive(true);
        };

        MainMenuWindow.OnExitButton += () =>
        {
            PlayClip(clickSound);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        };

        MainMenuWindow.HoverButton += () =>
        {
            PlayClip(hoverSound);
        };
        
        
        SettingWindow.OnCloseButton += () =>
        {
            SettingWindow.gameObject.SetActive(false);
        };
    }

    private void PlayClip(AudioClip _audioClip)
    {
        AudioSource.clip = _audioClip;
        AudioSource.Play();
    }
}
