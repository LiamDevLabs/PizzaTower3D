using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoUppercutState : PeppinoBaseState
{
    public PeppinoUppercutState(PeppinoController player) : base(player) { }

    float time = 0;

    bool canJump = false;
    bool jumped = false;
    bool unpressingJump = false;

    public override void EnterState()
    {
        //ForceJump
        character.AnimatorParameters.SetString("Uppercut");

        //Damage (se habilita al saltar)
        character.TopDamage.SetActive(false);

        canJump = false;
        jumped = false;
        unpressingJump = false;

        time = 0;

        PlayAudio(character.playerAudios.uppercut, true);
    }

    public override void Update()
    {
        UpdateInputLogic();

        time += Time.deltaTime;
        canJump = time > character.UppercutJumpDelay;
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        unpressingJump = !character.player.Input.JumpButtonInput.triggered;

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

        if (character.player.Input.ParryButtonInput.down)
        {
            character.StateManager.SwitchState(character.StateManager.States.ParryState); //PARRY
            character.AfterUppercut = true;
        }
    }

    public override void FixedUpdate()
    {
        if (canJump)
        {
            if (!jumped)
            {
                //Damage
                character.TopDamage.SetActive(true);
                character.TopDamage.DestroyEnemies = true;
                //Jump
                character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x * character.UpperCutVelocityMultiplier, 0, character.Rigidbody.velocity.z * character.UpperCutVelocityMultiplier);
                character.Rigidbody.AddForce(character.Rigidbody.transform.up * character.ForceJumpUppercut, ForceMode.Impulse);
                jumped = true;
            }

            if (unpressingJump && character.Rigidbody.velocity.y > 0)
                character.Rigidbody.AddForce(-character.Rigidbody.transform.up * character.UnpressJumpForce, ForceMode.Impulse);
        }
        else
        {
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, character.Rigidbody.velocity.y * character.UppercutFallMultiplier, character.Rigidbody.velocity.z);
        }


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
        character.TopDamage.SetActive(false);
    }
}
