using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private PlayerBaseController controller;
    Rigidbody playerRB;

    private void Awake()
    {
        playerRB = controller.GetComponentInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
            other.GetComponent<Collectable>().Collect(controller, playerRB);
    }
}
