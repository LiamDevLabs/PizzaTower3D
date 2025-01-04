using System;
using UnityEngine;

public class NoiseMinigunAttackState : NoiseBossBaseState
{
    public NoiseMinigunAttackState(NoiseBoss boss) : base(boss) { }

    //Initialized
    float lastFire, lastReload;

    public override void EnterState()
    {
        Initialized();
    }

    public override void Update()
    {
        Fire();
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
        boss.LookAtTarget.enabled = false;
    }

    /*--------------------------------------------------------------*/

    private void Initialized()
    {
        lastReload = Time.time;
        lastFire = 0;
        boss.LookAtTarget.enabled = true;
        boss.LookAtTarget.onlyHorizontalMove = false;
        boss.AnimatorParameters.SetString("MinigunIdle");
        Print("Initialized");
    }

    private void Fire()
    {
        if (Time.time < lastReload + boss.Minigun_ReloadRate)
        {
            if (Time.time > lastFire + boss.Minigun_FireRate)
            {
                //FIRE
                lastFire = Time.time;
                Rigidbody bullet = MonoBehaviour.Instantiate(boss.Minigun_Bullet_Prefab, boss.Minigun_Bullet_Origin.position, boss.Minigun_Bullet_Origin.rotation);
                bullet.AddForce(bullet.transform.forward * boss.Minigun_BulletForce, ForceMode.Impulse);
                boss.AudioSource.PlayOneShot(boss.Audio_Minigun_Fire[UnityEngine.Random.Range(0, boss.Audio_Minigun_Fire.Length)]);
                boss.AnimatorParameters.SetString("MinigunFire");
                Print("Fire");
            }
        }
        //RELOAD
        else
        {
            lastReload = Time.time;
            Print("Reload");
            boss.AudioSource.PlayOneShot(boss.Audio_Minigun_Reload);
            boss.StateManager.SwitchState(boss.StateManager.States.HittableState);
        }
    }

    /*
    private void Reload()
    {
        if (reloading)
        {
            //Animation
            if (!reloadStarted)
            {
                boss.AnimatorParameters.SetString("MinigunReload");
                startReloadTime = Time.time;
                reloadStarted = true;
            }

            //Reload Time
            if (Time.time > startReloadTime + ReloadingTime)
            {
                lastReload = Time.time;
                reloading = false;
            }
        }
    }
    */

    private void Print(string msg) 
    {
        if (boss.DebugStates)
            Debug.Log("Noise - SkateState - "+msg);
    }


    /*--------------------------------------------------------------*/
    /*--------------------------------------------------------------*/
}