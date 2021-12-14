using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Sounds;
using Input;
using ModestTree;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Header("Player Inputs")]
    public PlayerInput PlayerInput;
    public PlayerMicInput PlayerMicInput;
    [Header("Player Camera")]
    public Camera mainCamera;
    [Header("Player Rigidbody")]
    public Rigidbody Rigidbody;
    [Header("Microphone Sphere Collider")]
    public SphereCollider SoundSphere;
    [Header("Точка спауна снаряда")]
    public Transform spawnTransform;
    [Header("Родительский трансформ, для спауна шагов")]
    public Transform levelTransform;
    
    [Header("Скорость игрока")]
    [SerializeField] private float playerSpeed;
    [Header("Характеристики шагов")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private float secBetweenSteps = 0.5f;
    [SerializeField] private float maxSoundDistance = 10f;

    [Header("Громкость микрофона")] [SerializeField]
    private float distanceForMaxVolume = 20f;

    [Header("Предмет в руке")] [SerializeField]
    private List<GameObject> itemPrefab;
    
    [Header("UI окно игрока")]
    [SerializeField] private InGameUILogic uiWindow;
    
    private SoundManager _soundManager;
    private DiContainer _container;
    private bool isWalking = false;
    private bool isTiptoeing;
    
    [Inject]
    private void Construct(SoundManager soundManager, DiContainer container)
    {
        _soundManager = soundManager;
        _container = container;
    }
    void Update()
    {
        
        if (PlayerInput == null)
            return;
        
        var (soundVolume, micInitialized,micEnabled) = PlayerMicInput.MicInput();
        if (micInitialized && micEnabled)
        {
            Debug.Log(0);
            SoundSphere.radius = soundVolume * distanceForMaxVolume;
            uiWindow.SetUIVolume(soundVolume);
        }
        
        var (moveDirection, horizAngleRotation, vertAngleRotation, isFiring, tiptoe, canPickObject, pressedE, pickableObject) = PlayerInput.CurrentInput();
        isTiptoeing = tiptoe;

        Rigidbody.velocity = moveDirection.normalized * playerSpeed * (tiptoe ? 0.5f : 1) +
                             new Vector3(0, Rigidbody.velocity.y, 0);
        transform.Rotate(Vector3.up, horizAngleRotation);
        mainCamera.transform.Rotate(Vector3.right, vertAngleRotation);
        uiWindow.ShowEWindowFunction(canPickObject);
        
        if (canPickObject && pressedE)
        {
            pickableObject.GetComponent<Pickable>().AddProjectilePrefab(itemPrefab);
        }
        
        if (moveDirection.magnitude > 0 && !isWalking)
        {
            isWalking = true;
            StartCoroutine("SpawnSteps");
        }else if (moveDirection.magnitude == 0)
        {
            isWalking = false;
            StopCoroutine("SpawnSteps");
        }

        if (isFiring)
        {
            ThrowObject();
        }
    }

    private void ThrowObject()
    {
        if (itemPrefab.Count != 0)
        {
            _container.InstantiatePrefab(itemPrefab[0], spawnTransform.position, mainCamera.transform.rotation,
                levelTransform);
            itemPrefab.Remove(itemPrefab[0]);
        }
    }
    
    IEnumerator SpawnSteps()
    {
        while (true)
        {
            _soundManager.CreateSoundObject().Play(stepSound,transform.position, (isTiptoeing ? 0.01f : 0.1f), maxSoundDistance*(isTiptoeing ? 0.01f:1));
            yield return new WaitForSeconds(secBetweenSteps * (isTiptoeing ? 2f : 1));
        }
    }
}
