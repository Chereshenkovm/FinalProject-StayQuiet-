using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void AddProjectilePrefab(List<GameObject> itemArray)
    {
        itemArray.Add(projectilePrefab);
        Destroy(gameObject);
    }
}
