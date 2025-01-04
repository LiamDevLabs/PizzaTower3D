using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuController_TutorialReady : PlayerBaseController
{
    public PlayerMenuController_TutorialReady(Player player) : base(player)
    {
        this.player = player;
    }

    private CheckPlayersReady checkReady;
    [SerializeField] private Image checkMark;

    public bool ReadyInput { get; private set; }
    private bool readySet = false;
    float startedTime;

    protected override void Start()
    {
        startedTime = Time.time;
        checkReady = FindObjectOfType<CheckPlayersReady>();
        base.Start();

        //AGREGADO DESPUES
        checkReady.SetReadyToLoadMinigame(true);
    }

    protected override void SetSettings() { }

    public override void PlayerStart() 
    { }

    public override void PlayerUpdate()
    {
        //AGREGADO DESPUES 
        return; 

        
        if (Time.time > startedTime + 0.1f) //Solucion a que: apenas se une el jugador, marca el ready y empieza la partida.
        if (!ReadyInput)
        {
            ReadyInput = player.Input.JumpButtonInput.down;
        }
        else
        {
            if (!readySet)
            {
                checkMark.enabled = true;
                checkReady.SetReadyToLoadMinigame(true);
                readySet= true;
            }         
        }
    }

    public override void PlayerFixedUpdate() { }

    public override void PlayerLateUpdate()
    {
    }

    private void OnLevelWasLoaded()
    {
        ReadyInput = false;
        readySet = false;
        checkMark.enabled = false;
    }

}
