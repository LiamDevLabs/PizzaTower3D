using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoBodySlamState : PeppinoBaseState
{
    public PeppinoBodySlamState(PeppinoController player) : base(player) { }

    private float time=0;
    private float heavyBodySlamTime = 0;

    private bool divingJump = false;
    private float yAngle;

    private float currentFallingAcceleration;

    private bool audioFalling = false;

    public override void EnterState()
    {
        //Timers
        time = 0;
        heavyBodySlamTime = 0;
        //Bools
        character.BodySlamming = false;
        character.HeavyBodySlamming = false;
        //Damage
        character.BottomDamage.SetActive(false);
        //Diving Slam
        divingJump = character.DivingSlamming;
        yAngle = character.Rigidbody.transform.localEulerAngles.y;
        //Audio
        PlayAudio(character.playerAudios.startSlam, false);
        audioFalling = false;


        if (!character.DivingSlamming)
        {
            //Speed
            character.CurrentSpeed = character.BodySlamHorizontalSpeed;
            currentFallingAcceleration = character.BodySlamFallingAcceleration;
            //Preparing Animation
            character.AnimatorParameters.SetString("PreparingBodySlam");
        }
        else
        {
            //Speed
            currentFallingAcceleration = character.DivingSlamFallingSpeed;
            //Diving Slam Animation
            character.AnimatorParameters.SetString("DivingSlam");
            //Damage
            character.ForwardDamage.SetActive(true);
        }

    }

    public override void Update()
    {
        UpdateInputLogic();

        //Body Slam Time
        time += Time.deltaTime;

        if(time > character.BodySlamPreparingTime)
        {
            //Body Slam Fallling Animation
            if (!character.BodySlamming) 
            { 
                if (!character.DivingSlamming) 
                    character.AnimatorParameters.SetString("FallingBodySlam");
                else
                    character.AnimatorParameters.SetString("DivingSlam");
            }

            //Audio
            if (!audioFalling)
            {
                PlayAudio(character.playerAudios.fallSlam, true);
                audioFalling = true;
            }

            //Body Slamming
            character.BottomDamage.SetActive(true);
            character.BottomDamage.DestroyEnemies = true;
            character.BodySlamming = true;

            //Falling speed
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, -currentFallingAcceleration*(1+heavyBodySlamTime), character.Rigidbody.velocity.z);

            //Heavy Body Slam Time
            heavyBodySlamTime += Time.deltaTime;

            if (heavyBodySlamTime > character.HeavyBodySlamMinTime)
            {
                //Heavy Slamming
                character.HeavyBodySlamming = true;
            }
        }
        else if(!character.DivingSlamming)
        {
            //Preparing in air
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);
        }

        character.BottomDamage.DestroyMetal = character.HeavyBodySlamming;

        //Diving Slam
        if(character.DivingSlamming)
        {
            //Jump
            if (divingJump)
            {
                character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);
                character.Rigidbody.AddForce(character.Rigidbody.transform.up * character.DivingSlamJumpForce, ForceMode.Impulse);
                divingJump = false;
            }

            //Rotation
            yAngle += character.DivingSlamSpinSpeed * Time.deltaTime;
            character.Rigidbody.transform.localEulerAngles = new Vector3(character.Rigidbody.transform.localEulerAngles.x, yAngle, character.Rigidbody.transform.localEulerAngles.z);
        }
    }

    protected override void UpdateInputLogic()
    {
        MovementInput();

        //Land
        if (character.IsGrounded)
        {
            character.StateManager.SwitchState(character.StateManager.States.LandState);
            if(character.DivingSlamming) character.AnimatorParameters.SetString("DivingSlamLanding");
        }
    }

    public override void FixedUpdate()
    {
        MovePlayerWithVelocityLimits();
    }

    public override void LateUpdate()
    {

    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void ExitState()
    {
        StopAudio();
        character.ForwardDamage.SetActive(false);
        character.BottomDamage.SetActive(false);
        //Mirar en direccion de la camara
        if (character.DivingSlamming) character.Rigidbody.transform.eulerAngles = new Vector3(character.Rigidbody.transform.eulerAngles.x, character.player.Cam.transform.eulerAngles.y, character.Rigidbody.transform.eulerAngles.z);
    }
}