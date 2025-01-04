using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoCeilingCrashState : PeppinoBaseState
{
    public PeppinoCeilingCrashState(PeppinoController player) : base(player) { }

    float time = 0;

    public override void EnterState()
    {
        //Animation
        character.AnimatorParameters.SetString("CeilingCrash");
        //Freeze player
        character.Rigidbody.isKinematic = true;

        character.CurrentSpeed = character.WalkSpeed;
        character.CurrentMachRun = -1;
        time = 0;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if (time > character.CeilingCrashTime)
        {
            character.StateManager.SwitchState(character.StateManager.States.FallState);
            character.AfterCeilingCrash = true;
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
        //Unfreeze player
        character.Rigidbody.isKinematic = false;
    }
}