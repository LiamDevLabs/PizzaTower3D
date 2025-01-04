using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoTechnicalProblemsState : PeppinoBaseState
{
    public PeppinoTechnicalProblemsState(PeppinoController player) : base(player) { }

    float time = 0;

    public override void EnterState()
    {
        time = 0;
        character.player.Input.PlayerInput.enabled = false;
        character.player.HUD.enabled = false;
        character.TechnicalProblems.SetActive(true);
        character.Rigidbody.isKinematic = true;
    }

    public override void Update()
    {
        UpdateInputLogic();

    }

    protected override void UpdateInputLogic()
    {
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {
        time += Time.deltaTime;
        if (time > character.TechnicalProblemsTime)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        character.Rigidbody.isKinematic = false;
        character.player.Input.PlayerInput.enabled = true;
        character.player.HUD.enabled = true;
        character.TechnicalProblems.SetActive(false);
        character.BodySlamming = false;
    }
}
