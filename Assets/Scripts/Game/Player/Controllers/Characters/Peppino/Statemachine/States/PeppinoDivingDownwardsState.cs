using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoDivingDownwardsState : PeppinoBaseState
{
    public PeppinoDivingDownwardsState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.AnimatorParameters.SetString("DivingDownwards");
        character.ForwardDamage.SetActive(true);
        character.BottomDamage.SetActive(true);
        character.BottomDamage.DestroyMetal = false;
        character.ForwardDamage.DestroyMetal = false;
        PlayAudio(character.playerAudios.dive, false);

        if (character.CurrentMachRun < 0) character.CurrentMachRun = 0;
    }

    public override void Update()
    {
        UpdateInputLogic();

        if(character.Rigidbody.velocity.y > -character.DivingDownwardsSpeed)
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, -character.DivingDownwardsSpeed - character.CurrentMachRun * character.DivingDownwardsMachRunMultiplier, character.Rigidbody.velocity.z);
    }

    protected override void UpdateInputLogic()
    {
        character.ForwardDamage.DestroyMetal = false;

        MovementInput();

        if (character.IsGrounded)
        {
            if(!character.player.Input.CrouchButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.LandState);
            else
                character.StateManager.SwitchState(character.StateManager.States.SlideState);
        }

        if (character.IsWall && !character.IsCeiling)
        {
            if (!character.IsGrounded && character.player.Input.RunButtonInput.triggered && character.CurrentMachRun >= 0)
                character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);
            else
                character.StateManager.SwitchState(character.StateManager.States.FallState);
        }

        if(!character.IsGrounded && character.player.Input.JumpButtonInput.down)
        {
            character.DivingSlamming = true;
            character.StateManager.SwitchState(character.StateManager.States.BodySlamState);
        }
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
        character.ForwardDamage.SetActive(false);
        character.BottomDamage.SetActive(false);
    }
}