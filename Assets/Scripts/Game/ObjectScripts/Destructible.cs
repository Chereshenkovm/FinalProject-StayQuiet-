using System;
using System.Collections;
using System.Collections.Generic;
using Core.Sounds;
using Zenject;
using UnityEngine;
using Object = System.Object;

[RequireComponent(typeof(Rigidbody))]
public class Destructible : MonoBehaviour
{
    [Header("Скорость для разрушения")] [SerializeField]
    private float destructSpeed = 10f;

    [Header("Максимальная дистанция")] 
    [SerializeField] private float maxDistance = 10f;
    
    private SoundManager _soundManager;
    private Rigidbody _rigidbody;

    private float objectVelocity;

    public AudioClip breakSound;

    [Header("Эффект разрушения")] [SerializeField]
    private ParticleSystem destroyEffect;
    
    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(VelocityWrite());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (objectVelocity > destructSpeed)
        {
            _soundManager.CreateSoundObject().Play(breakSound, transform.position, maxDistance: maxDistance);
            GameObject.Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator VelocityWrite()
    {
        while (true)
        {
            objectVelocity = _rigidbody.velocity.magnitude;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
