using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Trigger : MonoBehaviour
{
    [SerializeField] private string tagTarget;
    [SerializeField] private Type type;
    [SerializeField] private bool once = true;
    private bool triggered = false;

    [SerializeField] private UnityEvent OnTriggerUnityEvent;

    public delegate void TriggerAction();
    public event TriggerAction OnTrigger;

    enum Type
    {
        OnTriggerEnter, OnTriggerExit, OnTriggerStay, OnCollisionEnter, OnCollisionExit, OnCollisionStay
    }

    private void OnTriggerEnter(Collider other)
    {
        if (once && triggered) return;
        if (type != Type.OnTriggerEnter) return;
        if (!other.CompareTag(tagTarget)) return;
        Triggered();
    }

    private void OnTriggerExit(Collider other)
    {
        if (once && triggered) return;
        if (type != Type.OnTriggerExit) return;
        if (!other.CompareTag(tagTarget)) return;
        Triggered();

    }

    private void OnTriggerStay(Collider other)
    {
        if (once && triggered) return;
        if (type != Type.OnTriggerStay) return;
        if (!other.CompareTag(tagTarget)) return;
        Triggered();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (once && triggered) return;
        if (type != Type.OnCollisionEnter) return;
        if (!collision.gameObject.CompareTag(tagTarget)) return;
        Triggered();

    }

    private void OnCollisionExit(Collision collision)
    {
        if (once && triggered) return;
        if (type != Type.OnCollisionExit) return;
        if (!collision.gameObject.CompareTag(tagTarget)) return;
        Triggered();

    }

    private void OnCollisionStay(Collision collision)
    {
        if (once && triggered) return;
        if (type != Type.OnCollisionStay) return;
        if (!collision.gameObject.CompareTag(tagTarget)) return;
        Triggered();
    }

    private void Triggered()
    {
        OnTrigger?.Invoke();
        OnTriggerUnityEvent.Invoke();
        triggered = true;
    }
}
