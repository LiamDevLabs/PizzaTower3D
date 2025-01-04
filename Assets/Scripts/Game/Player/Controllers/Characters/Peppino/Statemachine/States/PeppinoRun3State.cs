using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoRun3State : PeppinoBaseState
{
    public PeppinoRun3State(PeppinoController player) : base(player) { }

    public override void EnterState()
    {
        character.Animator.SetTrigger("Run3");
        character.AnimatorParameters.SetString("Run3");
        character.CurrentMachRun = 3;
        character.ForwardDamage.SetActive(true);
        character.ForwardDamage.DestroyEnemies = true;
        character.ForwardDamage.DestroyMetal = true;
        PlayAudio(character.playerAudios.mach3, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, character.MaxRunSpeed, Time.deltaTime * character.Run3TransitionSpeed);
    }

    protected override void UpdateInputLogic()
    {
        RunBaseInput();
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
        character.Rigidbody.useGravity = true;
        character.ForwardDamage.SetActive(false);
        character.IsRunning = false;
        character.ForwardDamage.DestroyEnemies = false;
        character.ForwardDamage.DestroyMetal = false;
    }
}
