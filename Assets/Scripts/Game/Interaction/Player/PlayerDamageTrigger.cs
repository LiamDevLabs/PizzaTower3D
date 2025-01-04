using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{

    [SerializeField] PlayerBaseController controller;
    Rigidbody playerRB;

    enum TriggerType
    {
        OnTriggerEnter, OnTriggerStay, OverlapSphere, OverlapCapsule
    }
    [SerializeField] private TriggerType triggerType;


    [Header("Overlap Sphere")]
    [SerializeField] private Vector3 position;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;
    [Header("Overlap Capsule")]
    [SerializeField] private Vector3 position2;

    //-----------------------------------------------------------//
    public bool DestroyMetal { get; set; } = false;
    public bool DestroyEnemies { get; set; } = false;

    private void Awake()
    {
        playerRB = controller.GetComponentInChildren<Rigidbody>();
    }

    public void SetActive(bool enable)
    {
        if (!enable)
        {
            DestroyMetal = false;
            DestroyEnemies = false;
        }
        gameObject.SetActive(enable);
    }
    private void OnTriggerStay(Collider other)
    {
        if (triggerType != TriggerType.OnTriggerStay) return;

        if (other.CompareTag("Hitbox"))
            Damage(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType != TriggerType.OnTriggerEnter) return;

        if (other.CompareTag("Hitbox"))
            Damage(other);
    }

    private void Update()
    {
        if (triggerType == TriggerType.OverlapSphere)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(position), radius, layerMask);
            if (colliders.Length > 0)
                foreach (Collider collider in colliders)
                    Damage(collider);
        }
        if (triggerType == TriggerType.OverlapCapsule)
        {
            Collider[] colliders = Physics.OverlapCapsule(transform.TransformPoint(position), transform.TransformPoint(position2), radius, layerMask);
            if (colliders.Length > 0)
                foreach (Collider collider in colliders)
                    Damage(collider);
        }
    }

    private void Damage(Collider collider)
    {
        if(controller.Health.activated && collider.TryGetComponent(out BossHitbox boss)) boss.Hit();
        else collider.GetComponent<Hitbox>().Kill(controller.player.Combo, DestroyMetal, DestroyEnemies, playerRB);
    }

    private void OnDrawGizmosSelected()
    {
        if(triggerType == TriggerType.OverlapSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(position), radius);
        }
        if (triggerType == TriggerType.OverlapCapsule)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(position), radius);
            Gizmos.DrawWireSphere(transform.TransformPoint(position2), radius);
            Gizmos.DrawRay(transform.TransformPoint(position) + transform.up * radius, transform.TransformPoint(position2)- transform.TransformPoint(position));
            Gizmos.DrawRay(transform.TransformPoint(position) - transform.up * radius, transform.TransformPoint(position2) - transform.TransformPoint(position));
            Gizmos.DrawRay(transform.TransformPoint(position) + transform.right * radius, transform.TransformPoint(position2) - transform.TransformPoint(position));
            Gizmos.DrawRay(transform.TransformPoint(position) - transform.right * radius, transform.TransformPoint(position2) - transform.TransformPoint(position));
        }
    }
}
