using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingFallState : PeppinoBaseState
{
    public PeppinoHoldingFallState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingFall");
    }

    public override void Update()
    {
        UpdateInputLogic();
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        if (character.IsGrounded)
            character.StateManager.SwitchState(character.StateManager.States.HoldingIdleState);

        //Hacer un ataque slam para abajo mientras lo tenes agarrado
        if (!character.IsGrounded && character.player.Input.CrouchButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.HoldingPileDrivingState); 

        //Si apretás el botón de Atacar -> Holding AttackState
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.HoldingAttackState);

        //Si no está sosteniendo nada -> volver a la normalidad
        if (!character.Grab.Holding)
            character.StateManager.SwitchState(character.StateManager.States.FallState);
    }

    public override void FixedUpdate()
    {
        MovePlayerWithVelocityLimits();
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {

    }
}