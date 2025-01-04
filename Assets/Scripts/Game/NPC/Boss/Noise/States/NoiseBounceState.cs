using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBounceState : NoiseBossBaseState
{
    public NoiseBounceState(NoiseBoss boss) : base(boss) { }

    //Initialized
    float time = 0;
    int bouncesCount = 0;
    float lastTimeBounce = 0;

    bool playAirAudio = false;

    enum StateParts
    {
        None,
    }

    //Initialized
    StateParts stateParts = StateParts.None;

    public override void EnterState()
    {
        Initialized();
        boss.SetBouncy(true);
    }

    public override void Update()
    {
        Timer();
    }

    public override void FixedUpdate()
    {
        Jump();
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void ExitState()
    {
        boss.Damaged = false;
        boss.SetBouncy(false);
        StopAudio();
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        time = 0;
        stateParts = StateParts.None;
        bouncesCount = 0;
        lastTimeBounce = float.MinValue;
        boss.AnimatorParameters.SetString("Bounce");
        playAirAudio = false;
    }

    private void Jump()
    {
        if (boss.IsGrounded || boss.IsColliding)
        {
            Print("Jump");
            CountJump();
            playAirAudio = false;

            //Jump Force
            boss.Rigidbody.AddForce(boss.Rigidbody.transform.up * boss.Bounce_JumpImpulse, ForceMode.Impulse);
            //Player Horizontal Direction
            Vector3 hDirection = (boss.Player.Rigidbody.position - boss.Rigidbody.position).normalized * boss.Bounce_ToPlayerImpulse;
            hDirection.y = boss.Rigidbody.velocity.y;
            //Horizontal Force
            boss.Rigidbody.velocity = hDirection;
            //Audio
            boss.HitAudioSource.Play();
        }
        else
        {
            AirAudio();
        }
    }

    private void AirAudio()
    {
        if (!playAirAudio)
        {
            PlayAudio(boss.Audios.spinAirBouncing, true);
            playAirAudio = true;
        }
    }

    private void CountJump()
    {
        //Count
        if(Time.time > lastTimeBounce + 0.1f)
        {
            bouncesCount++;
            lastTimeBounce = Time.time;
        }
        //Phase
        if(boss.CurrentPhase <= 0)
        {
            //Max Bounces
            if (bouncesCount > 1)
                ChangeState();
        }
        else
        {
            //Max Bounces
            if (bouncesCount > boss.Bounce_AmountSecondPhase + 1)
                ChangeState();
        }
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time > boss.Bounce_MaxTime && boss.Bounce_MaxTime != 0)
            ChangeState(); //Se acabo el tiempo del state
    }

    private void ChangeState()
    {
        //Si acaba de ser dañado...
        if (boss.Damaged)
        {
            //Atacar
            boss.StateManager.SwitchState(boss.StateManager.States.GetAttackState());
        }
        //Si acaba de atacar...
        else
        {
            //Puede ser dañado...
            boss.StateManager.SwitchState(boss.StateManager.States.HittableState);
        }
    }

    private void Print(string msg)
    {
        if (boss.DebugStates)
            Debug.Log("Noise - BounceState - " + msg);
    }
}
