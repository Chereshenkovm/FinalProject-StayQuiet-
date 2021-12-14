using System;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    public event Action OnStartGameButton;
    public event Action OnSettingsGameButton;
    public event Action OnExitButton;

    public event Action HoverButton;

    public void StartGame()
    {
        OnStartGameButton?.Invoke();
    }

    public void OpenSettings()
    {
        OnSettingsGameButton?.Invoke();
    }

    public void ExitGame()
    {
        OnExitButton?.Invoke();
    }

    public void PlaySoundHover()
    {
        HoverButton?.Invoke();
    }
}
