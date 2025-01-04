using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingPileDrivingState : PeppinoBaseState
{
    public PeppinoHoldingPileDrivingState(PeppinoController player) : base(player) { }

    float yAngle;
    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingSpin");
        character.CurrentMachRun = -1;
        character.ForwardDamage.SetActive(false);
        character.BottomDamage.SetActive(true);
        character.BottomDamage.DestroyEnemies = true;
        //Angle
        yAngle = character.Rigidbody.transform.localEulerAngles.y;
        //Prepare to Throw
        character.Grab.PrepareToThrow(true);
        PlayAudio(character.playerAudios.startSlam, false);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //Rotación
        yAngle += character.HoldingPileDrivingSpinSpeed * Time.deltaTime;
        character.Rigidbody.transform.localEulerAngles = new Vector3(character.Rigidbody.transform.localEulerAngles.x, yAngle, character.Rigidbody.transform.localEulerAngles.z);

        //Chocarse con la pared
        if (character.IsWall)
        {
            //Kill Holding Object
            character.Grab.KillHoldingObject();
            //Look at wall
            if (character.HitWall.normal != Vector3.zero) character.Rigidbody.transform.forward = -character.HitWall.normal;
            //Backward impact force
            character.Rigidbody.AddForce(-character.Rigidbody.transform.forward * character.HoldingKnockbackForce, ForceMode.Impulse);
            //Switch to idle state
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
        }

        //Tocar el piso
        if (character.IsGrounded)
        {
            //Kill Holding Object
            character.Grab.KillHoldingObject();
            //Land
            character.StateManager.SwitchState(character.StateManager.States.LandState);
            //Set speed
            character.CurrentSpeed = character.WalkSpeed;
        }

        //Si no está sosteniendo nada -> volver a la normalidad
        if (!character.Grab.Holding)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
    }

    protected override void UpdateInputLogic()
    {
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.HoldingAttackState);
    }

    public override void FixedUpdate()
    {
        //Falling speed
        character.Rigidbody.AddForce(-character.Rigidbody.transform.up * character.HoldingPileDrivingFallingForce * Time.fixedDeltaTime);
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        character.BottomDamage.SetActive(false);
    }
}
