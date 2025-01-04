using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool opened = false;

    private void Awake() => opened = false;
    public void Open()
    {
        opened = true;
        animator.SetTrigger("Open");
    }

    private void OnEnable()
    {
        if(opened) animator.SetTrigger("Open");
    }

}
