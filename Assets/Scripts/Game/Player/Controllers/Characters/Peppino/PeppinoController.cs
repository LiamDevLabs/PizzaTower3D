using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PeppinoController : PlayerBaseController
{
    public PeppinoController(Player player) : base(player)
    {
        this.player = player;
    }

    [field: Header("References")]
    [field: SerializeField] public CapsuleCollider Collider { get; private set; }
    [field: SerializeField] public CapsuleCollider CrouchCollider { get; private set; }
    [field: SerializeField] public GameObject TechnicalProblems { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    public AnimatorNamedParameters AnimatorParameters { get; private set; }
    [field: SerializeField] public PeppinoStateManager StateManager { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public PlayerDamageTrigger TopDamage { get; private set; }
    [field: SerializeField] public PlayerDamageTrigger ForwardDamage { get; private set; }
    [field: SerializeField] public PlayerDamageTrigger BottomDamage { get; private set; }
    [field: SerializeField] public PlayerGrab Grab { get; private set; }
    [field: SerializeField] public PlayerParry Parry { get; private set; }
    public PeppinoHitbox Hitbox { get; private set; }
    public bool Damaged { get; set; } = false;

    [field: Header("Basic Movement")]
    [field: SerializeField] public MoveForceMode MovementMode { get; set; }
    [field: SerializeField] public float WalkSpeed { get; private set; } = 10f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 1f;
    [field: SerializeField] public float IdleTransitionSpeed { get; private set; } = 5f;
    [Range(0,1)] public float lateralMoveSensibility = 1f;
    public float CurrentSpeed { get; set; }
    public bool IsRunning { get; set; }

    public enum MoveForceMode
    {
        Force, Impulse, FlatVelocity,
        OnlyRunningFlatVelocity,
        OnlyForwardFlatVelocity,
        FlatVelocityWithHorizontalTransition,
        OnlyForwardFlatVelocityAtHighSpeed,
        OnlyForwardFlatVelocity2,
        OnlyForwardFlatVelocity3,
        OnPlanet,
        None,
    }

    [field: Header("Mach MinSpeed Movement")]
    [field: SerializeField] public float Run1Speed { get; private set; } = 10f;

    [field: SerializeField] public float Run2Speed { get; private set; } = 35f;

    [field: SerializeField] public float Run3Speed { get; private set; } = 60f;

    [field: SerializeField] public float MaxRunSpeed { get; private set; } = 70f;
    public int CurrentMachRun { get; set; }

    [field: Header("Mach Transition Speed")]
    [field: SerializeField] public float Run0TransitionSpeed { get; private set; } = 0.55f;
    [field: SerializeField] public float Run1TransitionSpeed { get; private set; } = 0.55f;
    [field: SerializeField] public float Run2TransitionSpeed { get; private set; } = 0.15f;
    [field: SerializeField] public float Run3TransitionSpeed { get; private set; } = 0.15f;

    [field: Header("Wall Climb")]
    [field: SerializeField] public float ClimbingSpeed { get; private set; } = 25f;
    [field: SerializeField] public bool UseCurrentSpeedToClimb { get; private set; }
    [field: SerializeField] public float AfterClimbingVerticalSpeed { get; private set; } = 3f;
    [field: SerializeField] public float AfterClimbingMinForwardSpeed { get; private set; } = 3f;
    [field: SerializeField] public float CeilingCrashTime { get; private set; }
    [field: SerializeField] public float TimeToWallClimbAfterCeilingCrash { get; private set; }
    public bool AfterCeilingCrash { get; set; }

    public float WallClimbedLastTime { get; set; }

    [field: Header("Wall Jump")]
    [field: SerializeField] public float ForceUpWallJump { get; private set; }
    [field: SerializeField] public float ForceForwardWallJump { get; private set; }
    [field: SerializeField] public float MoveForceWhileWallJumping { get; private set; }
    [field: SerializeField] public float UnpressWallJumpForce { get; private set; }
    [field: SerializeField] public float DelayToWallJumpedAnimation { get; private set; }
    [field: SerializeField] public float DelayToWallClimbAndUppercut { get; private set; }

    public bool WallJumping { get; set; }

    [field: Header("Braking")]
    [field: SerializeField] public float BrakingSpeed { get; private set; } = 10f;

    [field: Header("TurnAround")]
    [field: SerializeField] public float TurnAroundAngle { get; private set; }
    [field: SerializeField] public float TurnAroundRateDetector { get; private set; }
    [field: SerializeField] public float TurnAroundBrakingTime { get; private set; } = 10f;
    [field:Range(0,1)] [field: SerializeField] public float TurnAroundBrakingSpeedPercentage { get; private set; } = 10f;
    public bool drift = true;

    [field: Header("Jump")]
    [field: SerializeField] public float ForceJump { get; private set; }
    [field: SerializeField] public float UnpressJumpForce { get; private set; }
    [field: SerializeField] public float DelayToDetectGroundAfterJump { get; private set; }

    [field: Header("Super Jump")]
    [field: SerializeField] public float PreparingSuperJumpTime { get; private set; }
    [field: SerializeField] public float PreparingSuperJumpMoveSpeed { get; private set; }
    [field: SerializeField] public float SuperJumpTime { get; private set; }
    [field: SerializeField] public float SuperJumpMaxSpeed { get; private set; }
    [field: SerializeField] public float SuperJumpSpinSpeed { get; private set; }

    [field: Header("Shoulder Bash")]
    [field: SerializeField] public float ShoulderBashPreparingTime { get; private set; }
    [field: SerializeField] public float ShoulderBashForceSpeed { get; private set; }

    [field: Header("Uppercut")]
    [field: SerializeField] public float UppercutJumpDelay { get; private set; }
    [field: SerializeField] public float UppercutFallMultiplier { get; private set; }
    [field: SerializeField] public float ForceJumpUppercut { get; private set; }
    [field: SerializeField] public float UpperCutVelocityMultiplier { get; private set; }
    public bool AfterUppercut { get; set; } = false;

    [field: Header("Crouch")]
    [field: SerializeField] public float CrawlSpeed { get; private set; }

    [field: Header("Diving Downwards")]
    [field: SerializeField] public float DivingDownwardsSpeed { get; private set; }
    [field: SerializeField] public float DivingDownwardsMachRunMultiplier { get; private set; }
    [field: SerializeField] public float DivingDownwardsEnableDelayAfterWallClimbing { get; private set; }

    [field: Header("Diving Slam")]
    [field: SerializeField] public float DivingSlamJumpForce { get; private set; }
    [field: SerializeField] public float DivingSlamSpinSpeed { get; private set; }
    [field: SerializeField] public float DivingSlamFallingSpeed { get; private set; }
    public bool DivingSlamming { get; set; }

    [field: Header("Slide")]
    [field: SerializeField] public float TimeRollingBeforeSlide { get; private set; } = 0.5f;
    [field: SerializeField] public float GrabSlideBoost { get; private set; } = 2f;

    [field: Header("BodySlam")]
    [field: SerializeField] public float BodySlamPreparingTime { get; private set; }
    [field: SerializeField] public float BodySlamFallingAcceleration { get; private set; }
    [field: SerializeField] public float BodySlamHorizontalSpeed { get; private set; }
    [field: SerializeField] public float HeavyBodySlamMinTime { get; private set; }
    [field: SerializeField] public float BodySlamLandingTime { get; private set; }
    [field: SerializeField] public float BodySlamLandingJump { get; private set; }
    public bool BodySlamming { get; set; }
    public bool HeavyBodySlamming { get; set; }
    public bool AfterSlam { get; set; }

    [field: Header("Land")]
    [field: SerializeField] public float LandingTime { get; private set; }
    [field: SerializeField] [field:Range(-1, 3)] public int MaxMachRunToLand { get; private set; }

    [field: Header("Wall Crash")]
    [field: SerializeField] public float WallMaxAngle { get; private set; }
    [field: SerializeField] public float WallCrash1Time { get; private set; }
    [field: SerializeField] public float WallCrash2Time { get; private set; }
    [field: SerializeField] public float WallCrash2Force { get; private set; }

    [field: Header("Grab")]
    [field: SerializeField] public float GrabTime { get; private set; }
    [field: SerializeField] public float MinGrabCurrentSpeed { get; private set; }
    public bool AfterGrabMovement { get; set; } = false;
    public bool GrabToPeppinoDireccion = false; //True: Grab Forward of Peppino --------- False: Grab Forward of Cam

    [field: Header("Parry")]
    [field: SerializeField] public float TauntTime { get; private set; }
    [field: SerializeField] public float ParryTime { get; private set; }

    [field: Header("Holding")]
    [field: SerializeField] public int HoldingSpinRotationSpeed { get; private set; }
    [field: SerializeField] public float HoldingSpinBrakingSpeed { get; private set; }
    [field: SerializeField] public float HoldingSpinMinSpeedToStop { get; private set; }
    [field: SerializeField] public float HoldingAttackTime { get; private set; }
    [field: SerializeField] public float HoldingStateTime { get; private set; }
    [field: SerializeField] public float HoldingThrowForce { get; private set; }
    [field: SerializeField] public float HoldingKnockbackForce { get; private set; }
    [field: SerializeField] public float HoldingPileDrivingSpinSpeed { get; private set; }
    [field: SerializeField] public float HoldingPileDrivingFallingForce { get; private set; }

    [field: Header("Technical Problems")]
    [field: SerializeField] public float TechnicalProblemsTime { get; private set; }

    //Ground Check
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Vector3 groundCheckPosition;
    [SerializeField] private LayerMask groundCheckLayerMask;
    [SerializeField] private bool groundCheckGizmos;
    public bool IsGrounded { get; private set; }

    //Ceiling Check
    [Header("Ceiling Check")]
    [SerializeField] private float ceilingCheckRadius;
    [SerializeField] private Vector3 ceilingCheckStartPosition;
    [SerializeField] private Vector3 ceilingCheckEndPosition;
    [SerializeField] private LayerMask ceilingCheckLayerMask;
    [SerializeField] private bool ceilingCheckGizmos;
    public bool IsCeiling { get; private set; }

    //Wall Check
    [Header("Wall Check")]
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private float wallCheckRate;
    [SerializeField] private float wallCheckPivotOffset;
    private float wallCheckLast;
    [SerializeField] private Vector3 wallCheckStartPosition;
    [SerializeField] private Vector3 wallCheckEndPosition;
    [SerializeField] private LayerMask wallCheckLayerMask;
    [SerializeField] private bool wallCheckGizmos;
    public bool IsWall { get; private set; }
    public bool IsWallRaycast { get; private set; }
    public bool IsWallCrash { get; private set; }

    //Crouch Wall Check
    [Header("Crouch Wall Check")]
    [SerializeField] private float crouchWallCheckRadius;
    [SerializeField] private Vector3 crouchWallCheckPosition;
    [SerializeField] private bool crouchWallCheckGizmos;

    //Wall Raycast
    [Header("Wall Raycast")]
    [SerializeField] private Vector3 wallRaycastOffset;
    [SerializeField] private Vector3 wallRaycastDirection;
    [SerializeField] private float wallRaycastRange;
    [SerializeField] private bool wallRaycastGizmos;
    public RaycastHit HitWall
    {
        get { return hitWall; } 
        private set { hitWall = value; }
    }
    private RaycastHit hitWall;

    //Slope Raycast
    [Header("Slope Raycast")]
    [SerializeField] private float minAngleToSlope;
    [SerializeField] private Vector3 slopeRaycastOffset;
    private Vector3 currentSlopeRaycastOffset;
    [SerializeField] private float maxZOffsetSlopeMoveBySpeed;
    [SerializeField] private Vector3 slopeRaycastDirection;
    [SerializeField] private float slopeRaycastRange;
    [SerializeField] private bool slopeRaycastGizmos;
    public bool IsSlope { get; private set; }

    public RaycastHit GroundHit
    {
        get { return groundHit; }
        private set { groundHit = value; }
    }
    private RaycastHit groundHit;


    //Audio 
    [field: Header("Audio")]
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    [field: SerializeField] public PlayerAudios playerAudios { get; private set; }

    [Header("Debug")]
    public TextMeshPro stateText;
    public bool debugStates;

    protected override void SetSettings()
    {
        throw new System.NotImplementedException();
    }

    protected void Awake()
    {
        StateManager = new PeppinoStateManager(this);
        AnimatorParameters = Animator.GetComponent<AnimatorNamedParameters>();
        Animator.writeDefaultValuesOnDisable = true;
        Hitbox = Rigidbody.GetComponent<PeppinoHitbox>();
        Collider.enabled = true;
        CrouchCollider.enabled = false;
        StateManager.OnSwitchState += DebugState;
        DebugState();
    }

    public override void PlayerStart()
    {
        if (player.enabled)
            StateManager.Start();

        player.Score.health = Health;
        Health.score = player.Score;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void PlayerUpdate()
    {
        StateManager.Update();
        ShowState();
    }

    public override void PlayerFixedUpdate()
    {
        StateManager.FixedUpdate();
        Checkers();
    }

    public override void PlayerLateUpdate()
    {
        StateManager.LateUpdate();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(player.enabled)
            StateManager.OnCollisionEnter(collision);
    }

    private void Checkers()
    {
        IsGrounded = Physics.CheckSphere(Rigidbody.transform.TransformPoint(groundCheckPosition), groundCheckRadius, groundCheckLayerMask);
       
        IsCeiling = Physics.CheckCapsule(Rigidbody.transform.TransformPoint(ceilingCheckStartPosition), Rigidbody.transform.TransformPoint(ceilingCheckEndPosition), ceilingCheckRadius, ceilingCheckLayerMask);

        //Slope
        if (IsGrounded)
        {
            if (IsWall || CheckWallRaycast())
                currentSlopeRaycastOffset = slopeRaycastOffset;
            else
                currentSlopeRaycastOffset = new Vector3(slopeRaycastOffset.x, slopeRaycastOffset.y, slopeRaycastOffset.z + Mathf.InverseLerp(0, MaxRunSpeed, CurrentSpeed) * maxZOffsetSlopeMoveBySpeed);

            Vector3 playerDirection = Rigidbody.transform.TransformDirection(slopeRaycastDirection.normalized);
            bool groundRaycast = Physics.Raycast(Rigidbody.transform.TransformPoint(currentSlopeRaycastOffset), playerDirection, out groundHit, slopeRaycastRange, groundCheckLayerMask); 

            IsSlope =
                groundRaycast &&
                Vector3.Angle(-groundHit.normal, playerDirection) >= minAngleToSlope;
        }
        else IsSlope = false;

        //Wall
        if (Time.time > wallCheckLast + wallCheckRate)
        {
            Vector3 wcUp = IsSlope ? GroundHit.normal : Vector3.up;
            Quaternion wcRotation = Quaternion.FromToRotation(Rigidbody.transform.up, wcUp) * Rigidbody.rotation;
            Vector3 wcFinalStartPos = Rigidbody.transform.position + wcRotation * wallCheckStartPosition;
            Vector3 wcFinalEndPos = Rigidbody.transform.TransformPoint(wallCheckEndPosition);

            if (!CrouchCollider.enabled)
                IsWall = Physics.CheckCapsule(wcFinalStartPos, wcFinalEndPos, wallCheckRadius, wallCheckLayerMask);
            else
                IsWall = Physics.CheckSphere(Rigidbody.transform.TransformPoint(crouchWallCheckPosition), crouchWallCheckRadius, wallCheckLayerMask);

            if (IsWall)
            {   

                IsWallRaycast = CheckWallRaycast();

                IsWallCrash = IsWall &&
                              IsGrounded &&
                              !IsSlope &&
                              IsWallRaycast;
            }
            else
            {
                IsWallCrash = false;
                IsWallRaycast = false;
            }
            wallCheckLast = Time.time;
        }

    }

    private bool CheckWallRaycast()
    {
        Vector3 playerDirection = Rigidbody.transform.TransformDirection(wallRaycastDirection.normalized);
        Vector3 playerOnlyVerticalDirection = -hitWall.normal;
        playerOnlyVerticalDirection.y = playerDirection.y;
        return Physics.Raycast(Rigidbody.transform.TransformPoint(wallRaycastOffset), playerDirection, out hitWall, wallRaycastRange, wallCheckLayerMask) &&
                                Vector3.Angle(-hitWall.normal, playerOnlyVerticalDirection) <= WallMaxAngle; //Angle == 0
    }

    private void ShowState()
    {
        if(stateText)
            stateText.text = "" + StateManager.GetCurrentState();   
    }

    private void DebugState()
    {
        if (debugStates) 
            Debug.Log(StateManager.GetCurrentState());
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckGizmos)
        {
            Gizmos.color = IsGrounded ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(groundCheckPosition), groundCheckRadius);
        }
        if (ceilingCheckGizmos)
        {
            Gizmos.color = IsCeiling ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(ceilingCheckStartPosition), ceilingCheckRadius);
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(ceilingCheckEndPosition), ceilingCheckRadius);
        }

        if (wallCheckGizmos)
        {
            Vector3 wcUp = IsSlope ? GroundHit.normal : Vector3.up;
            Quaternion wcRotation = Quaternion.FromToRotation(Rigidbody.transform.up, wcUp) * Rigidbody.rotation;
            Vector3 wcFinalStartPos = Rigidbody.transform.position + wcRotation * wallCheckStartPosition;
            Vector3 wcFinalEndPos = Rigidbody.transform.TransformPoint(wallCheckEndPosition);

            Gizmos.color = IsWall ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(wcFinalStartPos, wallCheckRadius);
            Gizmos.DrawWireSphere(wcFinalEndPos, wallCheckRadius); 
            Gizmos.DrawRay(wcFinalStartPos + transform.forward * wallCheckRadius, wcFinalEndPos - wcFinalStartPos);
            Gizmos.DrawRay(wcFinalStartPos - transform.forward * wallCheckRadius, wcFinalEndPos - wcFinalStartPos);
            Gizmos.DrawRay(wcFinalStartPos + transform.right * wallCheckRadius, wcFinalEndPos - wcFinalStartPos);
            Gizmos.DrawRay(wcFinalStartPos - transform.right * wallCheckRadius, wcFinalEndPos - wcFinalStartPos);
        }
        if (wallRaycastGizmos)
        {
            Gizmos.color = IsWallRaycast ? Color.cyan : Color.yellow;
            Gizmos.DrawRay(Rigidbody.transform.TransformPoint(wallRaycastOffset), Rigidbody.transform.TransformDirection(wallRaycastDirection.normalized) * wallRaycastRange);

            if (IsWallRaycast)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(Rigidbody.transform.position, (Vector3.up - Vector3.Dot(Vector3.up, HitWall.normal) * HitWall.normal) * ClimbingSpeed);
            }
        }
        if (crouchWallCheckGizmos)
        {
            Gizmos.color = IsWall ? Color.cyan : Color.yellow; 
            Gizmos.DrawWireSphere(Rigidbody.transform.TransformPoint(crouchWallCheckPosition), crouchWallCheckRadius);
        }
        if (slopeRaycastGizmos)
        {
            Gizmos.color = IsSlope ? Color.cyan : Color.yellow;
            Gizmos.DrawRay(Rigidbody.transform.TransformPoint(currentSlopeRaycastOffset), Rigidbody.transform.TransformDirection(slopeRaycastDirection.normalized) * slopeRaycastRange);
        }
    }
}
