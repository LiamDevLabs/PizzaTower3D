using UnityEngine;
public abstract class BossBaseState
{
    public abstract void EnterState();
    public abstract void Update();
    public abstract void LateUpdate();
    public abstract void FixedUpdate();
    public abstract void OnCollisionEnter(Collision collision);
    public abstract void ExitState();
}