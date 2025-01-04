using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] PeppinoController controller;

    [SerializeField] private Vector3 position;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatCanParry;

    public bool Taunting { get; private set; }
    public bool Parrying { get; private set; }

    public void SetActive(bool enable)
    {
        if (!enable) Parrying = false;
        Taunting = enable;
        gameObject.SetActive(enable);
        enabled = enable;
    }

    private void Update()
    {
        if (!Parrying)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(position), radius, whatCanParry);
            if (colliders.Length > 0)
                foreach (Collider collider in colliders)
                {
                    if (!Parrying)
                    {
                        int parryValue = collider.GetComponent<EnemyDamageTrigger>().Parry(controller.player.Combo);
                        Parrying = parryValue != -1 ? true : false; //Killed or Only parried = True
                        firstParriedPosition = collider.transform.position;

                        if(parryValue == 0) //Only parried (noise boss)
                        {
                            controller.Hitbox.Inmortal();
                        }
                    }
                    else
                        collider.GetComponent<EnemyDamageTrigger>().Parry(controller.player.Combo);
                }
        }
        else Taunting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.TransformPoint(position), radius);
    }

    private Vector3 firstParriedPosition;
    public Vector3 GetFirstParriedPosition()
    {
        if (Parrying)
            return firstParriedPosition;
        else
            return Vector3.zero;
    }

    private void OnEnable()
    {
        Taunting = true;
    }

    private void OnDisable()
    {
        Taunting = false;
        Parrying = false;
    }
}
