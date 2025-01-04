//using InputBuffering;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public DeviceSettings.ControllerType ControllerType { get; private set; }
    [field: SerializeField] public GeneralInputSettings GeneralInputSettings { get; private set; }

    public class PlayerButton
    {
        //New input system
        public bool triggered = false;
        public bool started = false;
        public bool canceled = true;

        //Liam
        public bool down = false;
        public bool up = false;
    }

    //private InputBuffer inputBuffer = new InputBuffer(5, 1.5f);
    //public InputBuffer Buffer { get { return inputBuffer; } }

    //Inputs
    public Vector2 MoveInput { get; private set; }
    public PlayerButton RunButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton JumpButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton GrabButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton ParryButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton CrouchButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton AlternateViewButtonInput { get; private set; } = new PlayerButton();
    public PlayerButton PauseButtonInput { get; private set; } = new PlayerButton();


    private void Awake() => PlayerInput = GetComponent<PlayerInput>();

    public void SetControllerType(PlayerInput playerInput)
    {
        string deviceString = playerInput.devices[0].displayName;

        if (deviceString.Contains("Keyboard", System.StringComparison.InvariantCultureIgnoreCase))
            ControllerType = DeviceSettings.ControllerType.Keyboard;

        if (deviceString.Contains("Xbox", System.StringComparison.InvariantCultureIgnoreCase))
            ControllerType = DeviceSettings.ControllerType.Xbox;

        if (deviceString.Contains("Playstation", System.StringComparison.InvariantCultureIgnoreCase) || deviceString.Contains("PS3", System.StringComparison.InvariantCultureIgnoreCase) || deviceString.Contains("PS4", System.StringComparison.InvariantCultureIgnoreCase) || deviceString.Contains("Controller", System.StringComparison.InvariantCultureIgnoreCase))
            ControllerType = DeviceSettings.ControllerType.Playstation;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnRunButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        RunButtonInput.triggered = context.action.triggered;
        RunButtonInput.started = context.started;
        RunButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(RunButtonInput, context.action.triggered));
    }

    public void OnJumpButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        JumpButtonInput.triggered = context.action.triggered;
        JumpButtonInput.started = context.started;
        JumpButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(JumpButtonInput, context.action.triggered));
    }

    public void OnGrabButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        GrabButtonInput.triggered = context.action.triggered;
        GrabButtonInput.started = context.started;
        GrabButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(GrabButtonInput, context.action.triggered));

    }

    public void OnParryButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        ParryButtonInput.triggered = context.action.triggered;
        ParryButtonInput.started = context.started;
        ParryButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(ParryButtonInput, context.action.triggered));
    }

    public void OnCrouchButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        CrouchButtonInput.triggered = context.action.triggered;
        CrouchButtonInput.started = context.started;
        CrouchButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(CrouchButtonInput, context.action.triggered));
    }

    public void OnAlternateViewButton(InputAction.CallbackContext context)
    {
        if (UI_PauseMenu.pause) return;

        AlternateViewButtonInput.triggered = context.action.triggered;
        AlternateViewButtonInput.started = context.started;
        AlternateViewButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(AlternateViewButtonInput, context.action.triggered));
    }

    public void OnPauseButton(InputAction.CallbackContext context)
    {
        PauseButtonInput.triggered = context.action.triggered;
        PauseButtonInput.started = context.started;
        PauseButtonInput.canceled = context.canceled;

        StartCoroutine(OneTapInput(PauseButtonInput, context.action.triggered));
    }

    private IEnumerator OneTapInput(PlayerButton playerButton, bool pressed)
    {
        if (pressed)
        {
            if (playerButton.down == false)
            {
                playerButton.down = true;
                yield return null;
                playerButton.down = false;
            }
        }
        else
        {
            if (playerButton.up == false)
            {
                playerButton.up = true;
                yield return null;
                playerButton.up = false;
            }
        }
    }

    private void Update()
    {
        //Debug.Log("player.Input.BasicButtonInput.started =" + BasicButtonInput.started);
        //Debug.Log("player.Input.BasicButtonInput.canceled =" + BasicButtonInput.canceled);
        //Debug.Log("player.Input.BasicButtonInput.triggered =" + BasicButtonInput.triggered);
        //Debug.Log("player.Input.SpecialButtonInput.started =" + SpecialButtonInput.started);
        //Debug.Log("player.Input.SpecialButtonInput.canceled =" + SpecialButtonInput.canceled);
        //Debug.Log("player.Input.SpecialButtonInput.triggered =" + SpecialButtonInput.triggered);
        //if (BasicButtonInput.down) Debug.Log("player.Input.BasicButtonInput.down =" + BasicButtonInput.down);
        //if (BasicButtonInput.up) Debug.Log("player.Input.BasicButtonInput.up =" + BasicButtonInput.up);
        //if (BasicButtonInput.previousPressed) Debug.Log("player.Input.BasicButtonInput.previousPressed =" + BasicButtonInput.previousPressed);
    }
}
