using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoTemplateState : PeppinoBaseState
{
    public PeppinoTemplateState(PeppinoController player) : base(player) { }


    public override void EnterState()
    {
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

    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {

    }
}
