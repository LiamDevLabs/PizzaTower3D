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

        //Si te dejás apretado el botón de agacharse -> 
        if (character.player.Input.CrouchButtonInput.triggered)
        {
            //Si no te movés -> Cambiar al CrouchState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                IdleCrouch();
        }
        //Si soltás el botón de agacharse y no hay techo -> 
        else if(!character.IsCeiling)
        {
            //Si no te movés -> Cambiar al IdleState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                character.StateManager.SwitchState(character.StateManager.States.IdleState);
            //Si te movés -> Cambiar al WalkState
            else
                character.StateManager.SwitchState(character.StateManager.States.WalkState);
        }
        //No estás apretando el botón de agacharte PERO hay techo -> 
        else
            //Te quedás quieto -> CrouchState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
            IdleCrouch();

        //Si estás tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apretás el botón de saltar y no hay techo -> saltar
            if (character.player.Input.JumpButtonInput.triggered && !character.IsCeiling)
                character.StateManager.SwitchState(character.StateManager.States.CrouchJumpState);
        }
        //Si NO estás tocando el suelo -> caer
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
