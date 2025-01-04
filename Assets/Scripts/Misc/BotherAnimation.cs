using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotherAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private string trigger;
    [SerializeField] private float delay;
    bool triggered = false;

    private void Awake()
    {
        triggered = false;
    }

    public void Trigger()
    {
        if (triggered) return;
        triggered = true;
        Invoke("Animate", delay);
    }

    private void Animate()
    {
        triggered = false;
        animator.SetTrigger(trigger);
    }
}
