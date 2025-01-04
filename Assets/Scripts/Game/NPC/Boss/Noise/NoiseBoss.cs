using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBoss : MonoBehaviour
{
    [field: Header("References")]
    public NoiseStateManager StateManager { get; private set; }
    [SerializeField] private BossFightManager bossFightManager;
    [field: SerializeField] public AnimatorNamedParameters AnimatorParameters { get; private set; }
    [field: SerializeField] public BossHitbox Hitbox { get; private set; }
    [field: SerializeField] public EnemyDamageTrigger DamageTrigger { get; private set; }
    [field: SerializeField] public LookAtTarget LookAtTarget { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public GameObject HealPrefab { get; private set; }
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    [field: SerializeField] public AudioSource HitAudioSource { get; private set; }

    [SerializeField] private Collider noiseCollider;
    [SerializeField] private PhysicMaterial defaultMaterial, bouncyMaterial;
    [field: SerializeField] public Rigidbody BombPrefab { get; private set; }
    [field: SerializeField] public Transform BombSpawnpoint { get; private set; }
    //Target
    public PeppinoController Player { get; private set; }

    [field: Header("Effect References")]
    [field: SerializeField] public Spritesheet HealthSpriteUI { get; private set; }
    private UI_Health healthUI;
    [field: SerializeField] public GameObject HittableEffect { get; private set; }
    [field: SerializeField] public ParticleSystem HitParticles { get; private set; }

    [field: Header("Audio References")]
    [field: SerializeField] public NoiseAudios Audios { get; private set; }
    [field: SerializeField] public AudioClip[] Audio_Minigun_Fire { get; private set; }
    [field: SerializeField] public AudioClip Audio_Minigun_Reload { get; private set; }

    [field: Header("General")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int Phases { get; private set; }
    [field: SerializeField] public float DropHealth_Height { get; private set; }
    public int CurrentPhase { get; private set; }

    [field: Header("HittableState")]
    [field: SerializeField] public float Hittable_Time { get; private set; }
    [field: SerializeField] public float Hittable_SubtractPhaseTime { get; private set; }
    [field: SerializeField] public float Damaged_Force { get; private set; }
    [SerializeField] public float timeToEnableDamageTrigger;
    public bool Damaged { get; set; }

    [field: Header("Jetpack - AttackState")]
    [field: SerializeField] public float Jetpack_JumpImpulse { get; private set; }
    [field: SerializeField] public float Jetpack_FallForce { get; private set; }
    [field: SerializeField] public float Jetpack_SpinningMaxTime { get; private set; }
    [field: SerializeField] public float Jetpack_Speed { get; private set; }
    [field: SerializeField] public float Jetpack_MaxTime { get; private set; }

    [field: Header("Skate - AttackState")]
    [field: SerializeField] public float Skate_MoveOnTime { get; private set; }
    [field: SerializeField] public float Skate_MaxSpeed { get; private set; }
    [field: SerializeField] public float Skate_Force { get; private set; }
    [field: SerializeField] public float Skate_Number { get; private set; }
    [field: SerializeField] public float Skate_PlayerDistance { get; private set; }
    [field: SerializeField] public float Skate_JumpImpulse { get; private set; }
    [field: SerializeField] public float Skate_MaxTime { get; private set; }

    [field: Header("Ballon - AttackState")]
    [field: SerializeField] public Vector3 Ballon_PlayerOffset { get; private set; }
    [field: SerializeField] public float Ballon_JumpImpulse { get; private set; }
    [field: SerializeField] public float Ballon_FallForce { get; private set; }
    [field: SerializeField] public float Ballon_MaxSpeed { get; private set; }
    [field: SerializeField] public float Ballon_ForceMove { get; private set; }
    [field: SerializeField] public float Ballon_BombsRate { get; private set; }
    [field: SerializeField] public float Ballon_MaxTime { get; private set; }

    [field: Header("Jumper - AttackState")]
    [field: SerializeField] public float Jumper_JumpImpulse { get; private set; }
    [field: SerializeField] public float Jumper_ToPlayerImpulse { get; private set; }
    [field: SerializeField] public float Jumper_Amount { get; private set; }
    [field: SerializeField] public float Jumper_MaxTime { get; private set; }

    [field: Header("Minigun - AttackState")]
    [field: SerializeField] public Rigidbody Minigun_Bullet_Prefab { get; private set; }
    [field: SerializeField] public Transform Minigun_Bullet_Origin { get; private set; }
    [field: SerializeField] public float Minigun_BulletForce { get; private set; }
    [field: SerializeField] public float Minigun_FireRate { get; private set; }
    [field: SerializeField] public float Minigun_ReloadRate { get; private set; }
    [field: SerializeField] public float Minigun_ReloadingTime { get; private set; }

    [field: Header("Bounce - State")]
    [field: SerializeField] public float Bounce_JumpImpulse { get; private set; }
    [field: SerializeField] public float Bounce_ToPlayerImpulse { get; private set; }
    [field:SerializeField] public float Bounce_AmountSecondPhase { get; private set; }
    [field: SerializeField] public float Bounce_MaxTime { get; private set; }

    [Header("General Collision Check")]
    [SerializeField] private float collisionCheckRadius;
    [SerializeField] private Vector3 collisionCheckPosition;
    [SerializeField] private LayerMask collisionCheckLayerMask;
    [SerializeField] private bool collisionCheckGizmos;
    public bool IsColliding { get; private set; }

    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Vector3 groundCheckPosition;
    [SerializeField] private LayerMask groundCheckLayerMask;
    [SerializeField] private bool groundCheckGizmos;
    public bool IsGrounded { get; private set; }

    [field: Header("Debug")]
    [field: SerializeField] public bool DebugStates { get; private set; }

    /*----------------------------------------------------------*/

    private void Awake()
    {
        StateManager = new NoiseStateManager(this);
        DamageTrigger.SetActive(true);
        CurrentPhase = 0;
        CurrentHealth = MaxHealth;
        Animator animator = AnimatorParameters.GetComponent<Animator>();
        Debug.Log(animator.writeDefaultValuesOnDisable);
        animator.writeDefaultValuesOnDisable = true;
    }

    private IEnumerator Start()
    {
        yield return null;
        SetPeppino();
        StateManager.Start(StateManager.States.GetAttackState());
    }

    private void Update()
    {
        StateManager.Update();
        Checkers();
    }

    private void LateUpdate() => StateManager.LateUpdate();

    private void FixedUpdate() => StateManager.FixedUpdate();

    private void OnCollisionEnter(Collision collision) => StateManager.OnCollisionEnter(collision);

    private void SetPeppino()
    {
        Player = FindObjectOfType<PeppinoController>();
        Player.Health.activated = true;
        LookAtBoss peppinoLookAt = Player.GetComponentInChildren<LookAtBoss>();
        peppinoLookAt.target = Rigidbody.transform;
        healthUI = FindObjectOfType<UI_Health>(true);
        healthUI.gameObject.SetActive(true);
        healthUI.SetBossUI(HealthSpriteUI, MaxHealth);
    }

    private void Checkers()
    {
        IsColliding = Physics.CheckSphere(Rigidbody.transform.TransformPoint(collisionCheckPosition), collisionCheckRadius, collisionCheckLayerMask);
        IsGrounded = Physics.CheckSphere(Rigidbody.transform.TransformPoint(groundCheckPosition), groundCheckRadius, groundCheckLayerMask);
    }

    public void SetBouncy(bool isBouncy)
    {
        if (isBouncy)
            noiseCollider.material = bouncyMaterial;
        else
            noiseCollider.material = defaultMaterial;
    }

    public void EnableDamageTriggerDelay() => Invoke("EnableDamageTrigger", timeToEnableDamageTrigger);
    private void EnableDamageTrigger() => DamageTrigger.SetActive(true);

    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (CurrentHealth <= 0)
        {
            CurrentPhase++;
            bossFightManager.NextPhase(CurrentPhase==Phases);
            if (CurrentPhase < Phases)
            {                
                //Restaurar Salud
                if(CurrentPhase != Phases-1)
                    CurrentHealth = MaxHealth;
                else
                {
                    StateManager.SwitchState(StateManager.States.MinigunAttackState);
                    CurrentHealth = 1;
                }
            }
            else
            {
                //Derrotado definiviamente
                Destroy(gameObject);
            }
        }
        DoByDamage();
        healthUI.UpdateBossHealth(CurrentHealth);
    }

    private void DoByDamage()
    {
        //General
        HitAudioSource.Play();
        //Specific Phase and Damage
        switch (CurrentPhase)
        {
            case 0:
                switch (CurrentHealth)
                {
                    case 5: DropHealth(); break;
                    case 2: DropHealth(); break;
                }
                break;
            case 1:
                switch (CurrentHealth)
                {
                    case 7: DropHealth(); break;
                    case 1: DropHealth(); break;
                }
                break;
        }
    }

    private void DropHealth()
    {
        Vector3 position = new Vector3(Rigidbody.position.x, DropHealth_Height, Rigidbody.position.z);
        Instantiate(HealPrefab, position, Rigidbody.rotation);
    }

    private void OnEnable()
    {
        if(CurrentPhase > 0 && CurrentPhase != Phases - 1)
            StateManager.SwitchState(StateManager.States.GetAttackState());
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckGizmos)
        {
            Gizmos.color = IsGrounded ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(groundCheckPosition), groundCheckRadius);
        }
        if (collisionCheckGizmos)
        {
            Gizmos.color = IsColliding ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(collisionCheckPosition), collisionCheckRadius);
        }
    }
}

public abstract class NoiseBossBaseState : BossBaseState
{
    protected NoiseBoss boss;
    protected NoiseBossBaseState(NoiseBoss boss) => this.boss = boss;

    protected void PlayAudio(AudioClip clip, bool loop, float volume = 1)
    {
        if (clip == null) return;
        boss.AudioSource.clip = clip;
        boss.AudioSource.loop = loop;
        boss.AudioSource.volume = volume;
        boss.AudioSource.Play();
    }

    protected void StopAudio()
    {
        boss.AudioSource.Stop();
        boss.AudioSource.clip = null;
        boss.AudioSource.loop = false;
    }
}
public class NoiseBossStates : BossStates
{
    public NoiseBounceState BounceState { get; protected set; }
    public NoiseMinigunAttackState MinigunAttackState { get; protected set; }

    public NoiseBossStates(NoiseBoss boss) : base()
    {
        HittableState = new NoiseHittableState(boss);
        BounceState = new NoiseBounceState(boss);
        MinigunAttackState = new NoiseMinigunAttackState(boss);
        AttackStates.Add(new NoiseJetpackAttackState(boss));
        AttackStates.Add(new NoiseSkateAttackState(boss));
        AttackStates.Add(new NoiseBallonAttackState(boss));
        AttackStates.Add(new NoiseJumperAttackState(boss));
        AttackStatesRoulette.AddRange(AttackStates.ToArray());
    }
}
public class NoiseStateManager : BaseBossStateManager
{
    public NoiseBossStates States { get; protected set; }
    public NoiseStateManager(NoiseBoss boss) => States = new NoiseBossStates(boss);
}

