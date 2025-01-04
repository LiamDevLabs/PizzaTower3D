using System.Collections.Generic;
using UnityEngine;

public abstract class BossStates
{
    public BossBaseState HittableState { get; protected set; }
    protected List<BossBaseState> AttackStates { get; set; }
    protected List<BossBaseState> AttackStatesRoulette { get; set; }
    public BossStates()
    {
        AttackStates = new List<BossBaseState>();
        AttackStatesRoulette = new List<BossBaseState>();
    }

    public BossBaseState GetAttackState()
    {
        if (AttackStatesRoulette.Count == 0) AttackStatesRoulette.AddRange(AttackStates.ToArray());
        int index = Random.Range(0, AttackStatesRoulette.Count);
        BossBaseState AttackState = AttackStatesRoulette[index];
        AttackStatesRoulette.RemoveAt(index);
        return AttackState;
    }
}