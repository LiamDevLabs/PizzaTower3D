using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoTurnAroundState : PeppinoBaseState
{
    public PeppinoTurnAroundState(PeppinoController player) : base(player) { }

    private float time = 0;
    private float previousSpeed;
    private float brakingSpeed;

    public override void EnterState()
    {
        MovementInput();
        if(character.CurrentMachRun < 2)
            character.AnimatorParameters.SetString("TurnAround1");
        else
            character.AnimatorParameters.SetString("TurnAround2");
        previousSpeed = character.CurrentSpeed;
        brakingSpeed = character.CurrentSpeed * character.TurnAroundBrakingSpeedPercentage;
        time = 0;

        PlayAudio(character.playerAudios.turnAround, true);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //Frenar hasta alcanzar la velocidad minima
        if (character.CurrentSpeed > brakingSpeed)
        {
            time += Time.deltaTime / character.TurnAroundBrakingTime;
            character.CurrentSpeed = Mathf.Lerp(character.CurrentSpeed, brakingSpeed, time);
        }
        //Se alcanzó la velocidad minima... Salir del State
        else
        {
            if (previousSpeed < character.Run3Speed)
                character.CurrentSpeed = previousSpeed;
            else
                character.CurrentSpeed = character.Run2Speed;

            SetSpeedToCurrentMachSpeed();
            SwitchToMachRunBySpeed();
        }
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();



        if (!character.player.Input.RunButtonInput.triggered)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
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
