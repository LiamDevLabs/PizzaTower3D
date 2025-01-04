using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDanceAnimation : MonoBehaviour
{
    Animator animator;

    [SerializeField] private float maxAnimations;
    private bool dance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Dance(bool dance)
    {
        if(dance)
            animator.SetInteger("Dance", (int)Random.Range(1, maxAnimations));
        else
            animator.SetInteger("Dance", 0);

    }
}
