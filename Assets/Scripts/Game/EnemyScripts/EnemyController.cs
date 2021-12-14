using System;
using System.Collections;
using System.Collections.Generic;
using Core.Sounds;
using Input;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Inputs")]
    public EnemyInput EnemyInput;
    
    [Header("Enemy components")]
    public Animator Animator;
    public Rigidbody Rigidbody;


    [Header("Характеристики врага")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private float secBetweenSteps;
    
    [Header("Game manager")]
    public EndingManager EndingManager;
    
    private bool hasWondered = false;
    private bool isWondering = false;
    private bool moving = false;
    private bool gameFinished = false;

    private SoundManager _soundManager;
    private DiContainer _container;
    
    [Inject]
    private void Construct(SoundManager soundManager, DiContainer container)
    {
        _soundManager = soundManager;
        _container = container;
    }
    
    void Update()
    {
        if (EnemyInput == null)
            return;

        if (!gameFinished)
        {
            var (moveDirection, viewDirection, something, noSoundSource, pointReached, playerFound) =
                EnemyInput.CurrentInput();
            if (playerFound)
            {
                gameFinished = playerFound;
                EndingManager.GameEnd();
                return;
            }

            if (!hasWondered)
            {
                hasWondered = pointReached;
                if (!hasWondered)
                {
                    Rigidbody.velocity = something
                        ? moveDirection.normalized * (noSoundSource ? enemySpeed * 0.2f : enemySpeed)
                        : Vector3.zero;
                    transform.rotation = new Quaternion(0, viewDirection.y, 0, viewDirection.w);
                    Animator.SetBool("isRunning",
                        (Rigidbody.velocity != Vector3.zero && !noSoundSource) ? true : false);
                    Animator.SetBool("isWalking", noSoundSource ? true : false);

                    if (Rigidbody.velocity.magnitude > 0 && !moving)
                    {
                        moving = true;
                        StartCoroutine("SpawnSteps");
                    }
                    else if (Rigidbody.velocity.magnitude == 0)
                    {
                        moving = false;
                        StopCoroutine("SpawnSteps");
                    }
                }
            }
            else
            {
                if (!isWondering)
                {
                    isWondering = true;
                    StartCoroutine(Wonder());
                }

                if (!noSoundSource)
                {
                    StopCoroutine(Wonder());
                    Animator.SetBool("reachedThePoint", false);
                    hasWondered = false;
                    isWondering = false;
                }
            }
        }
    }

    IEnumerator Wonder()
    {
        Rigidbody.velocity = Vector3.zero;
        moving = false;
        StopCoroutine("SpawnSteps");
        Animator.SetBool("isRunning", false);
        Animator.SetBool("isWalking", false);
        Animator.SetBool("reachedThePoint", true);
        yield return new WaitForSeconds(4.0f);
        Animator.SetBool("reachedThePoint", false);
        hasWondered = false;
        isWondering = false;
    }
    
    IEnumerator SpawnSteps()
    {
        while (true)
        {
            _soundManager.CreateSoundObject().Play(stepSound,transform.position, detectable: false);
            yield return new WaitForSeconds(secBetweenSteps * (Animator.GetBool("isWalking")?1:0.2f));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.GetComponentInParent<PlayerController>())
            EndingManager.GameEnd();
    }
}
