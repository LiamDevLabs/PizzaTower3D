using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoSlideState : PeppinoBaseState
{
    public PeppinoSlideState(PeppinoController player) : base(player) { }

    bool crouch = false;

    float time = 0;

    public override void EnterState()
    {
        crouch = false;
        CrouchPlayer(true);
        character.ForwardDamage.SetActive(true);

        if (!character.AfterGrabMovement)
            character.AnimatorParameters.SetString("Rolling");
        else
        {
            //Min to Add Force
            if (character.CurrentSpeed <= character.GrabSlideBoost)
            {
                character.CurrentSpeed = character.GrabSlideBoost;
                character.Rigidbody.AddForce(Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * Vector3.forward * character.GrabSlideBoost, ForceMode.VelocityChange);
            }
            character.AnimatorParameters.SetString("GrabCrouch");
        }

        PlayAudio(character.playerAudios.slide, true);
    }

    public override void Update()
    {
        UpdateInputLogic();

        DisableMachDamage();

        time += Time.deltaTime;
        if (!character.AfterGrabMovement)
            if (time >= character.TimeRollingBeforeSlide && character.CurrentMachRun >= 2)
                character.AnimatorParameters.SetString("Slide");
    }

    protected override void UpdateInputLogic()
    {
        ////Movement input
        //if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
        //{
        //    movementInput = new Vector3(character.player.Input.MoveInput.x * character.sideMoveSensibility, 0, character.player.Input.MoveInput.y);
        //}
        //else movementInput = new Vector3(0, 0, 1);
        //movement = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;
        MovementInput();

        //Si soltás el botón de agacharse y no hay techo -> 
        if (!character.player.Input.CrouchButtonInput.triggered && !character.IsCeiling)
        {
            //Si no te movés -> Frenar
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput) && !character.player.Input.RunButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.BrakingState);
            //Si te movés -> Correr
            else SwitchToCurrentMachRun();
        }

        //Si NO estás tocando el suelo ->
        if (!character.IsGrounded)
        {
            //Si estás apretando el boton de agacharse -> Tirarse para abajo
            if (character.player.Input.CrouchButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState);
            else
            //Si NO estás apretando el boton de agacharse Y NO estas tocando el techo -> Caer
            if (!character.IsCeiling)
            character.StateManager.SwitchState(character.StateManager.States.FallState);
        }
        //Si estás en el piso
        else
            //Si tocás la pared -> chocarte
            if (character.IsWallCrash)
            character.StateManager.SwitchState(character.StateManager.States.WallCrashState);

        //SUPER JUMP
        if(!character.IsCeiling && character.CurrentMachRun >= 2 && character.player.Input.JumpButtonInput.triggered)
            character.StateManager.SwitchState(character.StateManager.States.SuperJumpPreparingState);
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
        time = 0;
        character.ForwardDamage.SetActive(false);
        character.AfterGrabMovement = false;
        CrouchPlayer(crouch);
        StopAudio();
    }

    private void DisableMachDamage()
    {
        if (time > Time.deltaTime && time < 0.1)
        {
            character.ForwardDamage.DestroyEnemies = false;
            character.ForwardDamage.DestroyMetal = false;
        }
    }

    private void Crouch()
    {
        character.StateManager.SwitchState(character.StateManager.States.CrouchState);
        crouch = true;
    }
}
