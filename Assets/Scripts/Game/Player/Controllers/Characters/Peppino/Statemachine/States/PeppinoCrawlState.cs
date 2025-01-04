using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoCrawlState : PeppinoBaseState
{
    public PeppinoCrawlState(PeppinoController player) : base(player) { }

    bool idleCrouch = false;

    public override void EnterState()
    {
        idleCrouch = false;
        character.CurrentMachRun = -1;
        character.CurrentSpeed = character.WalkSpeed;
        CrouchPlayer(true);
        character.AnimatorParameters.SetString("Crawl");
    }

    public override void Update()
    {
        UpdateInputLogic();
    }

    protected override void UpdateInputLogic()
    {
        Vector3 input = new Vector3(character.player.Input.MoveInput.x, 0, character.player.Input.MoveInput.y);
        if (input == Vector3.zero && character.GrabToPeppinoDireccion)
            movementInput = character.Rigidbody.transform.forward.normalized;
        else
            movementInput = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * new Vector3(character.player.Input.MoveInput.x, 0, character.player.Input.MoveInput.y);
        movement = movementInput * character.CurrentSpeed;

        //Si te dej�s apretado el bot�n de agacharse -> 
        if (character.player.Input.CrouchButtonInput.triggered)
        {
            //Si no te mov�s -> Cambiar al CrouchState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                IdleCrouch();
        }
        //Si solt�s el bot�n de agacharse y no hay techo -> 
        else if(!character.IsCeiling)
        {
            //Si no te mov�s -> Cambiar al IdleState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                character.StateManager.SwitchState(character.StateManager.States.IdleState);
            //Si te mov�s -> Cambiar al WalkState
            else
                character.StateManager.SwitchState(character.StateManager.States.WalkState);
        }
        //No est�s apretando el bot�n de agacharte PERO hay techo -> 
        else
            //Te qued�s quieto -> CrouchState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
            IdleCrouch();

        //Si est�s tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apret�s el bot�n de saltar y no hay techo -> saltar
            if (character.player.Input.JumpButtonInput.triggered && !character.IsCeiling)
                character.StateManager.SwitchState(character.StateManager.States.CrouchJumpState);
        }
        //Si NO est�s tocando el suelo -> caer
        else
            character.StateManager.SwitchState(character.StateManager.States.FallState);
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
        CrouchPlayer(idleCrouch);
    }

    private void IdleCrouch()
    {
        character.StateManager.SwitchState(character.StateManager.States.CrouchState);
        idleCrouch = true;
    }
}
