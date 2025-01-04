using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoShoulderBashState : PeppinoBaseState
{
    public PeppinoShoulderBashState(PeppinoController player) : base(player) { }

    private float time=0;

    private bool shoulderBash = false;

    public override void EnterState()
    {
        character.CurrentSpeed = character.ShoulderBashForceSpeed;
        character.AnimatorParameters.SetString("ShoulderBashPreparing");
        time = 0;
        character.ForwardDamage.SetActive(true);
        character.ForwardDamage.DestroyEnemies = true;
        character.ForwardDamage.DestroyMetal = true;
        shoulderBash = false;
        PlayAudio(character.playerAudios.shoulderBash, false);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //ShoulderBash Time
        time += Time.deltaTime;

        if(time > character.ShoulderBashPreparingTime)
        {
            if (!shoulderBash)
            {
                shoulderBash = true;
                character.Rigidbody.velocity = Vector3.zero;
                character.Rigidbody.AddForce(Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * Vector3.forward * character.ShoulderBashForceSpeed, ForceMode.Impulse);
                character.AnimatorParameters.SetString("ShoulderBash");
            }
        }
        else
        {
            //Preparing in air
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);

            //Mirar en direccion de la camara
            character.Rigidbody.transform.eulerAngles = new Vector3(character.Rigidbody.transform.eulerAngles.x, character.player.Cam.transform.eulerAngles.y, character.Rigidbody.transform.eulerAngles.z);
        }
    }

    protected override void UpdateInputLogic()
    {
        if (shoulderBash)
            MovementInput();

        //Downward
        if (!character.IsGrounded && character.player.Input.CrouchButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.DivingDownwardsState);

        //Land
        if (character.IsGrounded)
            character.StateManager.SwitchState(character.StateManager.States.LandState);

        //Wall
        if (character.IsWall)
            character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);
    }

    public override void FixedUpdate()
    {
        if(shoulderBash)
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
        character.ForwardDamage.SetActive(false);
    }
}