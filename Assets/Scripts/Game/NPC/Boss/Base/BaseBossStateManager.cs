using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossStateManager
{
    protected BossBaseState currentState;

    public void Start(BossBaseState startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (currentState != null)
            currentState.OnCollisionEnter(collision);
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    public void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdate();
    }

    public void LateUpdate()
    {
        if (currentState != null)
            currentState.LateUpdate();
    }

    public void SwitchState(BossBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState();
    }

    public string DebugCurrentState()
    {
        if (currentState != null)
            return currentState.ToString();
        return "No State";
    }
    

}
