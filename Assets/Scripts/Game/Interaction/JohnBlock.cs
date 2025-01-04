using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnBlock : MonoBehaviour
{
    //--------Static--------
    static public bool Toggled { get; private set; } = false;
    static private event ToggleAction OnToggle;

    static public void Toogle()
    {
        Toggled = !Toggled;
        OnToggle?.Invoke();
    }

    static private void OnLevelWasLoaded() => Toggled = false;

    //----------------------

    [SerializeField] private bool solid = false;
    private Animator animator;
    private MeshCollider meshCollider;
    private delegate void ToggleAction();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meshCollider = GetComponent<MeshCollider>();
        animator.SetBool("Sleep", !solid); 
        meshCollider.enabled = solid;
        OnToggle += ChangeState;
    }

    private void ChangeState()
    {
        solid = !solid;
        animator.SetBool("Sleep", !solid); 
        meshCollider.enabled = solid;
    }

    private void OnEnable() => animator.SetBool("Sleep", !solid);


    private void OnDrawGizmos()
    {
        if (solid)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.black;

        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void OnDestroy()
    {
        OnToggle -= ChangeState;
    }
}
