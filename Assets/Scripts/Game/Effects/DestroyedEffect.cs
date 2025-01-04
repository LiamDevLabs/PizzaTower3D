using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedEffect : MonoBehaviour
{
    private ParticleSystem sys;
    private ParticleSystem.MainModule sysMain;
    private ParticleSystem.VelocityOverLifetimeModule sysVelocity;
    private Rigidbody player;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public void SetPlayer(Rigidbody player) => this.player = player;

    private void Start()
    {
        if(!audioSource) TryGetComponent(out audioSource);
        if (!sys) TryGetComponent(out sys);
        if (sys)
        {
            sysMain = sys.main;
            Velocity();
            if(audioSource) PlayAudio();
        }
        else
        {
            Debug.LogWarning("No particle system assigned");
        }
    }

    private void Velocity()
    {
        if (player == null) return;
        sysVelocity = sys.velocityOverLifetime;
        sysVelocity.enabled = true;
        sysVelocity.space = ParticleSystemSimulationSpace.World;
        sysVelocity.x = player.velocity.x;
        sysVelocity.y = player.velocity.y;
        sysVelocity.z = player.velocity.z;
        if (player.velocity.y < -2) sysMain.gravityModifier = 0;
    }

    private void PlayAudio() => audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
}
