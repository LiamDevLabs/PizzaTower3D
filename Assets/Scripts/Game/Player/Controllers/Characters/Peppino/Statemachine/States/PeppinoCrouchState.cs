using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoCrouchState : PeppinoBaseState
{
    public PeppinoCrouchState(PeppinoController player) : base(player) { }

    bool crawl = false;

    public override void EnterState()
    {
        crawl = false;
        character.AnimatorParameters.SetString("Crouch");
        CrouchPlayer(true);
        character.CurrentMachRun = -1;
    }

    public override void Update()
    {
        UpdateInputLogic();

        float velocityX = Mathf.Lerp(character.Rigidbody.velocity.x, 0, Time.deltaTime * character.IdleTransitionSpeed);
        float velocityZ = Mathf.Lerp(character.Rigidbody.velocity.z, 0, Time.deltaTime * character.IdleTransitionSpeed);
        character.Rigidbody.velocity = new Vector3(velocityX, character.Rigidbody.velocity.y, velocityZ);
    }

    protected override void UpdateInputLogic()
    {
        //Si dej�s apretado el bot�n de agacharse -> 
        if (character.player.Input.CrouchButtonInput.triggered)
        {
            //Si te mov�s -> Gatear
            if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                Crawl();
        }
        //Si solt�s el bot�n de agacharse y no hay techo -> 
        else if(!character.IsCeiling)
        {
            //Si no te mov�s -> Cambiar al IdleState
            if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                character.StateManager.SwitchState(character.StateManager.States.IdleState);
            //Si te mov�s -> Cambiar al WalkState
            else
                character.StateManager.SwitchState(character.StateManager.States.WalkState);
        }
        //No est�s apretando el bot�n de agacharte PERO hay techo -> 
        else
            //Te mov�s -> Gatear
            if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                Crawl();


        //Si est�s tocando el suelo ->
        if (character.IsGrounded)
        {
            //Si apret�s el bot�n de saltar y no hay techo -> saltar
            if (character.player.Input.JumpButtonInput.triggered && !character.IsCeiling)
                character.StateManager.SwitchState(character.StateManager.States.CrouchJumpState);
        }
        //Si NO est�s tocando el suelo -> caer
        else
            character.StateManager.SwitchState(character.StateManager.States.FallState);
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
        CrouchPlayer(crawl);
    }

    private void Crawl()
    {
        character.StateManager.SwitchState(character.StateManager.States.CrawlState);
        crawl = true;
    }
}