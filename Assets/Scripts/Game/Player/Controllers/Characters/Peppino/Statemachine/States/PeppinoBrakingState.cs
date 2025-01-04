using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoBrakingState : PeppinoBaseState
{
    public PeppinoBrakingState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
        MovementInput();
        character.Animator.SetBool("Braking", true);
        character.AnimatorParameters.SetString("Braking");
        PlayAudio(character.playerAudios.runBreak, true);
    }

    public override void Update()
    {
        UpdateInputLogic();
        character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, 0, Time.deltaTime * character.BrakingSpeed);
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Si la rapidez bajo a 0 -> Cambiar al IdleState
        if (character.CurrentSpeed < 0.1f)
        {
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
            character.Animator.SetBool("Braking", false);
        }
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
    }
}
