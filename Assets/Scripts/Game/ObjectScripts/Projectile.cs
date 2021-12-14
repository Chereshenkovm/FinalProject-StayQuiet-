using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 20;
    }
}
