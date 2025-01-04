using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoHoldingAttackState : PeppinoBaseState
{
    public PeppinoHoldingAttackState(PeppinoController player) : base(player) { }


    float time = 0;
    bool attacked = false;

    public override void EnterState()
    {
        character.AnimatorParameters.SetString("HoldingAttack1");
        character.CurrentMachRun = -1;
        character.CurrentSpeed = character.WalkSpeed;
        time = 0;
        attacked = false;
        //Preparar para lanzar
        character.Grab.PrepareToThrow();
    }

    public override void Update()
    {
        UpdateInputLogic();

        //Frenar
        float velocityX = Mathf.Lerp(character.Rigidbody.velocity.x, 0, Time.deltaTime * character.IdleTransitionSpeed);
        float velocityZ = Mathf.Lerp(character.Rigidbody.velocity.z, 0, Time.deltaTime * character.IdleTransitionSpeed);
        character.Rigidbody.velocity = new Vector3(velocityX, character.Rigidbody.velocity.y, velocityZ);

        //Tiempo para lanzar objeto
        time += Time.deltaTime;
        if (!attacked && time > character.HoldingAttackTime)
        {
            character.Grab.Throw(character.Rigidbody.transform.forward * character.HoldingThrowForce);
            character.Rigidbody.AddForce(-character.Rigidbody.transform.forward * character.HoldingKnockbackForce, ForceMode.Impulse);
            attacked = true;
        }

        //Tiempo para terminar el state
        if (time > character.HoldingStateTime)
            character.StateManager.SwitchState(character.StateManager.States.IdleState);
    }

    protected override void UpdateInputLogic()
    {

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
    }
}
