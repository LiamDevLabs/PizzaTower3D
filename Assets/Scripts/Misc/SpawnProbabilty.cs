using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProbabilty : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] private float probability;
    [SerializeField] private GameObject spawnObject;
    void Start()
    {
        if (Random.Range(0, 99) <= probability)
        {
            Instantiate(spawnObject, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
