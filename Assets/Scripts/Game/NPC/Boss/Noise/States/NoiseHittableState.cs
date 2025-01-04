using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHittableState : NoiseBossBaseState
{
    public NoiseHittableState(NoiseBoss boss) : base(boss) { }

    //Initialized
    float time = 0;
    float maxTime;
    bool firstCall=false;
    bool isOnThisState = false;

    public override void EnterState()
    {
        Initialized();
        TauntAnimation();
        boss.DamageTrigger.SetActive(false);
        Print(".");
    }

    public override void FixedUpdate(){}
    public override void LateUpdate(){}
    public override void OnCollisionEnter(Collision collision){}

    public override void Update()
    {
        Timer();
    }

    public override void ExitState()
    {
        isOnThisState = false;
        boss.EnableDamageTriggerDelay();
        boss.LookAtTarget.enabled = false;
        boss.LookAtTarget.onlyHorizontalMove = true;
        boss.HittableEffect.SetActive(false);
        StopAudio();
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        boss.HittableEffect.SetActive(true);
        boss.LookAtTarget.enabled = true;
        time = 0;
        CalculateMaxTime();
        isOnThisState = true;
        if (!firstCall)
        {
            boss.Hitbox.OnHit += Damaged;
            firstCall = true;
        }
    }

    private void CalculateMaxTime()
    {
        if (boss.CurrentPhase != boss.Phases - 1)
            maxTime = boss.Hittable_Time - boss.CurrentPhase * boss.Hittable_SubtractPhaseTime;
        else
            maxTime = boss.Minigun_ReloadingTime;
        maxTime = maxTime < 0 ? 0 : maxTime;
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time > maxTime)
            ChangeToAttackState();
    }

    private void TauntAnimation()
    {
        PlayAudio(boss.Audios.hittable, true);
        if (boss.CurrentPhase != boss.Phases - 1)
            boss.AnimatorParameters.SetString("Taunt" + Random.Range(1, 2));
        else
            boss.AnimatorParameters.SetString("MinigunReload");
    }

    private void ChangeToAttackState()
    {
        if (boss.CurrentPhase != boss.Phases - 1)
            boss.StateManager.SwitchState(boss.StateManager.States.GetAttackState());
        else
            boss.StateManager.SwitchState(boss.StateManager.States.MinigunAttackState);
    }

    private void Damaged()
    {
        if (isOnThisState)
        {
            boss.Damaged = true;
            boss.Rigidbody.AddForce((boss.Rigidbody.position - boss.Player.Rigidbody.position).normalized * boss.Damaged_Force);
            boss.StateManager.SwitchState(boss.StateManager.States.BounceState);
            boss.HitParticles.Play(true);
            boss.Damage(1);
        }
    }

    private void Print(string msg)
    {
        if (boss.DebugStates)
            Debug.Log("Noise - HiitableState - " + msg);
    }
}
