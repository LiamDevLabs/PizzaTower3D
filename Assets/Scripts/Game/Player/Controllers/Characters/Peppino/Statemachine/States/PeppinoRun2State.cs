using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoRun2State : PeppinoBaseState
{
    public PeppinoRun2State(PeppinoController player) : base(player) { }

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("Run2");
        character.CurrentMachRun = 2;
        character.ForwardDamage.SetActive(true);
        character.ForwardDamage.DestroyEnemies = true;
        character.ForwardDamage.DestroyMetal = true;
        PlayAudio(character.playerAudios.mach2, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, character.MaxRunSpeed, Time.deltaTime * character.Run2TransitionSpeed);
    }

    protected override void UpdateInputLogic()
    {
        RunBaseInput();
        if (character.CurrentSpeed >= character.Run3Speed)
            character.StateManager.SwitchState(character.StateManager.States.Run3State);
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
        character.Rigidbody.useGravity = true;
        character.ForwardDamage.SetActive(false);
        character.ForwardDamage.DestroyEnemies = false;
        character.ForwardDamage.DestroyMetal = false;
        character.IsRunning = false;
        if (!jumpedWhileRunning) StopAudio();
    }
}
