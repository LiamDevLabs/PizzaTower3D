using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoSuperJumpState : PeppinoBaseState
{
    public PeppinoSuperJumpState(PeppinoController player) : base(player) { }

    private float time=0;
    private float heavyBodySlamTime = 0;
    float yAngle;
    private bool falling = false;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("SuperJump");
        time = 0;
        character.TopDamage.SetActive(true);
        character.TopDamage.DestroyMetal = true;
        character.TopDamage.DestroyEnemies = true;
        falling = false;
        //Angle
        yAngle = character.Rigidbody.transform.localEulerAngles.y;

        PlayAudio(character.playerAudios.superjumpReleased, false);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //SuperJump Time
        time += Time.deltaTime;
        if(time < character.SuperJumpTime)
            //Super jumping speed
            character.Rigidbody.velocity = new Vector3(0, Mathf.Lerp(character.SuperJumpMaxSpeed, 0, time / character.SuperJumpTime), 0); 

        //Rotation
        yAngle += Mathf.Abs(character.Rigidbody.velocity.y * character.SuperJumpSpinSpeed) * Time.deltaTime;
        character.Rigidbody.transform.localEulerAngles = new Vector3(0, yAngle, 0);

        //Land / Slam
        if(falling && character.IsGrounded)
        {
            character.BodySlamming = true;
            character.StateManager.SwitchState(character.StateManager.States.LandState);
            character.AnimatorParameters.SetString("SuperLand");
        }

        //Ceiling Crash
        if (character.IsCeiling)
        {
            character.BodySlamming = false;
            character.HeavyBodySlamming = false;
            character.StateManager.SwitchState(character.StateManager.States.CeilingCrashState);
        }
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        if(character.player.Input.GrabButtonInput.down && !character.IsGrounded)
        {
            character.BodySlamming = false;
            character.HeavyBodySlamming = false;
            character.StateManager.SwitchState(character.StateManager.States.ShoulderBashState);
        }
    }

    public override void FixedUpdate()
    {
        MovePlayerWithVelocityLimits();
    }

    public override void LateUpdate()
    {
        //Falling
        if (!falling && time > character.SuperJumpTime && character.Rigidbody.velocity.y < 5f)
        {
            falling = true;
            character.AnimatorParameters.SetString("SuperFall");
            character.TopDamage.SetActive(false);
            character.BottomDamage.SetActive(true);
            character.BottomDamage.DestroyMetal = true;
            character.BottomDamage.DestroyEnemies = true;
            character.BodySlamming = true;
            character.HeavyBodySlamming = true;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        StopAudio();
        character.TopDamage.SetActive(false);
        character.BottomDamage.SetActive(false);
    }
}