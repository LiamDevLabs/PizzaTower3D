using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] private PlayerBaseController controller;

    [SerializeField] private Transform holdContainer;
    [SerializeField] private Transform prepareToThrowContainer;

    private Grippable holdingObject;
    public bool Holding { get; private set; } 

    [Header("Trigger")]
    [SerializeField] private Vector3 position;
    [SerializeField] private float radius;
    [SerializeField] private float range;
    [SerializeField] private LayerMask whatCanHold;

    public void SetActive(bool enable)
    {
        gameObject.SetActive(enable);
        enabled = enable;
    }

    private void Update()
    {
        if (!Holding)
        {
            if (Physics.SphereCast(transform.TransformPoint(position), radius, transform.forward, out RaycastHit hit, range, whatCanHold)) //Hay que poner una capa mas o algo mas en esta parte para que no pueda agarrar cosas a través de las paredes
            {
                holdingObject = hit.collider.GetComponent<Grippable>();
                holdingObject.Grab(controller.player.Combo, holdContainer);
                Holding = true;
            }
        }
        else if (holdingObject == null) Holding = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.TransformPoint(position), radius);
        Gizmos.DrawRay(transform.TransformPoint(position) + transform.up * radius, transform.forward * range);
        Gizmos.DrawRay(transform.TransformPoint(position) - transform.up * radius, transform.forward * range);
        Gizmos.DrawRay(transform.TransformPoint(position) + transform.right * radius, transform.forward * range);
        Gizmos.DrawRay(transform.TransformPoint(position) - transform.right * radius, transform.forward * range);
        Gizmos.DrawWireSphere(transform.TransformPoint(position) + transform.forward*range, radius);
    }



    public void PrepareToThrow(bool canHit = false)
    {
        if (holdingObject == null) { Holding = false; return; }
        //---------------/
        holdingObject.PrepareToThrow(prepareToThrowContainer, canHit);
    }

    public void BackToHold()
    {
        if (holdingObject == null) { Holding = false; return; }
        //---------------/
        if (Holding) holdingObject.Grab(controller.player.Combo, holdContainer);
    }

    public void Throw(Vector3 force, bool canHit = true)
    {
        if (holdingObject == null) { Holding = false; return; }
        //---------------/
        holdingObject.Throw(force, canHit);
        holdingObject = null;
        Holding = false;
    }

    public void KillHoldingObject()
    {
        if (holdingObject == null) { Holding = false; return; }
        //---------------/
        holdingObject.Drop();
        holdingObject.Hitbox.Kill(controller.player.Combo, true, true);
        Holding = false;
        holdingObject = null;
    }

}
