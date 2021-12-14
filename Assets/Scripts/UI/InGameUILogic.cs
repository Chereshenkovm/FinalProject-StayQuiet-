using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameUILogic : MonoBehaviour
{
    public UnityEngine.UI.Text EWindow;

    public RectTransform SoundImage;

    public void ShowEWindowFunction(bool canPick)
    {
        if(EWindow.gameObject.activeSelf !=canPick)
            EWindow.gameObject.SetActive(canPick);
    }

    public void SetUIVolume(float volume)
    {
        SoundImage.sizeDelta = new Vector2(100, volume * 300f);
        SoundImage.anchoredPosition = new Vector2(0, volume * 150f);
    }
}
