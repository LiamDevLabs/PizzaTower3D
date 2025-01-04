using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoJumpState : PeppinoBaseState
{
    public PeppinoJumpState(PeppinoController player) : base(player) { }

    bool jumped = false;
    bool unpressingJump = false;
    bool switchedToFallState = false;

    float currentForceJump;

    float time = 0;

    public override void EnterState()
    {
        //ForceJump
        currentForceJump = character.ForceJump;
        time = 0;

        if (!character.AfterGrabMovement && !character.AfterSlam)
        {
            //Animations
            switch (GetMachRunBySpeed(character.CurrentSpeed))
            {
                case -1:
                    character.AnimatorParameters.SetString("Jump");
                    break;
                case 0:
                    character.AnimatorParameters.SetString("Jump1");
                    break;
                case 1:
                    character.AnimatorParameters.SetString("Jump1");
                    break;
                case 2:
                    character.AnimatorParameters.SetString("Jump2");
                    break;
            }
        }

        //After Grab
        if(character.AfterGrabMovement) 
            character.AnimatorParameters.SetString("GrabJump");

        //After Slam Landing
        if(character.AfterSlam)
        {
            character.AnimatorParameters.SetString("JumpAfterSlam");
            currentForceJump = character.BodySlamLandingJump;
        }

        //Audio
        if(character.CurrentMachRun <= 1)
            PlayAudio(character.playerAudios.jump, false);

        jumped = false;
        unpressingJump = false;
        switchedToFallState = false;
    }

    public override void Update()
    {
        UpdateInputLogic();
        time += Time.deltaTime;
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        unpressingJump = !character.player.Input.JumpButtonInput.triggered;

        if (jumped && character.Rigidbody.velocity.y < 0)
        {
            switchedToFallState = true;
            if (character.CurrentMachRun == 2) character.AnimatorParameters.SetString("Run2");
            //if(character.AfterSlam) character.AnimatorParameters.SetString("Falling");
            character.StateManager.SwitchState(character.StateManager.States.FallState); //Fall State
        }

        if (character.IsGrounded && time > character.DelayToDetectGroundAfterJump)
            character.StateManager.SwitchState(character.StateManager.States.LandState); //Land State

        if (character.IsWall && !character.IsGrounded && (character.player.Input.RunButtonInput.triggered) || (character.player.Input.GrabButtonInput.down && character.AfterGrabMovement))
            character.StateManager.SwitchState(character.StateManager.States.WallClimbingState); //Wall CLimb

        if (character.player.Input.CrouchButtonInput.triggered && character.CurrentMachRun <= -1 && !character.AfterGrabMovement)
            character.StateManager.SwitchState(character.StateManager.States.BodySlamState); //Body Slam

        if (character.player.Input.CrouchButtonInput.triggered && (character.CurrentMachRun > -1 || character.AfterGrabMovement))
            character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState); //Tirarse en picada

        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState); //Grab

        if(!character.IsGrounded && jumped && character.player.Input.JumpButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.UppercutState); //Uppercut / Double Jump

        if (character.player.Input.ParryButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.ParryState); //PARRY

        //Daño
        character.ForwardDamage.SetActive(character.CurrentMachRun > -1);
        character.ForwardDamage.DestroyEnemies = character.CurrentMachRun >= 2;
        character.ForwardDamage.DestroyMetal = character.CurrentMachRun >= 2;
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
        character.ForwardDamage.SetActive(false);
        if (!switchedToFallState) character.AfterGrabMovement = false;
        character.AfterSlam = false;
    }
}
