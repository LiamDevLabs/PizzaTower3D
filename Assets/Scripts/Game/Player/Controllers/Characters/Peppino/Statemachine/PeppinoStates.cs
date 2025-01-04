public class PeppinoStates
{
    public PeppinoIdleState IdleState { get; private set; }
    public PeppinoWalkState WalkState { get; private set; }
    public PeppinoRun0State Run0State { get; private set; }
    public PeppinoRun1State Run1State { get; private set; }
    public PeppinoRun2State Run2State { get; private set; }
    public PeppinoRun3State Run3State { get; private set; }
    public PeppinoWallClimbingState WallClimbingState { get; private set; }
    public PeppinoBrakingState BrakingState { get; private set; }
    public PeppinoTurnAroundState TurnAroundState { get; private set; }
    public PeppinoJumpState JumpState { get; private set; }
    public PeppinoFallState FallState { get; private set; }
    public PeppinoLandState LandState { get; private set; }
    public PeppinoCrouchState CrouchState { get; private set; }
    public PeppinoCrawlState CrawlState { get; private set; }
    public PeppinoCrouchJumpState CrouchJumpState { get; private set; }
    public PeppinoBodySlamState BodySlamState { get; private set; }
    public PeppinoDivingDownwardsState DivingDownwardsState { get; private set; }
    public PeppinoSlideState SlideState { get; private set; }
    public PeppinoWallCrashState WallCrashState { get; private set; }
    public PeppinoGrabState GrabState { get; private set; }
    public PeppinoHoldingIdleState HoldingIdleState { get; private set; }
    public PeppinoHoldingWalkState HoldingWalkState { get; private set; }
    public PeppinoHoldingSpinState HoldingSpinState { get; private set; }
    public PeppinoHoldingJumpState HoldingJumpState { get; private set; }
    public PeppinoHoldingFallState HoldingFallState { get; private set; }
    public PeppinoHoldingPileDrivingState HoldingPileDrivingState { get; private set; }
    public PeppinoHoldingAttackState HoldingAttackState { get; private set; }
    public PeppinoWallJumpState WallJumpState { get; private set; }
    public PeppinoCeilingCrashState CeilingCrashState { get; private set; }
    public PeppinoSuperJumpPreparingState SuperJumpPreparingState { get; private set; }
    public PeppinoSuperJumpState SuperJumpState { get; private set; }
    public PeppinoShoulderBashState ShoulderBashState { get; private set; }
    public PeppinoUppercutState UppercutState { get; private set; }
    public PeppinoParryState ParryState { get; private set; }
    public PeppinoDamagedState DamagedState { get; private set; }
    public PeppinoDoorState DoorState { get; private set; }
    public PeppinoTechnicalProblemsState TechnicalProblemsState { get; private set; }

    public PeppinoStates(PeppinoController player) 
    {
        IdleState = new PeppinoIdleState(player);
        WalkState = new PeppinoWalkState(player);
        Run0State = new PeppinoRun0State(player);
        Run1State = new PeppinoRun1State(player);
        Run2State = new PeppinoRun2State(player);
        Run3State = new PeppinoRun3State(player);
        WallClimbingState = new PeppinoWallClimbingState(player);
        BrakingState = new PeppinoBrakingState(player);
        TurnAroundState = new PeppinoTurnAroundState(player);
        JumpState = new PeppinoJumpState(player);
        FallState = new PeppinoFallState(player);
        LandState = new PeppinoLandState(player);
        CrouchState = new PeppinoCrouchState(player);
        CrawlState = new PeppinoCrawlState(player);
        CrouchJumpState = new PeppinoCrouchJumpState(player);
        BodySlamState = new PeppinoBodySlamState(player);
        DivingDownwardsState = new PeppinoDivingDownwardsState(player);
        SlideState = new PeppinoSlideState(player);
        WallCrashState = new PeppinoWallCrashState(player);
        GrabState = new PeppinoGrabState(player);
        HoldingIdleState = new PeppinoHoldingIdleState(player);
        HoldingWalkState = new PeppinoHoldingWalkState(player);
        HoldingJumpState = new PeppinoHoldingJumpState(player);
        HoldingFallState = new PeppinoHoldingFallState(player);
        HoldingAttackState = new PeppinoHoldingAttackState(player);
        HoldingSpinState = new PeppinoHoldingSpinState(player);
        HoldingPileDrivingState = new PeppinoHoldingPileDrivingState(player);
        WallJumpState = new PeppinoWallJumpState(player);
        CeilingCrashState = new PeppinoCeilingCrashState(player);
        SuperJumpPreparingState = new PeppinoSuperJumpPreparingState(player);
        SuperJumpState = new PeppinoSuperJumpState(player);
        ShoulderBashState = new PeppinoShoulderBashState(player);
        UppercutState = new PeppinoUppercutState(player);
        ParryState = new PeppinoParryState(player);
        DamagedState = new PeppinoDamagedState(player);
        DoorState = new PeppinoDoorState(player);
        TechnicalProblemsState = new PeppinoTechnicalProblemsState(player);
    }
}
