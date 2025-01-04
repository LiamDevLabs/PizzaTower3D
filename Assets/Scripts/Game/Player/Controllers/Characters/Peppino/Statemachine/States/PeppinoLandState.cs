using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoLandState : PeppinoBaseState
{
    public PeppinoLandState(PeppinoController player) : base(player) { }

    float time = 0;
    float currentMaxLandTime = 1;

    public override void EnterState()
    {
        if (!character.BodySlamming)
        {
            if (character.CurrentMachRun <= character.MaxMachRunToLand)
            {
                character.AnimatorParameters.SetString("Landing");
                currentMaxLandTime = character.LandingTime;
            }
            else
            {
                SwitchToCurrentMachRun();
            }
        }
        else
        {
            character.AnimatorParameters.SetString("LandingBodySlam");
            PlayAudio(character.playerAudios.endSlam, false);
            currentMaxLandTime = character.BodySlamLandingTime;
            character.CurrentSpeed = character.WalkSpeed;
            character.CurrentMachRun = -1;
            character.BottomDamage.SetActive(false);
        }

        time = 0;
    }

    public override void Update()
    {
        UpdateInputLogic();
        time += Time.deltaTime;
        if (time > currentMaxLandTime)
            SwitchToMachRunBySpeed();
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Si hiciste un bodyslam tenes que apretar en el momento JUSTO para saltar. Si NO es un bodyslam solamente podes dejar apretado el boton de saltar para hacerlo
        if (character.player.Input.JumpButtonInput.triggered)
        {
            if (!character.BodySlamming || character.player.Input.JumpButtonInput.down)
            {
                character.AfterSlam = character.BodySlamming;
                character.StateManager.SwitchState(character.StateManager.States.JumpState);
            }
        }

        //Si apretás el botón de Agarrar -> agarrar
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);
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
        character.BottomDamage.SetActive(false);
        character.BodySlamming = false;
        character.HeavyBodySlamming = false;
        character.DivingSlamming = false;
        if (character.AfterSlam) character.Rigidbody.velocity = Vector3.zero;
        StopAudio();
    }
}