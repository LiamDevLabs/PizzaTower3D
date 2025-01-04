using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTeleporter : Teleporter
{
    [SerializeField] private Transform pizzaTimeExitPoint;

    /*
    private void Awake() => CheckpointTrigger.OnCheckpoint += SetRespawnpoint;

    private void Start() => exitPoint = CheckpointTrigger.CurrentRespawnpoint;

    private void SetRespawnpoint() => exitPoint = CheckpointTrigger.CurrentRespawnpoint;
    */

    protected override void Trigger(Transform player)
    {
        if (PizzaTimeManager.PizzaTime && pizzaTimeExitPoint != null && exitPoint != pizzaTimeExitPoint) exitPoint = pizzaTimeExitPoint;

        base.Trigger(player);

        PeppinoController peppino = player.GetComponent<PeppinoHitbox>().Peppino;
        peppino.StateManager.SwitchState(peppino.StateManager.States.TechnicalProblemsState);
    }
}
