using System;
using UnityEngine;

public class PauseWindow : MonoBehaviour
{
    public event Action OnContinueButton;
    public event Action OnMainMenuButton;
    public event Action OnSettingsButton;
    public event Action OnExitButton;

    public event Action OnHoverButton;

    public void ContinueGame()
    {
        OnContinueButton?.Invoke();
    }
    
    public void OpenMainMenu()
    {
        OnMainMenuButton?.Invoke();
    }

    public void OpenSettings()
    {
        OnSettingsButton?.Invoke();
    }

    public void ExitGame()
    {
        OnExitButton?.Invoke();
    }

    public void PlayHoverSound()
    {
        OnHoverButton?.Invoke();
    }
}
