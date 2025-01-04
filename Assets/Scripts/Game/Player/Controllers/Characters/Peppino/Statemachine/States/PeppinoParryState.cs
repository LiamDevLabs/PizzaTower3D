using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PeppinoParryState : PeppinoBaseState
{
    public PeppinoParryState(PeppinoController player) : base(player) { }

    float time = 0;
    float maxTime;
    Vector3 previousVelocity;
    bool parried = false;

    public override void EnterState()
    {
        time = 0;
        maxTime = character.TauntTime;
        parried = false;
        int tauntNumber = Random.Range(1, 1);
        previousVelocity = character.Rigidbody.velocity;
        character.Rigidbody.isKinematic = true;
        character.AnimatorParameters.SetString("Taunt"+tauntNumber);
        character.Parry.SetActive(true);
        character.Rigidbody.transform.forward = new Vector3(-character.player.Cam.transform.forward.x, character.Rigidbody.transform.forward.y, -character.player.Cam.transform.forward.z);
        PlayAudio(character.playerAudios.taunt, false);
    }

    public override void Update()
    {
        UpdateInputLogic();

        //Time to exit
        time += Time.deltaTime;
        if (time >= maxTime)
        {
            //Set previous velocity
            character.Rigidbody.velocity = previousVelocity;

            //Exit state
            SwitchToCurrentMachRun();
        }

        //PARRY
        if (character.Parry.Parrying && !parried)
        {
            Vector3 firstParryDirection = (character.Parry.GetFirstParriedPosition() - character.Rigidbody.transform.position).normalized;
            character.Rigidbody.transform.forward = new Vector3(firstParryDirection.x, character.Rigidbody.transform.forward.y, firstParryDirection.z);
            character.AnimatorParameters.SetString("Parry");
            time = 0;
            maxTime = character.ParryTime;
            character.Collider.enabled = false;
            parried = true;
            PlayAudio(character.playerAudios.parry, false);
        }
    }

    protected override void UpdateInputLogic()
    {


    }

    public override void FixedUpdate()
    {

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
        character.Rigidbody.isKinematic = false;
        character.Parry.SetActive(false);
        if(character.IsGrounded) character.AfterUppercut = false;
        character.Collider.enabled = true;
    }
}
