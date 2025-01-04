using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoFallState : PeppinoBaseState
{
    public PeppinoFallState(PeppinoController player) : base(player) { }

    bool exitToParry = false;
    float time = 0;


    public override void EnterState()
    {
        exitToParry = false;
        time = 0;

        if (!character.AfterGrabMovement)
        {
            if (GetMachRunBySpeed(character.CurrentSpeed) < 0)
            {
                character.AnimatorParameters.SetString("Falling");
            }
        }
        else
        {
            character.AnimatorParameters.SetString("GrabFall");
        }
    }

    public override void Update()
    {
        UpdateInputLogic();
        AfterCeilingCrash();
    }


    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Land State
        if (character.IsGrounded)
            character.StateManager.SwitchState(character.StateManager.States.LandState);

        //Wall Climb
        //if (character.IsWall && !character.IsGrounded && (character.player.Input.RunButtonInput.triggered && character.CurrentMachRun >= 0) || (character.player.Input.AttackButtonInput.down && character.AfterGrabMovement))
        if ((character.IsWall && !character.IsGrounded && (character.player.Input.RunButtonInput.triggered) || (character.player.Input.GrabButtonInput.down && character.AfterGrabMovement)) && !character.AfterCeilingCrash)
            character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);
        //Body Slam
        if (character.player.Input.CrouchButtonInput.down && character.CurrentMachRun <= -1 && !character.AfterGrabMovement)
            character.StateManager.SwitchState(character.StateManager.States.BodySlamState);
        //Tirarse en picada
        if (character.player.Input.CrouchButtonInput.triggered && !character.IsWall && (character.CurrentMachRun > -1 || character.AfterGrabMovement))
            if(Time.time > character.WallClimbedLastTime + character.DivingDownwardsEnableDelayAfterWallClimbing) 
                character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState);
        //Grab
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);
        //Uppercut / Double Jump
        if (!character.IsGrounded && !character.AfterUppercut && character.player.Input.JumpButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.UppercutState);
        //PARRY
        if (character.player.Input.ParryButtonInput.down)
        {
            exitToParry = true;
            character.StateManager.SwitchState(character.StateManager.States.ParryState); 
        }
            
        //Daño
        character.ForwardDamage.SetActive(character.CurrentMachRun > -1);
        character.ForwardDamage.DestroyEnemies = character.CurrentMachRun >= 2;
        character.ForwardDamage.DestroyMetal = character.CurrentMachRun >= 2;
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
        character.AfterGrabMovement = false;
        character.WallJumping = false;
        character.AfterCeilingCrash = false;
        if (!exitToParry) character.AfterUppercut = false;
    }

    private void AfterCeilingCrash()
    {
        time += Time.deltaTime;
        if (character.AfterCeilingCrash && time > character.TimeToWallClimbAfterCeilingCrash)
        {
            character.AfterCeilingCrash = false;
            time = 0;
        }
    }
}