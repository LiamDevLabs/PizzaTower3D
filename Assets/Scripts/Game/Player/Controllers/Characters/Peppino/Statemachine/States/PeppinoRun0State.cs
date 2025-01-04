using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoRun0State : PeppinoBaseState
{
    public PeppinoRun0State(PeppinoController player) : base(player) { }

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("Run0");
        character.CurrentMachRun = 0;
        character.ForwardDamage.SetActive(true);
        PlayAudio(character.playerAudios.mach0, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, character.MaxRunSpeed, Time.deltaTime * character.Run0TransitionSpeed);
    }

    protected override void UpdateInputLogic()
    {
        RunBaseInput();

        if (character.CurrentSpeed >= character.Run1Speed)
            character.StateManager.SwitchState(character.StateManager.States.Run1State);
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
        character.ForwardDamage.SetActive(false);
        character.IsRunning = false;
        character.Rigidbody.useGravity = true;
    }
}
