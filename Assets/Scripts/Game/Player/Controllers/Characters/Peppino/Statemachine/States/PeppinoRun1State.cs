using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoRun1State : PeppinoBaseState
{
    public PeppinoRun1State(PeppinoController player) : base(player) { }

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("Run1");
        character.CurrentMachRun = 1;
        character.ForwardDamage.SetActive(true);
        PlayAudio(character.playerAudios.mach1, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, character.MaxRunSpeed, Time.deltaTime * character.Run1TransitionSpeed);
    }

    protected override void UpdateInputLogic()
    {
        RunBaseInput();

        if (character.CurrentSpeed >= character.Run2Speed)
            character.StateManager.SwitchState(character.StateManager.States.Run2State);
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
        character.IsRunning = false;
        character.Rigidbody.useGravity = true;
    }
}
