using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoDoorState : PeppinoBaseState
{
    public PeppinoDoorState(PeppinoController player) : base(player) { }

    public string animationName;
    public float duration;
    private float time;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString(animationName);
        character.Rigidbody.velocity = Vector3.zero;
        character.CurrentMachRun = -1;
        character.CurrentSpeed = character.WalkSpeed;
        character.player.Combo.pause = true;
        if (!character.player.Combo.PerfectCombo) character.player.Combo.LooseFirstCombo();
        time = 0;
    }
    public override void Update()
    {
        time += Time.deltaTime;
        if (time > duration)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
    }
    protected override void UpdateInputLogic() { }
    public override void FixedUpdate() { }
    public override void LateUpdate() { }
    public override void OnCollisionEnter(Collision collision) { }
    public override void ExitState()
    {
        character.player.Combo.pause = false;
        time = 0;
    }
}
