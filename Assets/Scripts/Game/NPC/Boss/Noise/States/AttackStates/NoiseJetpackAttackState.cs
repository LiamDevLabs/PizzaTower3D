using System;
using UnityEngine;

internal class NoiseJetpackAttackState : NoiseBossBaseState
{
    public NoiseJetpackAttackState(NoiseBoss boss) : base(boss) { }

    enum StateParts
    {
        None, Jump, Spin, Jetpack, Slide
    }

    //Initialized
    StateParts stateParts = StateParts.None;
    float time = 0;
    float spinningTime = 0;
    bool spinAnimated = false;
    bool jetpackAnimated = false;
    bool playerPositionFound = false;
    Vector3 playerPosition;
    Vector3 startedJetpackPosition;

    public override void EnterState()
    {
        Initialized();
        Jump();
    }

    public override void Update()
    {
        Spin();

        Timer();
    }

    public override void FixedUpdate()
    {
        Jetpack();
        Slide();
    }

    public override void LateUpdate()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void ExitState()
    {
        boss.LookAtTarget.enabled = false;
        boss.LookAtTarget.onlyHorizontalMove = true;
        StopAudio();
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        stateParts = StateParts.None;
        time = 0;
        spinAnimated = false;
        spinningTime = 0;
        jetpackAnimated = false;
        playerPositionFound = false;
        boss.SetBouncy(false);
        boss.LookAtTarget.enabled = true;
        boss.LookAtTarget.onlyHorizontalMove = false;
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
            boss.Rigidbody.AddForce(boss.Rigidbody.transform.up * boss.Jetpack_JumpImpulse, ForceMode.Impulse);
            boss.Rigidbody.AddForce(-boss.Rigidbody.transform.up * boss.Jetpack_FallForce);
            PlayAudio(boss.Audios.jump, false);
        }
    }

    private void Spin()
    {
        //Check BeforeState && Fall
        if(stateParts == StateParts.Jump && boss.Rigidbody.velocity.y < 0)
            stateParts = StateParts.Spin;

        //Spin
        if (stateParts == StateParts.Spin)
        {
            Print("Spin");

            //Animation
            if (!spinAnimated)
            {
                boss.AnimatorParameters.SetString("JetpackSpin");
                PlayAudio(boss.Audios.spinJetpack, false, 0.4f);
                spinAnimated = true;
            }
            //Hold in air
            boss.Rigidbody.velocity = Vector3.zero;
            //Timer
            spinningTime += Time.deltaTime;
            //Activate jetpack
            if(spinningTime >= boss.Jetpack_SpinningMaxTime)
                stateParts = StateParts.Jetpack;
        }
    }

    private void Jetpack()
    {
        if (stateParts == StateParts.Jetpack)
        {
            Print("Jetpack");

            //Find peppino position
            if (!playerPositionFound)
            {
                playerPosition = boss.Player.Rigidbody.position;
                startedJetpackPosition = boss.Rigidbody.position;
                playerPositionFound = true;
                boss.LookAtTarget.enabled = false;
            }
            //Animation
            if (!jetpackAnimated)
            {
                boss.AnimatorParameters.SetString("JetpackFly");
                PlayAudio(boss.Audios.flyJetpack, true, 0.4f);
                jetpackAnimated = true;
            }
            //Move to Player
            boss.Rigidbody.velocity = (playerPosition - startedJetpackPosition).normalized * boss.Jetpack_Speed;
            //Activate Slide
            if (boss.IsGrounded)
                stateParts = StateParts.Slide;
            //ChangeState Bounce
            if (boss.IsColliding)
                boss.StateManager.SwitchState(boss.StateManager.States.BounceState);
        }
    }

    private void Slide()
    {
        if (stateParts == StateParts.Slide)
        {
            Print("Slide");

            //Ground direction
            playerPosition.y = 0;
            startedJetpackPosition.y = 0;
            Vector3 direction = (playerPosition - startedJetpackPosition).normalized * boss.Jetpack_Speed;
            direction.y = boss.Rigidbody.velocity.y;
            //Keep moving
            boss.Rigidbody.velocity = direction;
            //ChangeState
            if (boss.IsColliding)
                boss.StateManager.SwitchState(boss.StateManager.States.BounceState);
        }
    }

    private void Timer()
    {
        time += Time.deltaTime;
        if (time > boss.Jetpack_MaxTime && boss.Jetpack_MaxTime != 0)
            boss.StateManager.SwitchState(boss.StateManager.States.BounceState); //Se acabo el tiempo del state
    }

    private void Print(string msg) 
    {
        if (boss.DebugStates)
            Debug.Log("Noise - JetpackState - "+msg);
    }
}