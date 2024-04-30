using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDeleteAfterUse : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject prefab;
    void Start()
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
