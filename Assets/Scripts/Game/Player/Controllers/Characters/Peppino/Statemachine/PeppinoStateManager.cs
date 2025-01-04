using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeppinoStateManager 
{
    public PeppinoStates States { get; private set; }
    private PeppinoBaseState currentState;

    public delegate void SwitchStateAction();
    public event SwitchStateAction OnSwitchState;

    public PeppinoStateManager (PeppinoController player)
    {
        States = new PeppinoStates(player);
    }

    public void Start()
    {
        //Comenzar el State de inicio
        currentState = States.IdleState;
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

    public void SwitchState(PeppinoBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState();
        OnSwitchState?.Invoke();
    }


    public void SwitchToDoorState(string animation, float duration)
    {
        States.DoorState.animationName = animation;
        States.DoorState.duration = duration;
        SwitchState(States.DoorState);
    }

    public string GetCurrentState()
    {
        if (currentState != null)
            return currentState.ToString();
        return "No State";
    }
}
