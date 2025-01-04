using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageTrigger : MonoBehaviour
{
    [SerializeField] private bool instaKill = false;
    [SerializeField] private float substractComboTime;

    enum TriggerType
    {
        OnTriggerEnter, OnTriggerStay, OverlapSphere
    }
    [SerializeField] private TriggerType triggerType;

    private float hitDelay = 0.1f;
    private bool canHit = true;

    [Header("Overlap Sphere")]
    [SerializeField] private Vector3 position;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;

    [Header("Parry")]
    [SerializeField] private Hitbox enemyHitbox;
    [SerializeField] private bool canBeParried = true;

    private void Awake()
    {
        gameObject.tag = "EnemyDamage";
        gameObject.layer = LayerMask.NameToLayer("EnemyDamage");
        canHit = true;
    }

    public void SetActive(bool enable) => gameObject.SetActive(enable);

    private void OnTriggerStay(Collider other)
    {
        if (triggerType != TriggerType.OnTriggerStay) return;

        if (canHit)
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PeppinoHitbox>().Hit(transform.position, substractComboTime, instaKill);
            StartCoroutine("HitReset");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType != TriggerType.OnTriggerEnter) return;

        if (other.CompareTag("Player"))
            other.GetComponent<PeppinoHitbox>().Hit(transform.position, substractComboTime, instaKill);
    }

    private void Update()
    {
        if(canHit)
        if (triggerType == TriggerType.OverlapSphere)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(position), radius, layerMask);
            if (colliders.Length > 0)
                foreach (Collider collider in colliders)
                {
                    collider.GetComponent<PeppinoHitbox>().Hit(transform.TransformPoint(position), substractComboTime, instaKill);
                    StartCoroutine("HitReset");
                }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(triggerType == TriggerType.OverlapSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(position), radius);
        }
    }

    IEnumerator HitReset()
    {
        canHit = false;
        yield return new WaitForSecondsRealtime(hitDelay);
        canHit = true;

    }

    //Sirve para el parry
    public int Parry(PlayerCombo playerCombo = null)
    {
        if(canBeParried)
        {
            if (enemyHitbox)
            {
                enemyHitbox.Kill(playerCombo, false, true);
                return 1; //Killed
            }
            return 0; //Only Parried
        }
        return -1; //No Killed No Parried
    }
}
