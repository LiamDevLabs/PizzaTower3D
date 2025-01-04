using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayersReady : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    int playersCount = 0;
    int playersReady = 0;

    public delegate void PlayerReadyAction(int playersReady);
    public event PlayerReadyAction OnPlayerReady;

    public void UpdatePlayersCount()
    {
        playersCount = PlayerManager.players.Count;
        playersReady = 0;

        foreach(var p in FindObjectsOfType<PlayerMenuController_ChooseAvatar>())
            p.ResetReady();
    }

    public void SetReadyToNextMinigame(bool ready)
    {
        if (ready)
            playersReady++;
        else if (playersReady > 0)
            playersReady--;

        if (playersCount == playersReady && playersCount > 0)
        {
            gameManager.PassToNextMinigame();
            playersReady= 0;
        }

        if (OnPlayerReady != null)
            OnPlayerReady.Invoke(playersReady);
    }

    public void SetReadyToLoadMinigame(bool ready)
    {
        //AGREGADO DESPUES
        gameManager.LoadSelectedMinigame(); 
        return;

        if (ready)
            playersReady++;
        else if (playersReady > 0)
            playersReady--;

        if (playersCount == playersReady && playersCount > 0)
        {
            gameManager.LoadSelectedMinigame();
            playersReady = 0;
        }

        if (OnPlayerReady != null)
            OnPlayerReady.Invoke(playersReady);
    }
}
