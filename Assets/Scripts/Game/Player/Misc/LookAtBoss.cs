using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookAtBoss : MonoBehaviour
{
    enum UpdateType
    {
        Update,LateUpdate,FixedUpdate
    }

    [SerializeField] private CinemachineFreeLook freeLook;
    public Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;
    public float speed;
    //[SerializeField] private UpdateType updateType;
  
    float GetAngle()
    {
        Vector3 targetDirection = (target.position - player.position).normalized;
        targetDirection.y = 0;
        Vector3 camDirection = cam.forward;
        camDirection.y = 0;
        return Vector3.SignedAngle(camDirection, targetDirection, player.up);
    }

    private void Update()
    {
        if (!freeLook || !target || !player || !cam || speed <= 0 /*|| updateType != UpdateType.Update*/) return;
        freeLook.m_XAxis.Value += GetAngle() * Time.deltaTime * speed;
    }

    /*void FixedUpdate()
    {
        if (!freeLook || !target || !player || !cam || updateType != UpdateType.FixedUpdate) return;
        freeLook.m_XAxis.Value += GetAngle() * Time.fixedDeltaTime * speed;

    }*/

    /*private void LateUpdate()
    {
        if (!freeLook || !target || !player || !cam || updateType != UpdateType.LateUpdate) return;
        freeLook.m_XAxis.Value += GetAngle() * Time.deltaTime * speed;
    }*/
}
