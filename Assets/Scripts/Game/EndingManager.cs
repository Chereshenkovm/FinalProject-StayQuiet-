using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [Header("Камеры, между которыми идёт переключение")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera endCamera;
    
    [Header("Копия противника")]
    [SerializeField] private GameObject killingEnemy;
    
    [Header("GameObject'ы персонажа и противника")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy;

    [Header("Элементы UI")]
    [SerializeField] private GameObject DeathScreenObject;
    [SerializeField] private GameObject GameUI;

    public void GameEnd()
    {
        DissableEntities();
        ChangeCameras();
        StartCoroutine(WaitingForAnimationEnd());
    }

    private void DissableEntities()
    {
        Player.SetActive(false);
        Enemy.SetActive(false);
    }
    
    private void ChangeCameras()
    {
        mainCamera.enabled = false;
        endCamera.enabled = true;
        endCamera.GetComponent<AudioListener>().enabled = true;
        killingEnemy.SetActive(true);
        GameUI.SetActive(false);
    }

    private void DeathScreen()
    {
        DeathScreenObject.SetActive(true);
    }

    IEnumerator WaitingForAnimationEnd()
    {
        yield return new WaitForSeconds(3f);
        DeathScreen();
        Cursor.visible = true;
        StartCoroutine(WaitForButton());
    }

    IEnumerator WaitForButton()
    {
        while (true)
        {
            if (UnityEngine.Input.anyKey)
            {
                SceneManager.LoadScene("MainMenu");
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
