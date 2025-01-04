using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHitbox : MonoBehaviour
{
    public delegate void HitAction();
    public event HitAction OnHit;
    [SerializeField] private UnityEvent OnHitUnityEvent;

    public void Hit()
    {
        OnHit?.Invoke();
        OnHitUnityEvent?.Invoke();
    }
}
