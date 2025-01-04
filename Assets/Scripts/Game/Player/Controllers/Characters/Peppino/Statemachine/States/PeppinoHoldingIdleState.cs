using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingIdleState : PeppinoBaseState
{
    public PeppinoHoldingIdleState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingIdle");
        character.CurrentMachRun = -1;
        character.ForwardDamage.SetActive(false);
    }

    public override void Update()
    {
        UpdateInputLogic();

        float velocityX = Mathf.Lerp(character.Rigidbody.velocity.x, 0, Time.deltaTime * character.IdleTransitionSpeed);
        float velocityZ = Mathf.Lerp(character.Rigidbody.velocity.z, 0, Time.deltaTime * character.IdleTransitionSpeed);
        character.Rigidbody.velocity = new Vector3(velocityX, character.Rigidbody.velocity.y, velocityZ);
    }

    protected override void UpdateInputLogic()
    {
        //Si te mov�s ->  Holding WalkState
        if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
            character.StateManager.SwitchState(character.StateManager.States.HoldingWalkState);

        //Si est�s tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apret�s el bot�n de saltar -> Holding WalkState
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
        if(!character.Grab.Holding)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);

    }

    public override void FixedUpdate()
    {

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
