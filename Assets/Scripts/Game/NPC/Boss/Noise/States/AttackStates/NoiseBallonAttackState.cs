using System;
using UnityEngine;

internal class NoiseBallonAttackState : NoiseBossBaseState
{
    public NoiseBallonAttackState(NoiseBoss boss) : base(boss) { }

    enum StateParts
    {
        None, Jump, Floating
    }

    //Initialized
    StateParts stateParts = StateParts.None;
    float time = 0;
    bool ballonAnimated = false;
    Vector3 playerPosition;
    float lastBombTime;

    public override void EnterState()
    {
        Initialized();
        Jump();
    }

    public override void Update()
    {
        DropBomb();
        Timer();
    }

    public override void FixedUpdate()
    {
        Floating();
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void ExitState()
    {
        boss.Rigidbody.useGravity = true;
        boss.SetBouncy(false);
        boss.LookAtTarget.enabled = false;
        StopAudio();
    }


    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        stateParts = StateParts.None;
        ballonAnimated = false;
        time = 0;
        lastBombTime = 0;
        boss.Rigidbody.useGravity = false;
        boss.SetBouncy(true);
        boss.LookAtTarget.enabled = true;
        boss.AnimatorParameters.SetString("Balloon");
    }

    private void Jump()
    {
        //Check Ground
        //if (boss.IsGrounded)
            stateParts = StateParts.Jump;
        //else
        //    stateParts = StateParts.Spin;

        //Jump
        if (stateParts == StateParts.Jump)
        {
            Print("Jump");
            boss.Rigidbody.AddForce(boss.Rigidbody.transform.up * boss.Ballon_JumpImpulse, ForceMode.Impulse);
            boss.Rigidbody.AddForce(-boss.Rigidbody.transform.up * boss.Ballon_FallForce);
            PlayAudio(boss.Audios.jump, false);
        }
    }

    private void Floating()
    {
        if (stateParts == StateParts.Jump && boss.Rigidbody.velocity.y < 0)
            stateParts = StateParts.Floating;

        if (stateParts == StateParts.Floating)
        {
            Print("Jetpack");

            //Animation
            if (!ballonAnimated)
            {
                boss.AnimatorParameters.SetString("Balloon");
                PlayAudio(boss.Audios.flyBalloon, true);
                ballonAnimated = true;
            }

            //Calculate Direction
            playerPosition = boss.Player.Rigidbody.position + boss.Player.Rigidbody.transform.TransformVector(boss.Ballon_PlayerOffset);
            Vector3 direction = (playerPosition - boss.Rigidbody.position).normalized;
            //Move to Player
            boss.Rigidbody.AddForce(direction * boss.Ballon_ForceMove);
            boss.Rigidbody.velocity = boss.Rigidbody.velocity.magnitude > boss.Ballon_MaxSpeed ? direction * boss.Ballon_MaxSpeed : boss.Rigidbody.velocity; //Limit velocity
        }
    }

    private void DropBomb()
    {
        if(Time.time > lastBombTime + boss.Ballon_BombsRate)
        {
            lastBombTime = Time.time;
            ballonAnimated = false;
            boss.AnimatorParameters.SetString("BalloonDropBomb");
            UnityEngine.Object.Instantiate(boss.BombPrefab, boss.BombSpawnpoint.position, boss.BombSpawnpoint.rotation);
        }
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time > boss.Ballon_MaxTime)
            boss.StateManager.SwitchState(boss.StateManager.States.HittableState);
    }

    private void Print(string msg) 
    {
        if (boss.DebugStates)
            Debug.Log("Noise - JetpackState - "+msg);
    }
}