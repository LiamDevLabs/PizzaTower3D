using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class GameRankingManager
{
    static public int maxScorePerMinigame = 100;

    static public int ScoreDifferenceBetweenRankings()
    {
       switch (PlayerManager.players.Count)
       {
                //Si hay 2 jugadores
                case 2:
                    return 60;
                //Si hay 3 jugadores
                case 3:
                    return 30;
                //Si hay 4 jugadores
                case 4:
                    return 20;
                //Si hay cualquier cantidad de jugadores que no sean las anteriores
                default:
                    return 10;
       }
    }

    static public int GetPlayerRanking(Player player)
    {
        int i = 0;
        foreach(Player p in GetPlayersSortedByScore())
        {
            if (p == player)
                return i;
            i++;
        }
        return -1;
    }

    static public IEnumerable<Player> GetPlayersSortedByScore()
    {
        return PlayerManager.players.OrderByDescending(p => p.Score);
    }

    static public void UpdatePlayersScore()
    {
        foreach(Player p in PlayerManager.players)
        {
            p.Score.UpdateScore(0);
        }
    }
}
