using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.AI;

public class NavMashEnemy : EnemyInput
{
    [Header("Transform игрока")]
    [SerializeField] private Transform _player;

    [Header("Точки, по которым ходит противник")]
    [SerializeField] private Transform[] walkingPoints;
    
    private NavMeshPath path;
    private float closestDistance;
    private float distance;

    private Vector3 soundPosition;
    private Quaternion lastViewDirection = Quaternion.identity;

    private int currentPoint = 0;
    private bool reachedThePoint = false;

    private bool isPlayerCatched = false;

    public override (Vector3 moveDirection, Quaternion viewDirection, bool something, bool noSoundSource, bool reachedPoint, bool playerFound) CurrentInput()
    {
        path = new NavMeshPath();
        reachedThePoint = false;

        if ((transform.position - soundPosition).magnitude <= 1.5f)
        {
            soundPosition = Vector3.zero;
            reachedThePoint = true;
            if ((transform.position - _player.position).magnitude <= 2f)
            {
                isPlayerCatched = true;
            }
        } 
        if (soundPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, soundPosition, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(soundPosition, out hit, 5.0f, NavMesh.AllAreas))
                {
                    soundPosition = hit.position;
                }else
                    return (Vector3.zero,
                        lastViewDirection,
                        false,
                        false,
                        reachedThePoint,
                        isPlayerCatched);
                NavMesh.CalculatePath(transform.position, soundPosition, NavMesh.AllAreas, path);
            }

            lastViewDirection = Quaternion.LookRotation(path.corners[1] - path.corners[0]);
            return (path.corners[1] - path.corners[0],
                lastViewDirection, 
                true,
                false,
                reachedThePoint,
                isPlayerCatched);
        }

        if ((transform.position - walkingPoints[currentPoint].position).magnitude <= 1.5f)
        {
            currentPoint += 1;
            if (currentPoint == walkingPoints.Length)
            {
                currentPoint = 0;
            }

            reachedThePoint = true;
            if ((transform.position - _player.position).magnitude < 2f)
            {
                isPlayerCatched = true;
            }
        }
        
        NavMesh.CalculatePath(transform.position, walkingPoints[currentPoint].position, NavMesh.AllAreas, path);
        lastViewDirection = Quaternion.LookRotation(path.corners[1] - path.corners[0]);
        return (path.corners[1] - path.corners[0],
            lastViewDirection, 
            true,
            true,
            reachedThePoint,
            isPlayerCatched);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
            soundPosition = other.transform.position;
    }
}
