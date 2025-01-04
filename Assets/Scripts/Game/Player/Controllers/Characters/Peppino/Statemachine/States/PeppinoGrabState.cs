using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoGrabState : PeppinoBaseState
{
    public PeppinoGrabState(PeppinoController player) : base(player) { }

    float time = 0;

    public override void EnterState()
    {
        if (character.IsGrounded) 
            character.AnimatorParameters.SetString("Grab");
        else 
            character.AnimatorParameters.SetString("AirGrab");

        time = 0;
        character.AfterGrabMovement = false;
        character.ForwardDamage.SetActive(true);
        character.ForwardDamage.DestroyMetal = false;
        character.ForwardDamage.DestroyEnemies = false;
        character.Grab.SetActive(true);

        //Min to Add Force
        if (character.CurrentSpeed <= character.MinGrabCurrentSpeed)
        {
            character.CurrentSpeed = character.MinGrabCurrentSpeed;
            //Add Force
            if(!character.GrabToPeppinoDireccion)
                character.Rigidbody.AddForce(Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * Vector3.forward * character.MinGrabCurrentSpeed, ForceMode.VelocityChange);
            else
                character.Rigidbody.AddForce(character.Rigidbody.transform.forward * character.MinGrabCurrentSpeed, ForceMode.VelocityChange);

        }

        //Mirar en direccion de la camara
        if (!character.GrabToPeppinoDireccion) character.Rigidbody.transform.eulerAngles = new Vector3(character.Rigidbody.transform.eulerAngles.x, character.player.Cam.transform.eulerAngles.y, character.Rigidbody.transform.eulerAngles.z);

        //Audio
        PlayAudio(character.playerAudios.grab, false);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.ForwardDamage.SetActive(true);

        //Tiempo para terminar
        time += Time.deltaTime;
        if (time > character.GrabTime && character.IsGrounded)
            SwitchToMachRunBySpeed(true);
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        if (character.IsGrounded)
        {
            //Grab en el piso (animación)
            character.AnimatorParameters.SetString("Grab");

            //Salto con grab
            if (character.player.Input.JumpButtonInput.triggered)
            {
                character.AfterGrabMovement = true;
                character.StateManager.SwitchState(character.StateManager.States.JumpState);
            }
            //Agacharse con grab
            if (character.player.Input.CrouchButtonInput.triggered)
            {
                character.AfterGrabMovement = true;
                character.StateManager.SwitchState(character.StateManager.States.SlideState);
            }

            //Chocar pared
            if (character.IsWallCrash)
                character.StateManager.SwitchState(character.StateManager.States.WallCrashState);
        }
        else
        {
            //Grab en el aire (animación)
            character.AnimatorParameters.SetString("GrabAir");

            //Trepar pared
            if (character.IsWall)
                character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);

            //Tirarse en picada
            if (character.player.Input.CrouchButtonInput.down)
                character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState);
        }

        //Holding state (agarrar a un enemigo)
        if (character.Grab.Holding)
        {
            if(character.CurrentMachRun > 0)
                character.StateManager.SwitchState(character.StateManager.States.HoldingSpinState);
            else
                character.StateManager.SwitchState(character.StateManager.States.HoldingIdleState);
        }
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
        character.ForwardDamage.SetActive(false);
        character.Grab.SetActive(false);
    }
}
