using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoWallCrashState : PeppinoBaseState
{
    public PeppinoWallCrashState(PeppinoController player) : base(player) { }

    float time = 0;
    float currentMaxCrashTime = 1;

    public override void EnterState()
    {
        if (character.CurrentMachRun > 1)
        {
            //Animation
            character.AnimatorParameters.SetString("WallCrash2");
            //Audio
            PlayAudio(character.playerAudios.wallcrash2, false);
            //Time crash
            currentMaxCrashTime = character.WallCrash2Time;
            //Look at wall
            if (character.HitWall.normal != Vector3.zero) character.Rigidbody.transform.forward = -character.HitWall.normal;
            //Velocity Reset
            character.Rigidbody.velocity = new Vector3(0, character.Rigidbody.velocity.y, 0);
            //Backward impact force
            character.Rigidbody.AddForce(-character.Rigidbody.transform.forward * character.WallCrash2Force, ForceMode.Impulse);
        }
        else if (character.CurrentMachRun > -1)
        {
            //Animation
            character.AnimatorParameters.SetString("WallCrash1");
            //Audio
            PlayAudio(character.playerAudios.wallcrash1, false);
            //Time crash
            currentMaxCrashTime = character.WallCrash1Time;
            //Look at wall
            if (character.HitWall.normal != Vector3.zero) character.Rigidbody.transform.forward = -character.HitWall.normal;
        }
        else SwitchToMachRunBySpeed();

        character.CurrentSpeed = character.WalkSpeed;
        character.CurrentMachRun = -1;
        time = 0;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if (time > currentMaxCrashTime)
        {
            if (!character.IsCeiling)
                SwitchToCurrentMachRun();
            else
                character.StateManager.SwitchState(character.StateManager.States.CrouchState);
        }
    }

    protected override void UpdateInputLogic()
    {

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

    }
}