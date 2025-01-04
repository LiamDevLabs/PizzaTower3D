using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoDamagedState : PeppinoBaseState
{
    public PeppinoDamagedState(PeppinoController player) : base(player) { }

    float time = 0;

    public override void EnterState()
    {
        time = 0;
        character.Rigidbody.useGravity = true;
        character.Damaged = true;
        character.AnimatorParameters.SetString("Damaged");
        character.CurrentMachRun = -1;
        character.Grab.Throw(Vector3.zero, false);
        PlayAudio(character.playerAudios.damage, false);
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5f && character.IsGrounded) 
            character.StateManager.SwitchState(character.StateManager.States.LandState);
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
        character.CurrentSpeed = character.WalkSpeed;
        character.Damaged = false;
    }
}