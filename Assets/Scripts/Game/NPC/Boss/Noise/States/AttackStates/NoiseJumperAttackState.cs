using System;
using UnityEngine;

internal class NoiseJumperAttackState : NoiseBossBaseState
{
    public NoiseJumperAttackState(NoiseBoss boss) : base(boss) { }

    //Initialized
    int bouncesCount = 0;
    float lastTimeBounce = 0;
    float time = 0;

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
        boss.LookAtTarget.enabled = false;
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        time = 0;
        stateParts = StateParts.None;
        bouncesCount = 0;
        lastTimeBounce = float.MinValue;
        boss.LookAtTarget.enabled = true;
    }

    private void Jump()
    {
        if (boss.IsGrounded || boss.IsColliding)
        {
            Print("Jump");
            //Animation
            boss.AnimatorParameters.SetString("PogoJump");
            //Contar saltos
            CountJump();
            //Jump Force
            boss.Rigidbody.AddForce(boss.Rigidbody.transform.up * boss.Jumper_JumpImpulse, ForceMode.Impulse);
            //Player Horizontal Direction
            Vector3 hDirection = (boss.Player.Rigidbody.position - boss.Rigidbody.position).normalized * boss.Jumper_ToPlayerImpulse;
            hDirection.y = boss.Rigidbody.velocity.y;
            //Horizontal Force
            boss.Rigidbody.velocity = hDirection;
        }
        else
        {
            boss.AnimatorParameters.SetString("PogoFall");
        }
    }

    private void CountJump()
    {
        //Count
        if (Time.time > lastTimeBounce + 0.1f)
        {
            bouncesCount++;
            lastTimeBounce = Time.time;
            //Audio
            PlayAudio(boss.Audios.pogoJump, false);
        }
        //Max Bounces
        if (bouncesCount > boss.Jumper_Amount + 1)
            ChangeState();
    }

    private void ChangeState()
    {
        //Si acaba de ser dañado...
        if (boss.Damaged)
        {
            //Atacar
            boss.StateManager.SwitchState(boss.StateManager.States.GetAttackState());
        }
        //Si acaba de atacar... (o se acabo el tiempo del state)
        else
        {
            //Puede ser dañado...
            boss.StateManager.SwitchState(boss.StateManager.States.HittableState);
        }
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time > boss.Jumper_MaxTime && boss.Jumper_MaxTime != 0)
            ChangeState(); //Se acabo el tiempo del state
    }

    private void Print(string msg)
    {
        if (boss.DebugStates)
            Debug.Log("Noise - JumperState - " + msg);
    }


}