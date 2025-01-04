using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingSpinState : PeppinoBaseState
{
    public PeppinoHoldingSpinState(PeppinoController player) : base(player) { }

    float time = 0;
    float yAngle, yStartAngle;
    float velocityX, velocityZ, startVelocityX, startVelocityZ;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingSpin");
        character.CurrentMachRun = -1;
        character.ForwardDamage.SetActive(false);
        //Velocity
        velocityX = character.Rigidbody.velocity.x;
        velocityZ = character.Rigidbody.velocity.z;
        startVelocityX = velocityX;
        startVelocityZ = velocityZ;
        //Angle
        yAngle = character.Rigidbody.transform.localEulerAngles.y;
        yStartAngle = yAngle;
        //Time
        time = 0;
        //Prepare to Throw
        character.Grab.PrepareToThrow(true);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //Tiempo
        time += Time.deltaTime;

        //Movimiento
        velocityX = Mathf.Lerp(character.Rigidbody.velocity.x, 0, Time.deltaTime * character.HoldingSpinBrakingSpeed);
        velocityZ = Mathf.Lerp(character.Rigidbody.velocity.z, 0, Time.deltaTime * character.HoldingSpinBrakingSpeed);
        character.Rigidbody.velocity = new Vector3(velocityX, character.Rigidbody.velocity.y, velocityZ);

        //Rotation
        yAngle += Mathf.Abs(character.Rigidbody.velocity.magnitude * character.HoldingSpinRotationSpeed) * Time.deltaTime;
        character.Rigidbody.transform.localEulerAngles = new Vector3(character.Rigidbody.transform.localEulerAngles.x, yAngle, character.Rigidbody.transform.localEulerAngles.z);

        //Terminar Spin
        if (character.Rigidbody.velocity.magnitude <= character.HoldingSpinMinSpeedToStop)
        {
            character.Grab.BackToHold();
            character.StateManager.SwitchState(character.StateManager.States.HoldingIdleState);
        }

        //Chocarse con la pared
        if (character.IsWall)
        {
            character.Grab.KillHoldingObject();
            //Look at wall
            if (character.HitWall.normal != Vector3.zero) character.Rigidbody.transform.forward = -character.HitWall.normal;
            //Backward impact force
            character.Rigidbody.AddForce(-character.Rigidbody.transform.forward * character.HoldingKnockbackForce, ForceMode.Impulse);
            //Switch to idle state
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
        }

        //Hacer un ataque slam para abajo mientras lo tenes agarrado
        if (!character.IsGrounded && character.player.Input.CrouchButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.HoldingPileDrivingState);

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

    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        //character.Rigidbody.transform.eulerAngles = new Vector3(character.Rigidbody.transform.eulerAngles.x, character.player.Cam.transform.eulerAngles.y, character.Rigidbody.transform.eulerAngles.z);
    }
}
