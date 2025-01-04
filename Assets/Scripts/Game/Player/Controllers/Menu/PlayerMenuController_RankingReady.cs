using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuController_RankingReady : PlayerBaseController
{
    public PlayerMenuController_RankingReady(Player player) : base(player)
    {
        this.player = player;
    }

    private CheckPlayersReady checkReady;
    [SerializeField] private UI_PlayerInRanking playerInRanking;
    [SerializeField] private Image checkMark;

    public bool ReadyInput { get; private set; }
    private bool readySet = false;

    protected override void Start()
    {
        player = playerInRanking.Player;
        playerInRanking.Player.PlayerController = this;
        checkReady = FindObjectOfType<CheckPlayersReady>();

        base.Start();
    }

    protected override void SetSettings() { }

    public override void PlayerStart()  { }

    public override void PlayerUpdate()
    {
        if (!ReadyInput)
        {
            ReadyInput = player.Input.RunButtonInput.down || player.Input.GrabButtonInput.down || player.Input.ParryButtonInput.down;
        }
        else
        {
            if (!readySet)
            {
                checkMark.enabled = true;
                checkReady.SetReadyToNextMinigame(true);
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
