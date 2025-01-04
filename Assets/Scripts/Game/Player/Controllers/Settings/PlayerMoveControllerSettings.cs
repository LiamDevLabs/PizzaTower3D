using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Move Controller Settings", menuName = "Liam/Player/Player Controllers/Player Move Controller")]
public class PlayerMoveControllerSettings : ScriptableObject
{
    public bool cam = false;

    [Header("Movement")]
    public float speed = 3f;
    public float jumpSpeed = 3.5f;
    public float gravity = 9.81f;

    [Header("Ground check")]
    public LayerMask groundCheckLayer;
    public float groundCheckRadius, groundCheckDistance;
}
