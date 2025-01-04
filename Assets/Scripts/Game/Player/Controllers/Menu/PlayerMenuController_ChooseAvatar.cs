using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuController_ChooseAvatar : PlayerBaseController
{
    public PlayerMenuController_ChooseAvatar(Player player) : base(player)
    {
        this.player = player;
    }

    PlayerManager playerManager;
    [HideInInspector] public UI_PlayerJoined playerJoinedUI;
    private int avatarIndex = 0;
    private bool enableToChangeAvatar = true;

    public bool ReadyInput { get; private set; }
    private bool readySet = false;
    private CheckPlayersReady checkReady;

    float startedTime;
    float readyDelay = 0.15f;
    float changedReadyTime;

    protected override void SetSettings() { }

    protected override void Start()
    {
        readyDelay = 0.15f;
        checkReady = FindObjectOfType<CheckPlayersReady>();
        base.Start();
    }

    public override void PlayerStart()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        startedTime = Time.time;
    }

    public override void PlayerUpdate()
    {
        Vector2 inputDirection = player.Input.MoveInput;

        if (enableToChangeAvatar)
        {
            if (inputDirection.x > 0)
            {
                avatarIndex++;
                if (avatarIndex > playerManager.AvatarSettings.AvatarList.Count - 1)
                    avatarIndex = 0;

                player.choosenAvatar = playerManager.AvatarSettings.AvatarList[avatarIndex];  //selecciona el avatar
                playerJoinedUI.ChangeAvatarImage(player.choosenAvatar.Icon);
                enableToChangeAvatar = false;
            }

            if (inputDirection.x < 0f)
            {
                avatarIndex--;
                if (avatarIndex < 0)
                    avatarIndex = playerManager.AvatarSettings.AvatarList.Count-1;

                player.choosenAvatar = playerManager.AvatarSettings.AvatarList[avatarIndex]; //selecciona el avatar
                playerJoinedUI.ChangeAvatarImage(player.choosenAvatar.Icon); 
                enableToChangeAvatar = false;
            }
        }

        if(inputDirection.x == 0 )
        {
            enableToChangeAvatar = true;
        }


        /*
          
        Voy a justificar el código complejo y horrible de abajo con lo siguientes problemas que tuve:

        PROBLEMAS:
        
        1-Apenas se une un jugador, marca el ready.
        2-Agregar que se pueda marcar y desmarcar el ready
        3-Como el input.down se "aprieta" dos veces (por alguna razónnnnnn), la marca del ready se marca y se vuelve a desmarcar (vuelve al mismo estado que antes).

        PD: En realidad, ahora me di cuenta de que el problema numero 1 es el mismo problema que el 3. Creo que podria haberlo solucionado solo dejando la "solución 3", pero bueno ahora queda asi.  

        */

        ReadyInput = player.Input.JumpButtonInput.down;

        if (Time.time > startedTime + 0.1f) // solución 1
        if (ReadyInput)
        {
                if(Time.time > changedReadyTime + readyDelay) // solución 3
                if (!readySet) // solución 2
                {
                    playerJoinedUI.ReadyCheckMark(true);
                    checkReady.SetReadyToNextMinigame(true);
                    changedReadyTime= Time.time;
                    readySet = true;
                }
                else
                {
                    playerJoinedUI.ReadyCheckMark(false);
                    checkReady.SetReadyToNextMinigame(false);
                    changedReadyTime = Time.time;
                    readySet = false;
                }
        }
    }

    public void ResetReady() => readySet = false;

    public override void PlayerFixedUpdate()
    {

    }

    public override void PlayerLateUpdate()
    {
    }

}
