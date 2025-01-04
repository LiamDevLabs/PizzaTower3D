using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoIdleState : PeppinoBaseState
{
    public PeppinoIdleState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.AnimatorParameters.SetString("Idle");
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
        //Si te movés -> 
        if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
        {
            //Si no apretás el botón de correr -> Cambiar al WalkState
            if (!character.player.Input.RunButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.WalkState);
            //Si apretás el botón de correr -> Cambiar al RunState
            else
            {
                //Si no hay una pared -> correr
                if (!character.IsWall)
                    character.StateManager.SwitchState(character.StateManager.States.Run0State);
                //Si hay una pared -> caminar
                else
                    character.StateManager.SwitchState(character.StateManager.States.WalkState);
            }
        }

        //Si estás tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apretás el botón de saltar -> saltar
            if (character.player.Input.JumpButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.JumpState);

            //Para que no deslice en los slopes)
            if(character.IsSlope)
                character.Rigidbody.velocity = Vector3.zero;
        }
        //Si NO estás tocando el suelo -> caer
        else
            character.StateManager.SwitchState(character.StateManager.States.FallState);

        //Si apretás el botón de agacharse -> agacharse
        if (character.player.Input.CrouchButtonInput.triggered)
            character.StateManager.SwitchState(character.StateManager.States.CrouchState);

        //Si apretás el botón de Agarrar -> agarrar
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);

        //Si apretás el botón de PARRY -> PARRY
        if (character.player.Input.ParryButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.ParryState);
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

        if (character.Rigidbody.velocity.magnitude < 0.5f)
            character.CurrentSpeed = 0;
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {

    }
}
