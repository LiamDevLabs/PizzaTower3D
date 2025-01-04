using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoSuperJumpPreparingState : PeppinoBaseState
{
    public PeppinoSuperJumpPreparingState(PeppinoController player) : base(player) { }

    private float time=0;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("PreparingSuperJumpIdle");
        character.CurrentSpeed = character.PreparingSuperJumpMoveSpeed;
        time = 0;

        PlayAudio(character.playerAudios.superjumpHold, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        time += Time.deltaTime;
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Animation
        if(movementInput == Vector3.zero)
            character.AnimatorParameters.SetString("PreparingSuperJumpIdle");
        else
            character.AnimatorParameters.SetString("PreparingSuperJumpWalk");

        //Fall
        if(!character.IsGrounded)
            character.StateManager.SwitchState(character.StateManager.States.FallState);

        //SUPER JUMP
        if (!character.player.Input.JumpButtonInput.triggered && time >= character.PreparingSuperJumpTime)
            character.StateManager.SwitchState(character.StateManager.States.SuperJumpState);
    }

    public override void FixedUpdate()
    {
        if (movementInput != Vector3.zero)
            MovePlayerWithVelocityLimits();
        else
        {
            float velocityX = Mathf.Lerp(character.Rigidbody.velocity.x, 0, Time.deltaTime * character.IdleTransitionSpeed);
            float velocityZ = Mathf.Lerp(character.Rigidbody.velocity.z, 0, Time.deltaTime * character.IdleTransitionSpeed);
            character.Rigidbody.velocity = new Vector3(velocityX, character.Rigidbody.velocity.y, velocityZ);
        }
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