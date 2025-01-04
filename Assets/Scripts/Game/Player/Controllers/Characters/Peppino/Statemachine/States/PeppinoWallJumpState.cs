using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoWallJumpState : PeppinoBaseState
{
    public PeppinoWallJumpState(PeppinoController player) : base(player) { }

    PeppinoController.MoveForceMode previousMoveMode;
    Quaternion desiredRotation = Quaternion.identity;
    float time = 0;
    bool exitToParry = false;
    bool exitToWallClimb = false;
    bool jumped = false;
    bool unpressingJump = false;
    bool walljumpedAnimation = false;

    public override void EnterState()
    {
        //Intialize
        previousMoveMode = character.MovementMode;
        exitToParry = false;
        exitToWallClimb = false;
        jumped = false;
        unpressingJump = false;
        character.WallJumping = true;
        walljumpedAnimation = false;
        time = 0;
        //Animations
        character.AnimatorParameters.SetString("WallJump");
        //MovementMode
        character.MovementMode = PeppinoController.MoveForceMode.None;
        //Look at the opposite of the wall
        if (character.HitWall.normal != Vector3.zero) character.Rigidbody.transform.forward = character.HitWall.normal;
        PlayAudio(character.playerAudios.jump, false);
    }

    public override void Update()
    {
        UpdateInputLogic();
        MovementInput();
        JumpedAnimation();
    }

    protected override void UpdateInputLogic()
    {
        //Land State
        if (character.IsGrounded)
            character.StateManager.SwitchState(character.StateManager.States.LandState); //Land State
        //Body Slam
        if (character.player.Input.CrouchButtonInput.down && character.CurrentMachRun <= -1 && !character.AfterGrabMovement)
            character.StateManager.SwitchState(character.StateManager.States.BodySlamState);
        //Tirarse en picada
        if (character.player.Input.CrouchButtonInput.triggered && !character.IsWall && (character.CurrentMachRun > -1 || character.AfterGrabMovement))
            if (Time.time > character.WallClimbedLastTime + character.DivingDownwardsEnableDelayAfterWallClimbing)
                character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState);
        //Grab
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);
        //PARRY
        if (character.player.Input.ParryButtonInput.down)
        {
            exitToParry = true;
            character.StateManager.SwitchState(character.StateManager.States.ParryState);
        }

        time += Time.deltaTime;
        if(time > character.DelayToWallClimbAndUppercut)
        {
            //Wall Climb
            if ((character.IsWall && !character.IsGrounded && (character.player.Input.RunButtonInput.triggered) || (character.player.Input.GrabButtonInput.down && character.AfterGrabMovement)) && !character.AfterCeilingCrash)
            {
                exitToWallClimb = true;
                character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);
            }
            //Uppercut / Double Jump
            if (!character.IsGrounded && !character.AfterUppercut && character.player.Input.JumpButtonInput.down)
                character.StateManager.SwitchState(character.StateManager.States.UppercutState);
        }
    }

    public override void FixedUpdate()
    {
        CalculateJump();
        character.Rigidbody.AddForce(movementInput * character.MoveForceWhileWallJumping, ForceMode.Force);
        LimitVelocity();
        Rotate();
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void ExitState()
    {
        character.WallJumping = false;
        character.MovementMode = previousMoveMode;
        
        if (!exitToWallClimb)
        {
            character.CurrentSpeed = (character.Run1Speed + character.Run2Speed)/2;
            character.CurrentMachRun = 1;
        }
        if (!exitToParry) character.AfterUppercut = false;
    }


    //---------------------------------------//
    //-------------- Funciones --------------//

    private void CalculateJump()
    {
        if (!jumped)
        {
            character.Rigidbody.AddForce(character.Rigidbody.transform.forward * character.ForceForwardWallJump + character.Rigidbody.transform.up * character.ForceUpWallJump, ForceMode.Impulse);
            jumped = true;
        }
        if (unpressingJump)
        {
            character.Rigidbody.AddForce(-character.Rigidbody.transform.up * character.UnpressWallJumpForce, ForceMode.Impulse);
        }
    }

    private void Rotate()
    {

        Vector3 hDirection = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z).normalized;
        desiredRotation = Quaternion.LookRotation((hDirection + (movement.normalized * .6f)) / 2, Vector3.up);
        character.Rigidbody.rotation = Quaternion.Slerp(character.Rigidbody.rotation, desiredRotation, character.RotationSpeed * Time.fixedDeltaTime);
    }

    private void JumpedAnimation() 
    { 
        if(time > character.DelayToWallJumpedAnimation && !walljumpedAnimation)
        {
            character.AnimatorParameters.SetString("WallJumped");
            walljumpedAnimation = true;
        }
    }
}
