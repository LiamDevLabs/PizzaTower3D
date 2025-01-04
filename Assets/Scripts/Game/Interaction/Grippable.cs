using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grippable : MonoBehaviour
{
    [field:Header("References")]
    [field: SerializeField] public Rigidbody ObjectToGrip { get; private set; }
    [field: SerializeField] public Hitbox Hitbox { get; private set; }

    [Header("ThrowDamage Trigger")]
    [SerializeField] private float minSpeedToDamage;
    [SerializeField] private Vector3 damagePosition;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask whatIsHitbox;

    //Este tiene que ser mas PEQUEÑO que el ThrowDamage Trigger
    [Tooltip("This has to be SMALLER than the ThrowDamage Trigger")]
    [Header("WallCrash Check")]
    [SerializeField] private Vector3 wallCheckPosition;
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private LayerMask whatIsWall;

    private bool killed = false;
    private bool canHit = false;
    private int startLayer;
    private float throwedTimer; //es para parchear un "bug"

    private PlayerCombo lastPlayerCombo;

    [field: SerializeField] public bool Gripped { get; private set; }
    [field: SerializeField] public bool Throwed { get; private set; }




    private void Awake()
    {
        gameObject.tag = "Grippable";
        gameObject.layer = LayerMask.NameToLayer("Grippable");
        startLayer = ObjectToGrip.gameObject.layer;
        canHit = false;
        killed = false;
        Gripped = false;
        Throwed = false;
        throwedTimer = 0;
        if (Hitbox) Hitbox.OnKill += Killed;
    }

    public void Grab(PlayerCombo playerCombo, Transform holdContainer)
    {
        lastPlayerCombo = playerCombo;
        Grab(holdContainer);
    }

    public void Grab(Transform holdContainer)
    {
        if (killed) return;
        ObjectToGrip.isKinematic = true;
        ObjectToGrip.transform.parent = holdContainer;
        ObjectToGrip.transform.localPosition = Vector3.zero;
        ObjectToGrip.transform.localEulerAngles = Vector3.zero;
        SetLayerMask(true);
        canHit = false;
        Gripped = true;
        Throwed = false;
    }

    public void PrepareToThrow(Transform prepareToThrowContainer, bool canHit = false)
    {
        if (killed) return;
        ObjectToGrip.transform.parent = prepareToThrowContainer;
        ObjectToGrip.transform.localPosition = Vector3.zero;
        ObjectToGrip.transform.localEulerAngles = Vector3.zero;
        this.canHit = canHit;
    }

    public void Throw(Vector3 force, bool canHit = true)
    {
        if (killed) return;
        ObjectToGrip.transform.parent = null;
        ObjectToGrip.isKinematic = false;
        ObjectToGrip.AddForce(force, ForceMode.Impulse);
        SetLayerMask(false);
        this.canHit = canHit;
        Gripped = false;
        Throwed = canHit;
        throwedTimer = 0;
    }

    public void Drop() => Throw(Vector3.zero);

    private void SetLayerMask(bool holding)
    {
        if (holding)
            ObjectToGrip.gameObject.layer = LayerMask.NameToLayer("Holding");
        else
            ObjectToGrip.gameObject.layer = startLayer;

        //if (colliders == null || colliders.Length == 0)
        //    rb.gameObject.layer = layer;
        //else
        //    foreach (Collider collider in colliders)
        //        collider.gameObject.layer = layer;
    }

    private void Killed()
    {
        Drop();
        killed = true;
        Gripped = false;
        Throwed = false;
    }

    private void Update()
    {
        //Al lanzar este objeto, que destruya los hitboxes con los que se choque
        if(canHit && Physics.CheckSphere(ObjectToGrip.transform.TransformPoint(damagePosition), damageRadius, whatIsHitbox))
        {
            Collider[] cols = Physics.OverlapSphere(ObjectToGrip.transform.TransformPoint(damagePosition), damageRadius, whatIsHitbox);
            foreach(Collider collider in cols) if(collider.gameObject != Hitbox.gameObject) collider.GetComponent<Hitbox>().Kill(lastPlayerCombo, false, true);
        }
    }

    private void LateUpdate()
    {
        //Chocarse con una pared -> Autodestruir
        if (canHit && Hitbox && Physics.CheckSphere(ObjectToGrip.transform.TransformPoint(wallCheckPosition), wallCheckRadius, whatIsWall))
            Hitbox.Kill(lastPlayerCombo, false, true);

        //Despues de tirarlo
        if (Throwed) throwedTimer += Time.deltaTime;
        if(throwedTimer > 0.25f && ObjectToGrip.velocity == Vector3.zero)
        {
            canHit = false;
            Throwed = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ObjectToGrip.transform.TransformPoint(damagePosition), damageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ObjectToGrip.transform.TransformPoint(wallCheckPosition), wallCheckRadius);
    }
}
