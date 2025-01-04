using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingWalkState : PeppinoBaseState
{
    public PeppinoHoldingWalkState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.CurrentMachRun = -1;
        character.CurrentSpeed = character.WalkSpeed;
        character.AnimatorParameters.SetString("HoldingWalk");
        character.ForwardDamage.SetActive(false);
        PlayAudio(character.playerAudios.walk, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Si no te mov�s -> Cambiar al Holding IdleState
        if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
            character.StateManager.SwitchState(character.StateManager.States.HoldingIdleState);

        //Si est�s tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apret�s el bot�n de saltar -> Holding JumpState
            if (character.player.Input.JumpButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.HoldingJumpState);

            //Si apret�s el bot�n de agacharse -> para dejar el objeto
            if (character.player.Input.CrouchButtonInput.down)
            {
                character.Grab.PrepareToThrow(false);
                character.Grab.Throw(character.Rigidbody.velocity, false);
                character.StateManager.SwitchState(character.StateManager.States.IdleState);
            }
        }
        //Si NO est�s tocando el suelo -> Holding FallState
        else
            character.StateManager.SwitchState(character.StateManager.States.HoldingFallState);

        //Si apret�s el bot�n de Atacar -> Holding AttackState
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.HoldingAttackState);

        //Si no est� sosteniendo nada -> volver a la normalidad
        if (!character.Grab.Holding)
            character.StateManager.SwitchState(character.StateManager.States.WalkState);
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
        StopAudio();
    }
}
