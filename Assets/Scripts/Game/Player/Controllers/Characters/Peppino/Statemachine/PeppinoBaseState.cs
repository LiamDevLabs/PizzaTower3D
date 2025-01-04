using UnityEngine;
using System.Linq;

public abstract class PeppinoBaseState
{
    protected PeppinoController character;


    public PeppinoBaseState(PeppinoController player)
    {
        this.character = player;
    }

    public abstract void EnterState();

    public abstract void Update();

    public abstract void LateUpdate();

    protected abstract void UpdateInputLogic();

    public abstract void FixedUpdate();

    public abstract void OnCollisionEnter(Collision collision);

    public abstract void ExitState();


    /*--------------------------Funciones que se pueden repetir en los states--------------------------*/

    protected Vector3 movement;
    protected Vector3 movementInput;
    protected void MovementInput()
    {
        //Input del movimiento
        Vector3 input = new Vector3(character.player.Input.MoveInput.x, 0, character.player.Input.MoveInput.y);

        if (input == Vector3.zero && character.GrabToPeppinoDireccion)
            movementInput = character.Rigidbody.transform.forward.normalized;
        else
            movementInput = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * new Vector3(character.player.Input.MoveInput.x * character.lateralMoveSensibility, 0, character.player.Input.MoveInput.y);

        movement = movementInput * character.CurrentSpeed;

    }

    private Vector2 currentDirection;
    private Vector2 previousDirection;
    private float lastPreviousDirection;
    protected bool jumpedWhileRunning = false;
    protected void RunBaseInput()
    {
        character.IsRunning = true;
        jumpedWhileRunning = false;

        //Movement input  
        if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput)) //Si se está moviendo el stick de movimiento...
        {
            movementInput = new Vector3(character.player.Input.MoveInput.x * character.lateralMoveSensibility, 0, character.player.Input.MoveInput.y);
            movement = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;
        }
        //Movement with NO input direction
        else
        {
            if (!character.GrabToPeppinoDireccion)
                movementInput = new Vector3(0, 0, 1);
            else
            {
                movementInput = character.Rigidbody.transform.forward.normalized;
                movement = movementInput * character.CurrentSpeed;
            }

        }

        if (!character.GrabToPeppinoDireccion)
            movement = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;


        //Si tocás el suelo...
        if (character.IsGrounded)
        {
            //Saltar
            if (character.player.Input.JumpButtonInput.triggered)
            {
                jumpedWhileRunning = character.CurrentMachRun > 1;
                character.StateManager.SwitchState(character.StateManager.States.JumpState);
            }

            //Deslizarse
            if (character.player.Input.CrouchButtonInput.triggered)
                character.StateManager.SwitchState(character.StateManager.States.SlideState);

            //Chocar Pared
            if (character.IsWallCrash)
                character.StateManager.SwitchState(character.StateManager.States.WallCrashState);

            //Si Slope...
            if (character.IsSlope)
            {
                //Trepar muro
                if(character.IsWallRaycast)
                    character.StateManager.SwitchState(character.StateManager.States.WallClimbingState);

                movement = Vector3.ProjectOnPlane(movement, character.GroundHit.normal);

                if(character.player.Input.ControllerType == DeviceSettings.ControllerType.Keyboard)
                    character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);
            }
            else
            //character.Rigidbody.useGravity = !character.IsSlope;

            //Quedarte pegado al suelo
            character.Rigidbody.velocity = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z);
        }
        //Si NO tocas el suelo...
        else
            //Caer
            character.StateManager.SwitchState(character.StateManager.States.FallState);

        //Agarrar
        if (character.player.Input.GrabButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.GrabState);

        //Si dejas de correr...
        if (!character.player.Input.RunButtonInput.triggered)
        {
            //Si MachRun mayor a 0 --> BrakingState
            if (character.CurrentMachRun > 0)
                character.StateManager.SwitchState(character.StateManager.States.BrakingState);
            //Si MachRun es menor a 0 --> IdleState
            else
                character.StateManager.SwitchState(character.StateManager.States.IdleState);
        }

        //Parry
        if(character.player.Input.ParryButtonInput.down)
            character.StateManager.SwitchState(character.StateManager.States.ParryState);

        //Activar daño para romper metal
            character.ForwardDamage.DestroyMetal = character.CurrentMachRun > 1;

        //-----------------------Turn Around-----------------------
        if (character.CurrentMachRun > 0 && !character.player.Input.ParryButtonInput.down)
            TurnAround();
    }

    protected void MovePlayerWithVelocityLimits()
    {
        //Move player
        MoveMode();
        LimitVelocity();
    }

    protected void LimitVelocity()
    {
        // Limitar velocidad actual si es necesario
        Vector3 flatVel = new Vector3(character.Rigidbody.velocity.x, 0f, character.Rigidbody.velocity.z);
        if (flatVel.magnitude > character.CurrentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * character.CurrentSpeed;
            character.Rigidbody.velocity = new Vector3(limitedVel.x, character.Rigidbody.velocity.y, limitedVel.z);
        }

        //Limitar maxima velocidad
        if (character.CurrentSpeed > character.MaxRunSpeed)
            character.CurrentSpeed = character.MaxRunSpeed;
    }


    //Mirar en la dirección que se mueve
    Quaternion desiredRotation = Quaternion.identity;
    private void Rotate()
    {
        Vector3 hDirection = new Vector3(character.Rigidbody.velocity.x, 0, character.Rigidbody.velocity.z).normalized;

        //Rotacion del jugador durante el match run
        if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(movement.normalized))
            //El jugador rota apuntando hacia la direccion en que apunta el vector del movimiento plano (es decir NO cuenta los SLOPES) 
            desiredRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(movement.normalized, Vector3.up), Vector3.up);
        else if (character.CurrentMachRun >= 0)
        {
            if (!character.GrabToPeppinoDireccion)
            {
                //El jugador rota apuntando hacia la direccion en que apunta la camara
                desiredRotation = Quaternion.LookRotation(new Vector3(character.player.Cam.transform.forward.x, character.Rigidbody.transform.forward.y, character.player.Cam.transform.forward.z), Vector3.up);
            }
        }
        else if(hDirection != Vector3.zero)
            //El jugador rota apuntando hacia la direccion en que apunta el vector de velocidad
            desiredRotation = Quaternion.LookRotation(hDirection, Vector3.up);

        if (!character.WallJumping || !character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(movement.normalized))
            character.Rigidbody.rotation = Quaternion.Slerp(character.Rigidbody.rotation, desiredRotation, character.RotationSpeed * Time.fixedDeltaTime);

        //Rotar hacia el suelo (podria ser util en otra cosa)
        //Quaternion desiredRotation2 = Quaternion.FromToRotation(character.Rigidbody.transform.up, character.GroundHit.normal) * desiredRotation;
    }

    private void TurnAround()
    {
        if (!character.drift) return;

        Vector2 hDirection = new Vector2(character.Rigidbody.transform.forward.x, character.Rigidbody.transform.forward.z);
        //Vector2 inputDirection = character.player.Input.MoveInput;

        //current direction
        currentDirection = hDirection;
        //previous direction 
        if (Time.time > lastPreviousDirection + character.TurnAroundRateDetector)
        {
            previousDirection = hDirection;
            lastPreviousDirection = Time.time;
        }

        //Turn around
        if (Vector2.Angle(previousDirection, currentDirection) > character.TurnAroundAngle)
        {
            character.StateManager.SwitchState(character.StateManager.States.TurnAroundState);
            previousDirection = currentDirection;
        }

        //Debug.Log("current direction " + currentDirection);
        //Debug.Log("previous direction " + previousDirection);
        //Debug.Log("angle " + Vector2.Angle(previousDirection, currentDirection));
    }


    float hTimeRunTransition;
    float fTimeRunTransition;
    private void MoveMode()
    {
        switch (character.MovementMode)
        {
            case PeppinoController.MoveForceMode.Force:
                character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                Rotate();
                break;
            case PeppinoController.MoveForceMode.Impulse:
                character.Rigidbody.AddForce(movement / 2.25f, ForceMode.Impulse);
                Rotate();
                break;
            case PeppinoController.MoveForceMode.FlatVelocity:
                character.Rigidbody.velocity = new Vector3(movement.x * 10f, character.Rigidbody.velocity.y, movement.z * 10f);
                Rotate();
                break;
            case PeppinoController.MoveForceMode.OnlyRunningFlatVelocity:
                if (character.IsRunning)
                    //Flat Move
                    character.Rigidbody.velocity = new Vector3(movement.x * 10f, character.Rigidbody.velocity.y, movement.z * 10f);
                //Force Move
                else 
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.OnlyForwardFlatVelocity:
                //Flat Move
                if (character.IsRunning)
                {
                    //Movement input  
                    Vector3 hMoveInput = new Vector3(character.player.Input.MoveInput.x, 0, 0);
                    Vector3 fMoveInput = new Vector3(0, 0, character.player.Input.MoveInput.y);
                    if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput)) fMoveInput = Vector3.forward;

                    Vector3 hMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * hMoveInput * character.CurrentSpeed;
                    Vector3 fMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * fMoveInput * character.CurrentSpeed;

                    character.Rigidbody.AddForce(hMove * 20f, ForceMode.Force);
                    character.Rigidbody.velocity = new Vector3(fMove.x * 10f, character.Rigidbody.velocity.y, fMove.z * 10f);
                }
                //Force Move
                else
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.FlatVelocityWithHorizontalTransition:
                //Flat Move
                if (character.IsRunning) 
                {
                    //Transition Time
                    if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneX(character.player.Input.MoveInput.x)) hTimeRunTransition = 0;
                    hTimeRunTransition += Time.deltaTime * 3;

                    //Movement input  
                    if (!character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZone(character.player.Input.MoveInput))
                        movementInput = new Vector3(Mathf.Lerp(0, character.player.Input.MoveInput.x,hTimeRunTransition), 0, character.player.Input.MoveInput.y);
                    else movementInput = Vector3.forward;
                    movement = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;

                    //Flat Velocity
                    character.Rigidbody.velocity = new Vector3(movement.x * 10f, character.Rigidbody.velocity.y, movement.z * 10f);
                }
                //Force Move
                else
                {
                    hTimeRunTransition = 0;
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                }
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.OnlyForwardFlatVelocityAtHighSpeed:
                //Flat Move
                if (character.IsRunning)
                {
                    if (character.CurrentMachRun >= 1)
                    {
                        //Transition Time
                        if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneX(character.player.Input.MoveInput.x)) hTimeRunTransition = 0;
                        hTimeRunTransition += Time.deltaTime * 3;

                        //Transition Time
                        if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneY(character.player.Input.MoveInput.y)) fTimeRunTransition = 0;
                        fTimeRunTransition += Time.deltaTime * 3;

                        //Movement input  
                        Vector3 hMoveInput = new Vector3(Mathf.Lerp(0, character.player.Input.MoveInput.x, hTimeRunTransition), 0, 0);
                        Vector3 fMoveInput = new Vector3(0, 0, Mathf.Lerp(0, character.player.Input.MoveInput.y, fTimeRunTransition));
                        if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneY(character.player.Input.MoveInput.y)) fMoveInput = new Vector3(0, 1, Mathf.Lerp(0, 1, fTimeRunTransition));

                        Vector3 hMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * hMoveInput * character.CurrentSpeed;
                        Vector3 fMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * fMoveInput * character.CurrentSpeed;

                        character.Rigidbody.AddForce(hMove * 20f, ForceMode.Force);
                        character.Rigidbody.velocity = new Vector3(fMove.x * 10f, character.Rigidbody.velocity.y, fMove.z * 10f);
                    }
                    else
                    {
                        hTimeRunTransition = 0;
                        fTimeRunTransition = 0;
                        character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                    }
                }
                //Force Move
                else
                {
                    hTimeRunTransition = 0;
                    fTimeRunTransition = 0;
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                }
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.OnlyForwardFlatVelocity2:

                movementInput = new Vector3(Mathf.Lerp(0, character.player.Input.MoveInput.x, hTimeRunTransition), 0, Mathf.Lerp(0, character.player.Input.MoveInput.y, fTimeRunTransition));

                //Transition Time
                if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneX(character.player.Input.MoveInput.x)) 
                    hTimeRunTransition = 0;
                else
                    hTimeRunTransition += Time.deltaTime * 3;

                //Transition Time
                if (character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneY(character.player.Input.MoveInput.y))
                    fTimeRunTransition = 0;
                else
                    fTimeRunTransition += Time.deltaTime * 3;

                hTimeRunTransition = Mathf.Clamp01(hTimeRunTransition);
                fTimeRunTransition = Mathf.Clamp01(fTimeRunTransition);

                //Flat Move
                if (character.CurrentMachRun > 0 && !character.WallJumping)
                {
                    float forceMultiplier;
                    Vector3 fMoveInput = Vector3.forward;
                    if (movementInput.z >= 0)
                        movementInput.z = 0; 

                    Vector3 move = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;
                    Vector3 fMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * fMoveInput * character.CurrentSpeed * 10;

                    if (movementInput.z < 0)
                    {
                        fMove = character.Rigidbody.velocity;
                        forceMultiplier = 5;
                    }
                    else forceMultiplier = 20;

                    character.Rigidbody.AddForce(move * forceMultiplier, ForceMode.Force);
                    character.Rigidbody.velocity = new Vector3(fMove.x, character.Rigidbody.velocity.y, fMove.z);
                }
                //Force Move
                else
                {
                    hTimeRunTransition = 0;
                    fTimeRunTransition = 0;
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);

                }
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.OnlyForwardFlatVelocity3:

                //Move input
                movementInput = new Vector3(character.player.Input.MoveInput.x, 0, character.player.Input.MoveInput.y);

                //Flat Move
                if ((character.CurrentMachRun > 0 && !character.WallJumping && character.player.Input.GeneralInputSettings.GamepadConfig.IsInMoveDeadZoneX(character.player.Input.MoveInput.x) && character.player.Input.MoveInput.y >= 0))
                {
                    float forceMultiplier;
                    Vector3 fMoveInput = Vector3.forward;
                    if (movementInput.z >= 0)
                        movementInput.z = 0;

                    Vector3 move = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * movementInput * character.CurrentSpeed;
                    Vector3 fMove = Quaternion.Euler(0, character.player.Cam.transform.eulerAngles.y, 0) * fMoveInput * character.CurrentSpeed * 10;

                    if (movementInput.z < 0)
                    {
                        fMove = character.Rigidbody.velocity;
                        forceMultiplier = 5;
                    }
                    else forceMultiplier = 20;

                    character.Rigidbody.AddForce(move * forceMultiplier, ForceMode.Force); //Horizontal Move
                    character.Rigidbody.velocity = new Vector3(fMove.x, character.Rigidbody.velocity.y, fMove.z); //Forward Move
                }
                //Force Move
                else
                {
                    character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                }
                Rotate();
                break;
            //------------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------------
            case PeppinoController.MoveForceMode.OnPlanet:
                character.Rigidbody.AddForce(movement * 10f, ForceMode.Force);
                Quaternion desiredRotation = Quaternion.LookRotation(movement.normalized, character.Rigidbody.transform.up);
                character.Rigidbody.transform.localEulerAngles = new Vector3(character.Rigidbody.transform.localEulerAngles.x, desiredRotation.eulerAngles.y, character.Rigidbody.transform.localEulerAngles.z);
                break;
            case PeppinoController.MoveForceMode.None:
                break;
        }
    }

    protected void CrouchPlayer(bool crouch)
    {
        character.Collider.enabled = !crouch;
        character.CrouchCollider.enabled = crouch;
    }

    protected void SwitchToCurrentMachRun(bool needsToPressRunInput = false)
    {
        if(!needsToPressRunInput || (needsToPressRunInput && character.player.Input.RunButtonInput.triggered))
        {
            switch (character.CurrentMachRun)
            {
                case -1:
                    character.StateManager.SwitchState(character.StateManager.States.IdleState);
                    return;
                case 0:
                    character.StateManager.SwitchState(character.StateManager.States.Run0State);
                    return;
                case 1:
                    character.StateManager.SwitchState(character.StateManager.States.Run1State);
                    return;
                case 2:
                    character.StateManager.SwitchState(character.StateManager.States.Run2State);
                    return;
                case 3:
                    character.StateManager.SwitchState(character.StateManager.States.Run3State);
                    return;
            }
        }

        //--------Idle--------
        character.StateManager.SwitchState(character.StateManager.States.IdleState);

    }

    protected void SwitchToMachRunBySpeed(bool needsToPressRunInput = false)
    {
        //--------Mach Run--------

        if (!needsToPressRunInput || (needsToPressRunInput && character.player.Input.RunButtonInput.triggered))
        {
            if (character.CurrentSpeed >= character.Run3Speed)
            {
                character.StateManager.SwitchState(character.StateManager.States.Run3State);
                return;
            }
            if (character.CurrentSpeed >= character.Run2Speed)
            {
                character.StateManager.SwitchState(character.StateManager.States.Run2State);
                return;
            }
            if (character.CurrentSpeed >= character.Run1Speed)
            {
                character.StateManager.SwitchState(character.StateManager.States.Run1State);
                return;
            }
            if (character.CurrentSpeed > character.WalkSpeed)
            {
                character.StateManager.SwitchState(character.StateManager.States.Run0State);
                return;
            }
        }

        //--------Walk / Idle--------

        if (character.CurrentSpeed > 0)
        {
            character.StateManager.SwitchState(character.StateManager.States.WalkState);
            return;
        }

        character.StateManager.SwitchState(character.StateManager.States.IdleState);
    }

    protected int GetMachRunBySpeed(float speed)
    {
        if (speed >= character.Run3Speed)
        {
            return 3;
        }
        if (speed >= character.Run2Speed)
        {
            return 2;
        }
        if (speed >= character.Run1Speed)
        {
            return 1;
        }
        if (speed > character.WalkSpeed)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    protected void SetSpeedToCurrentMachSpeed() => character.Rigidbody.velocity = character.Rigidbody.velocity.normalized * character.CurrentSpeed;

    protected void PlayAudio(AudioClip clip, bool loop)
    {
        if (clip == null) return;
        character.AudioSource.clip = clip;
        character.AudioSource.loop = loop;
        character.AudioSource.Play();
    }
    protected void StopAudio()
    {
        character.AudioSource.Stop();
        character.AudioSource.clip = null;
        character.AudioSource.loop = false;
    }
}
