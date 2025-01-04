using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] protected Transform exitPoint;
    [SerializeField] protected bool x, y, z;
    [SerializeField] protected bool lookCamExitDirection, lookPlayerExitDirection;
    [SerializeField] protected float delay;
    [SerializeField] protected string animationName;
    [SerializeField] protected float animationDuration;

    bool teleporting;
    Transform player;
    PeppinoController peppinoController;

   //Events
    [SerializeField] protected UnityEvent OnTriggerUnityEvent;
    [SerializeField] protected UnityEvent OnTeleportUnityEvent;
    public delegate void TeleportAction();
    public event TeleportAction OnTeleport;
    public delegate void TriggerAction();
    public event TriggerAction OnTrigger;

    static public event TeleportAction OnAnyTeleport;
    static public event TriggerAction OnAnyTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!teleporting && other.CompareTag("Player"))
        {
            Trigger(other.transform);
        }
    }

    virtual protected void Trigger(Transform player)
    {
        teleporting = true;
        this.player = player;
        peppinoController = player.GetComponent<PeppinoHitbox>().Peppino;
        OnTrigger?.Invoke();
        OnAnyTrigger?.Invoke();
        OnTriggerUnityEvent?.Invoke();
        Invoke("Teleport", delay);
    }

    private void Teleport()
    {
        Vector3 position = player.position;
        if (x) position.x = exitPoint.position.x;
        if (y) position.y = exitPoint.position.y;
        if (z) position.z = exitPoint.position.z;
        StartCoroutine("TeleportCoroutine");
        player.position = position;
        teleporting = false;
    }

    IEnumerator TeleportCoroutine()
    {
        if (lookPlayerExitDirection)
            peppinoController.Rigidbody.transform.forward = exitPoint.forward;

        if (lookCamExitDirection)
        {
            yield return null;
            Transform playerContainer = peppinoController.transform;
            CinemachineFreeLook freelook = playerContainer.GetComponentInChildren<CinemachineFreeLook>();
            Transform cam = playerContainer.GetComponentInChildren<CinemachineBrain>().transform;
            float angle = Vector3.SignedAngle(cam.forward, exitPoint.forward, player.up);
            freelook.m_XAxis.Value += angle;
            freelook.GetComponent<LookInputManager>().LookForwardInFirstPerson();
        }

        if (!string.IsNullOrEmpty(animationName))
        {
            peppinoController.StateManager.SwitchToDoorState(animationName, animationDuration);
        }

        OnTeleport?.Invoke();
        OnAnyTeleport?.Invoke();
        OnTeleportUnityEvent?.Invoke();
        AfterTeleport();
    }

    virtual protected void AfterTeleport() { }
}
