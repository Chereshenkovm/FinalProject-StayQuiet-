using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Контроллер игрока")]
    [SerializeField] private PlayerController playerController;
    
    [Header("Окна UI")]
    public PauseWindow PauseWindow;
    public SettingWindow SettingsWindow;

    [Header("Аудиоклипы для UI")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Header("AudioSource для UI")]
    public AudioSource AudioSource;

    private void OnEnable()
    {
        #if !UNITY_EDITOR
        Cursor.visible = false;
        #endif
    }

    private void Start()
    {
        PauseWindow.OnContinueButton += () =>
        {
            PlayClip(clickSound);
            playerController.enabled = true;
            PauseWindow.gameObject.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
        };

        PauseWindow.OnMainMenuButton += () =>
        {
            PlayClip(clickSound);
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        };

        PauseWindow.OnSettingsButton += () =>
        {
            PlayClip(clickSound);
            SettingsWindow.gameObject.SetActive(true);
        };

        PauseWindow.OnExitButton += () =>
        {
            PlayClip(clickSound);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        };

        PauseWindow.OnHoverButton += () =>
        {
            PlayClip(hoverSound);
        };

        SettingsWindow.OnCloseButton += () =>
        {
            SettingsWindow.gameObject.SetActive(false);
        };
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                playerController.enabled = false;
                PauseWindow.gameObject.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                playerController.enabled = true;
                PauseWindow.gameObject.SetActive(false);
                SettingsWindow.gameObject.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
    }
    
    private void PlayClip(AudioClip _audioClip)
    {
        AudioSource.clip = _audioClip;
        AudioSource.Play();
    }
}
