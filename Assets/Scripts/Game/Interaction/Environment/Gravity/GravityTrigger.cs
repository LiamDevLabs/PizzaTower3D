using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    [SerializeField] private Transform originGravity;
    [SerializeField] private float force;

    private PeppinoController player;



    private void Awake()
    {
        if(!originGravity) originGravity = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PeppinoHitbox>().Peppino;
            player.MovementMode = PeppinoController.MoveForceMode.OnPlanet;
            other.attachedRigidbody.useGravity = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 direction = (originGravity.position - player.Rigidbody.transform.position).normalized;
            player.Rigidbody.AddForce(direction * force);
            player.Rigidbody.transform.rotation = Quaternion.FromToRotation(-player.Rigidbody.transform.up, direction) * player.Rigidbody.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Rigidbody.useGravity = true;
            player.transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);
            player.MovementMode = PeppinoController.MoveForceMode.OnlyForwardFlatVelocity2;
            player = null;
        }
    }
}
