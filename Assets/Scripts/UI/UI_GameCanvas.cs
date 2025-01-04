using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameCanvas : MonoBehaviour
{
    [Header("Lobby")]
    public GameObject lobbyPanel;
    public UI_PlayersJoinedList playersJoinedList;

    [Header("Before-Game")]
    public UI_MinigameTutorial minigameTutorial;

    [Header("In-Game")]
    public UI_Countdown countdownPanel;
    public UI_Clock clock;

    [Header("Ranking")]
    public UI_PlayersRankingList ranking;

    private void Awake()
    {
        //LobbyView();
        //DontDestroyOnLoad(lobbyPanel);
    }

    public void UnableAll()
    {
        //lobbyPanel.SetActive(false);
        minigameTutorial.gameObject.SetActive(false);
        countdownPanel.gameObject.SetActive(false);
        ranking.gameObject.SetActive(false);
        clock.gameObject.SetActive(false);
        UI_PauseMenu.pause = false;
    }

    public void LobbyView()
    {
        UnableAll();
        lobbyPanel.SetActive(true);
    }

    public void MinigameTutorial()
    {
        UnableAll();
        minigameTutorial.gameObject.SetActive(true);
    }

    public void RankingView()
    {
        UnableAll();
        ranking.gameObject.SetActive(true);
    }

    public void GameView()
    {
        UnableAll();
        countdownPanel.gameObject.SetActive(true);
        clock.gameObject.SetActive(true);
    }


}
