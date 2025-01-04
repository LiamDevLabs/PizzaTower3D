using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingJumpState : PeppinoBaseState
{
    public PeppinoHoldingJumpState(PeppinoController player) : base(player) { }

    bool jumped = false;
    bool unpressingJump = false;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingJump");
        jumped = false;
        unpressingJump = false;
    }

    public override void Update()
    {
        UpdateInputLogic();
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        unpressingJump = !character.player.Input.JumpButtonInput.triggered;

        if (character.Rigidbody.velocity.y < 0)
            character.StateManager.SwitchState(character.StateManager.States.HoldingFallState); //HoldingFall State

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
        if (!jumped)
        {
            character.Rigidbody.AddForce(character.Rigidbody.transform.up * character.ForceJump, ForceMode.Impulse);
            jumped = true;
        }

        if(unpressingJump)
            character.Rigidbody.AddForce(-character.Rigidbody.transform.up * character.UnpressJumpForce, ForceMode.Impulse);

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
