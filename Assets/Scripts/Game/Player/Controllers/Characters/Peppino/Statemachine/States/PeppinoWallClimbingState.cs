using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoWallClimbingState : PeppinoBaseState
{
    public PeppinoWallClimbingState(PeppinoController player) : base(player) { }

    private float startSpeed;
    //Disable collisions
    private bool collisionDisabled = false;

    float time = 0;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("WallClimbing");
        PlayAudio(character.playerAudios.wallrun, true);
        startSpeed = character.CurrentSpeed;
        character.Rigidbody.useGravity = false;
        character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);
        character.TopDamage.SetActive(true);
        character.ForwardDamage.SetActive(false);
        collisionDisabled = false;
        time = 0;
    }

    public override void Update()
    {
        UpdateInputLogic();
        DisableForwardDamage();
        //DisableCollisions();
    }

    protected override void UpdateInputLogic()
    {
        //Si no estás tocando la pared O no apretás el botón de correr -> Correr / Caminar
        if (!character.IsWall || !character.player.Input.RunButtonInput.triggered && !character.player.Input.GrabButtonInput.triggered)
        {
            //Al llegar al final de la pared...
            if(character.player.Input.RunButtonInput.triggered)
            {
                //Set Horizontal Speed
                float horizontalAfterClimbingSpeed;
                if (character.CurrentSpeed > character.AfterClimbingMinForwardSpeed) horizontalAfterClimbingSpeed = character.CurrentSpeed;
                else horizontalAfterClimbingSpeed = character.AfterClimbingMinForwardSpeed;
                //Changing Velocity
                Vector3 afterClimbingVelocity = character.Rigidbody.transform.forward * horizontalAfterClimbingSpeed; //Forward Direction * Speed
                afterClimbingVelocity.y = character.AfterClimbingVerticalSpeed; //Vertical After Climbing velocity
                character.Rigidbody.velocity = afterClimbingVelocity; //Complete After Climbing velocity
            }

            //Correr / caminar
            SwitchToMachRunBySpeed(true);
        }

        //Si apretás el botón de saltar -> WallJump
        if (character.IsWall && character.player.Input.JumpButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.WallJumpState);

        //Chocarte con el techo
        if (character.IsCeiling)
            character.StateManager.SwitchState(character.StateManager.States.CeilingCrashState);
    }

    public override void FixedUpdate()
    {
        Vector3 direction = (Vector3.up - Vector3.Dot(Vector3.up, character.HitWall.normal) * character.HitWall.normal).normalized;
        if (!character.UseCurrentSpeedToClimb && character.CurrentSpeed < character.ClimbingSpeed)
        {
            character.Rigidbody.velocity = direction * character.ClimbingSpeed;
        }
        else
        {
            character.Rigidbody.velocity = direction * startSpeed;

        }
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        //if (!character.UseCurrentSpeedToClimb || character.CurrentSpeed <= character.ClimbingSpeed) character.CurrentSpeed = startSpeed;
        if (!character.UseCurrentSpeedToClimb && character.CurrentSpeed <= character.ClimbingSpeed) character.CurrentSpeed = character.ClimbingSpeed;
        character.Rigidbody.useGravity = true;
        character.WallClimbedLastTime = Time.time;
        character.TopDamage.SetActive(false);
        StopAudio();
        //DisableCollisions(false);
    }

    private void DisableForwardDamage()
    {
        time += Time.deltaTime;
        if (time > Time.deltaTime && time< 0.1)
        {
            character.ForwardDamage.DestroyEnemies = false;
            character.ForwardDamage.DestroyMetal = false;
        }
    }

    private void DisableCollisions(bool disable = true)
    {
        if (disable)
        {
            if (!collisionDisabled && character.Rigidbody.velocity.normalized.y > 0f)
            {
                character.Collider.isTrigger = true;
                collisionDisabled = true;
            }
        }
        else character.Collider.isTrigger = false;
       
    }

}
