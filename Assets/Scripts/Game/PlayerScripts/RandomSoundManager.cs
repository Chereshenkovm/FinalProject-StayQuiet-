using System;
using System.Collections;
using System.Collections.Generic;
using Core.Sounds;
using UnityEngine;
using Zenject;

public class RandomSoundManager : MonoBehaviour
{
    [SerializeField] private SphereCollider SphereCollider;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float soundDistance = 40f;

    private SoundManager _soundManager;
    private DiContainer _container;

    [SerializeField] private AudioClip randomSound;

    [Inject]
    private void Construct(SoundManager soundManager, DiContainer container)
    {
        _soundManager = soundManager;
        _container = container;
    }
    
    private void Start()
    {
        StartCoroutine(SpawnSound());
    }

    IEnumerator SpawnSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            _soundManager.CreateSoundObject().Play(randomSound,transform.position, 0.5f, soundDistance);
        }
    }
}
