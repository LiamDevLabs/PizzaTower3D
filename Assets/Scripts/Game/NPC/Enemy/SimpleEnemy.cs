 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEnemy : BaseEnemy
{
    [Header("SimpleEnemy")]
    [SerializeField] protected AnimatorNamedParameters animatorParameters;
    [SerializeField] protected Grippable grippable;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioScared;
    [SerializeField] private FollowWaypoints followWaypoints;
    [SerializeField] private FollowTarget followTarget;
    [SerializeField] private LookAtTarget lookAtTarget;
    [SerializeField] private EnemyDamageTrigger damageTrigger; //OPTIONAL
    [SerializeField] private float stunnedTime;

    bool gripped = false;
    float stunnedTimeCounter = 0;
    bool playedScaredAudio = false;


    protected override void States()
    {
        Gripped();
        Released();
        Patrol();
        Attack();
        Scared();
    }

    private void Gripped()
    {
        if (grippable.Gripped || grippable.Throwed)
        {
            animatorParameters.SetString("Gripped");
            lookAtTarget.enabled = false;
            followTarget.enabled = false;
            followWaypoints.enabled = false;
            gripped = true;
            stunnedTimeCounter = 0;
        }
        CanDamage(!grippable.Gripped);
    }

    private void Released()
    {
        if(!grippable.Gripped && gripped)
        {
            stunnedTimeCounter += Time.deltaTime;
            if(stunnedTimeCounter >= stunnedTime)
                gripped = false;
        }
    }

    private void Patrol()
    {
        if(!gripped && !grippable.Throwed)
        if(!isPlayerOnRange)
        {
            //Animation
            if(new Vector3(followWaypoints.Rb.velocity.x,0, followWaypoints.Rb.velocity.z).magnitude != 0)
                animatorParameters.SetString("Walk");
            else
                animatorParameters.SetString("Idle");
            //Booleans
            lookAtTarget.enabled = false;
            followTarget.enabled = false;
            followWaypoints.enabled = true;
            CanDamage(true);
        }
    }

    private void Attack()
    {
        if (!gripped && !grippable.Throwed)
        if (isPlayerOnRange && player && player.CurrentMachRun < 2)
        {
            //Animation
            if (new Vector3(followTarget.rb.velocity.x, 0, followTarget.rb.velocity.z).magnitude > 0.05)
                animatorParameters.SetString("Walk");
            else
                animatorParameters.SetString("Idle");
            //Booleans
            lookAtTarget.enabled = true;
            followTarget.enabled = true;
            followWaypoints.enabled = false;
            CanDamage(true);
        }
    }

    private void Scared()
    {
        if (!gripped && !grippable.Throwed)
        if (isPlayerOnRange && player && player.CurrentMachRun >= 2)
        {
            animatorParameters.SetString("Scared");
            lookAtTarget.enabled = true;
            followTarget.enabled = false;
            followWaypoints.enabled = false;
            CanDamage(false);
            PlayScaredAudio();
        }
        else playedScaredAudio = false;
    }

    private void PlayScaredAudio()
    {
        if (!playedScaredAudio)
        {
            audioSource.PlayOneShot(audioScared);
            playedScaredAudio = true;
        }
    }

    private void CanDamage(bool enable)
    {
        if (damageTrigger != null) damageTrigger.SetActive(enable);
    }
}
