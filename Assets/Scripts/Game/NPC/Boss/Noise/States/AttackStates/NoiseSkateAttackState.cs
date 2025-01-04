using System;
using UnityEngine;

internal class NoiseSkateAttackState : NoiseBossBaseState
{
    public NoiseSkateAttackState(NoiseBoss boss) : base(boss) { }

    //Initialized
    bool startSkating = false;
    bool skateAnimated = false;
    bool playerPositionFound = false;
    Vector3 playerPosition;
    Vector3 startedSkatePosition;
    Vector3 direction = Vector3.zero;
    bool changeDirection = false;
    bool reachedToPlayer = false;
    float numberOfSkating;
    float lastChangeTime, rateChangeTime=1f;
    float reachedPlayerTime;
    float time = 0;

    bool skating = false;

    public override void EnterState()
    {
        Initialized();
    }

    public override void Update()
    {
        //If its grounded, start skating
        if (boss.IsGrounded)
            startSkating = true;

        skating = startSkating && numberOfSkating < boss.Skate_Number && !Timer();
    }

    public override void FixedUpdate()
    {
        if (skating)
            Skate();
        else
            boss.StateManager.SwitchState(boss.StateManager.States.HittableState);
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
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        startSkating = false;
        time = 0;
        skateAnimated = false;
        playerPositionFound = false;
        numberOfSkating = 0;
        changeDirection = false;
        reachedToPlayer = false;
        lastChangeTime = 0;
        boss.LookAtTarget.enabled = true;
        PlayAudio(boss.Audios.skating, true);
    }

    private void Skate()
    {
        //Change Direction
        if (changeDirection && Time.time > lastChangeTime + rateChangeTime)
        {
            lastChangeTime = Time.time;
            changeDirection = false;
            playerPositionFound = false;
            numberOfSkating++;
            boss.AnimatorParameters.SetString("SkateDrift");
        }
        //Move
        else
        {
            Print("Skate");

            //Calculate Direction
            if (!playerPositionFound)
            {
                CalculateDirection();
                playerPositionFound = true;
            }
            //Move
            else
            {
                Move();
            }
            //Animation
            if (!skateAnimated)
            {
                boss.AnimatorParameters.SetString("Skate");
                skateAnimated = true;
            }

            //Time Moving
            if (CalculatePlayerDistance())
            {
                reachedToPlayer = true;
                reachedPlayerTime = Time.time;
            }
            //Change Direction after reach player AND timer
            else if (reachedToPlayer && Time.time > reachedPlayerTime + boss.Skate_MoveOnTime)
            {
                changeDirection = true;
            }
            //Collide
            if (boss.IsColliding)
            {
                changeDirection = true;
                CalculateDirection();
                Move();
                Jump();
            }
        }
    }

    private void Print(string msg) 
    {
        if (boss.DebugStates)
            Debug.Log("Noise - SkateState - "+msg);
    }


    /*--------------------------------------------------------------*/
    /*--------------------------------------------------------------*/

    private bool Timer()
    {
        time += Time.deltaTime;
        return time >= boss.Skate_MaxTime;
    }

    private bool CalculatePlayerDistance() => Vector3.Distance(new Vector3(boss.Rigidbody.position.x, 0, boss.Rigidbody.position.z), new Vector3(boss.Player.Rigidbody.position.x, 0, boss.Player.Rigidbody.position.z)) < boss.Skate_PlayerDistance;

    private void Move()
    {
        if (new Vector3(boss.Rigidbody.velocity.x, 0, boss.Rigidbody.velocity.z).magnitude < boss.Skate_MaxSpeed)
            boss.Rigidbody.AddForce(direction * boss.Skate_Force);
        else
            boss.Rigidbody.velocity = new Vector3(direction.x * boss.Skate_MaxSpeed, boss.Rigidbody.velocity.y, direction.z * boss.Skate_MaxSpeed);
    }

    private void CalculateDirection()
    {
        //Find peppino position
        playerPosition = boss.Player.Rigidbody.position;
        startedSkatePosition = boss.Rigidbody.position;
        //Ground direction
        playerPosition.y = 0;
        startedSkatePosition.y = 0;
        direction = (playerPosition - startedSkatePosition).normalized;
    }

    private void Jump()
    {
        if(boss.Rigidbody.velocity.magnitude < 0.15)
            boss.Rigidbody.AddForce(boss.Rigidbody.transform.up * boss.Skate_JumpImpulse, ForceMode.Impulse);
    }
}