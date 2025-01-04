using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoCrouchJumpState : PeppinoBaseState
{
    public PeppinoCrouchJumpState(PeppinoController player) : base(player) { }

    bool jumped = false;
    bool unpressingJump = false;

    float currentForceJump;

    public override void EnterState()
    {
        CrouchPlayer(true);

        //ForceJump
        currentForceJump = character.ForceJump;

        character.AnimatorParameters.SetString("CrouchJump");

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

        if (jumped && character.Rigidbody.velocity.y < 1f && character.IsGrounded)
        {
            character.StateManager.SwitchState(character.StateManager.States.CrouchState); 
        }
    }

    public override void FixedUpdate()
    {
        if (!jumped)
        {
            character.Rigidbody.AddForce(character.Rigidbody.transform.up * currentForceJump, ForceMode.Impulse);
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
        CrouchPlayer(false);
    }
}
