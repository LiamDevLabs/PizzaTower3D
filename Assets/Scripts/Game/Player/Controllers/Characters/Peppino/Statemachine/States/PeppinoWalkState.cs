using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoWalkState : PeppinoBaseState
{
    public PeppinoWalkState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        character.CurrentMachRun = -1;
        character.CurrentSpeed = character.WalkSpeed;
        character.AnimatorParameters.SetString("Walk");
        character.ForwardDamage.SetActive(false);
        PlayAudio(character.playerAudios.walk, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Si no te mov�s -> Cambiar al IdleState
        if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
            character.StateManager.SwitchState(character.StateManager.States.IdleState);

        //Si apret�s el boton de correr -> Cambiar al RunState
        if (character.player.Input.RunButtonInput.triggered)
        {
            //Si no hay una pared -> correr
            if (!character.IsWall)
                character.StateManager.SwitchState(character.StateManager.States.Run0State);
            //Si hay una pared -> caminar
            else
                character.StateManager.SwitchState(character.StateManager.States.WalkState);
        }

        //Si est�s tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apret�s el bot�n de saltar -> saltar
            if (character.player.Input.JumpButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.JumpState);
        }
        //Si NO est�s tocando el suelo -> caer
        else
            character.StateManager.SwitchState(character.StateManager.States.FallState);

        //Si apret�s el bot�n de agacharse -> gatear
        if (character.player.Input.CrouchButtonInput.triggered)
            character.StateManager.SwitchState(character.StateManager.States.CrawlState);

        //Si apret�s el bot�n de Agarrar -> agarrar
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);

        //Si apret�s el bot�n de PARRY -> PARRY
        if (character.player.Input.ParryButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.ParryState);
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
        StopAudio();
    }
}
