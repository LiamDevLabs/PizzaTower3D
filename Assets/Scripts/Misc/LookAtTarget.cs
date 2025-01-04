using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    enum Axis
    {
        x, y, z, negativeX, negativeY, negativeZ, none
    }

    enum OffsetType
    {
        worldOffset, localOffset, fromThisOffset
    }

    [SerializeField] private Transform target;
    [SerializeField] private bool targetIsPlayer = true;
    public bool onlyHorizontalMove = false;
    [SerializeField] private Axis worldUp;
    [SerializeField] private Axis ninetyRotation;
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private OffsetType offsetType;
    public float speed;

    Quaternion lookRotation = Quaternion.identity;

    IEnumerator Start()
    {
        yield return null;

        if(targetIsPlayer)
        target = FindObjectOfType<PeppinoController>().Rigidbody.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        //Target point
        Vector3 targetPoint;
        switch (offsetType)
        {
            case OffsetType.worldOffset: targetPoint = target.position + targetOffset; break;
            case OffsetType.localOffset: targetPoint = targetPoint = target.position + target.TransformVector(targetOffset); break;
            case OffsetType.fromThisOffset: targetPoint = target.position + transform.TransformVector(targetOffset); break;
            default: targetPoint = target.position; break;
        }

        //Look Direction
        Vector3 lookDirection = targetPoint - transform.position;

        //Only Horizontal Move
        if (onlyHorizontalMove) lookDirection.y = 0;

        //Up direction
        switch (worldUp)
        {
            case Axis.x:
                lookRotation = Quaternion.LookRotation(lookDirection, Vector3.right);
                break;
            case Axis.y:
                lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                break;
            case Axis.z:
                lookRotation = Quaternion.LookRotation(lookDirection, Vector3.forward);
                break;
            case Axis.negativeX:
                lookRotation = Quaternion.LookRotation(lookDirection, -Vector3.right);
                break;
            case Axis.negativeY:
                lookRotation = Quaternion.LookRotation(lookDirection, -Vector3.up);
                break;
            case Axis.negativeZ:
                lookRotation = Quaternion.LookRotation(lookDirection, -Vector3.forward);
                break;
        }

        //Ninety rotation
        switch (ninetyRotation)
        {
            case Axis.x:
                lookRotation *= Quaternion.AngleAxis(90, Vector3.right);
                break;
            case Axis.y:
                lookRotation *= Quaternion.AngleAxis(90, Vector3.up);
                break;
            case Axis.z:
                lookRotation *= Quaternion.AngleAxis(90, Vector3.forward);
                break;
            case Axis.negativeX:
                lookRotation *= Quaternion.AngleAxis(90, -Vector3.right);
                break;
            case Axis.negativeY:
                lookRotation *= Quaternion.AngleAxis(90, -Vector3.up);
                break;
            case Axis.negativeZ:
                lookRotation *= Quaternion.AngleAxis(90, -Vector3.forward);
                break;
            default:
                break;
        }

        //Smooth look at
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);
    }

    private void LateUpdate()
    {

    }
}
