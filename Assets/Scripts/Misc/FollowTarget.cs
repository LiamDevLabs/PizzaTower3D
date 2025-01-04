using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowTarget : MonoBehaviour
{
    enum UpdateType
    {
        Update, FixedUpdate, LateUpdate,
    }

    enum MoveType
    {
        RigidbodyForce, RigidbodyVelocity, RigidbodyHorizontalVelocity, TransformPosition
    }

    enum ReachMinDistance
    {
        BackwardForce, ZeroVelocity, BackwardVelocity, BackwardPosition, Nothing
    }

    [Header("References")]
    [SerializeField] public Rigidbody rb;

    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool targetIsPlayer;
    [SerializeField] private Vector3 targetOffset;

    [Header("Movement")]
    [SerializeField] private UpdateType updateType;
    [SerializeField] private MoveType moveType;
    [SerializeField] private ReachMinDistance onReachMinDistance;
    public float speed;
    [SerializeField] private float minDistance;
    [SerializeField] private bool normalizeVelocity = true;
    [SerializeField] private bool x, y, z = true;

    [Header("Events")]
    [SerializeField] UnityEvent OnReachMinDistance;
    [SerializeField] UnityEvent OnLeaveMinDistance;
    private bool reached = false;

    IEnumerator Start()
    {
        if (!rb) TryGetComponent(out rb);
        yield return null;
        if (targetIsPlayer) target = FindObjectOfType<PeppinoController>().Rigidbody.transform;
        reached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateType == UpdateType.Update)
            Move();
    }

    private void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
            Move();
    }

    private void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
            Move();
    }

    void Move()
    {
        if (target == null) return;

        //Target Point
        Vector3 targetPointAux = target.position + target.TransformVector(targetOffset);

        Vector3 targetPoint = transform.position;
        if (x) targetPoint.x = targetPointAux.x;
        if (y) targetPoint.y = targetPointAux.y;
        if (z) targetPoint.z = targetPointAux.z;

        //Distance
        float distance = Vector3.Distance(transform.position, targetPoint);

        //Direction
        Vector3 direction;
        if (normalizeVelocity)
            direction = (targetPoint - transform.position).normalized;
        else
            direction = (targetPoint - transform.position);

        //Move forward
        if (distance > minDistance)
        {
            switch (moveType)
            {
                case MoveType.RigidbodyForce:
                    rb.AddForce(direction * speed);
                    break;
                case MoveType.RigidbodyVelocity:
                    rb.velocity = direction * speed;
                    break;
                case MoveType.RigidbodyHorizontalVelocity:
                    rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
                    break;
                case MoveType.TransformPosition:
                    transform.position += direction * speed * Time.deltaTime;
                    break;
            }

            if (reached) OnLeaveMinDistance.Invoke();
            reached = false;
        }
        else
        {
            //Move backward / braking / nothing
            switch (onReachMinDistance)
            {
                case ReachMinDistance.BackwardForce:
                    rb.AddForce(-direction * speed);
                    break;
                case ReachMinDistance.ZeroVelocity:
                    rb.velocity = Vector3.zero;
                    break;
                case ReachMinDistance.BackwardVelocity:
                    rb.velocity = -direction * speed;
                    break;
                case ReachMinDistance.BackwardPosition:
                    transform.position += -direction * speed * Time.deltaTime;
                    break;
                default:
                    break;
            }

            if (!reached) OnReachMinDistance.Invoke();
            reached = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position + target.TransformVector(targetOffset), 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position + target.TransformVector(targetOffset), minDistance);

    }
}
