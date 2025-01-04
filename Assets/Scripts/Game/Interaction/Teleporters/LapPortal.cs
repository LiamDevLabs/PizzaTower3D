using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapPortal : Teleporter 
{
    [Header("Lap Portal")]
    [SerializeField] private PizzaTimeManager pizzaTimeManager;
    [SerializeField] private int lap;

    private void Awake() => gameObject.SetActive(false);

    protected override void Trigger(Transform player)
    {
        base.Trigger(player);
        pizzaTimeManager.EnableClones(lap);
    }
}
