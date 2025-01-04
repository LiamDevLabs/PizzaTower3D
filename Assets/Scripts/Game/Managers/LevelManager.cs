using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameManager GameManager { get; private set; }

    [SerializeField] private Transform spawnpointsParent;

    [SerializeField] private AvatarSettings.ControllerType controllerType;

    private class Winner
    {
        private static int winnersCount = 0;
        public int MinigameRanking = 0;

        public Player Player { get; private set; }

        public Winner (Player player)
        {
            Player = player;
            MinigameRanking = winnersCount;
            winnersCount++;

            player.PlayerController.GameOver(MinigameRanking <= 3);

            //int scoreToAdd = GameRankingManager.maxScorePerMinigame - MinigameRanking * GameRankingManager.ScoreDifferenceBetweenRankings();
            //if (scoreToAdd > 0) player.Score.UpdateScore = scoreToAdd;

            //Debug.Log(scoreToAdd + " = " + " " + GameRankingManager.maxScorePerMinigame + " - " + MinigameRanking + " * " + GameRankingManager.ScoreDifferenceBetweenRankings());
        }

        static public void ResetWinnersCount() => winnersCount = 0;
    }
    private List<Winner> winnersList = new List<Winner>();

    public bool spawnOnAwake = true;
    bool spawned = false;

    [Tooltip("Se desactivarán todos los GameObject que se introduzcan acá, excepto el primero")]
    [SerializeField] private GameObject[] rooms;

    private void Awake()
    {
        spawned = false;
        Winner.ResetWinnersCount();
        GameManager = FindObjectOfType<GameManager>();
    }

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        if (rooms != null && rooms.Length > 0)
        {
            foreach (GameObject room in rooms) room.SetActive(false);
            rooms[0].SetActive(true);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(spawnOnAwake && !spawned)
            SpawnAvatars();
    }

    private void OnEnable()
    {
        if (spawnOnAwake && !spawned)
            SpawnAvatars();
    }

    public void SpawnAvatars()
    {
        if (spawned) return;

        int spawnpointIndex=0;
        foreach (Player p in PlayerManager.players)
        {
            //Set Spawn Point
            Transform spawnpoint = spawnpointsParent.GetChild(spawnpointIndex);
            if(spawnpointIndex > spawnpointsParent.childCount-1)
                spawnpointIndex = 0;

            //Spawn Avatar
            PlayerBaseController playerController = Instantiate(p.choosenAvatar.GetAvatar(controllerType), spawnpoint.position, spawnpoint.rotation);
            playerController.player = p;
            p.PlayerController = playerController;
            spawnpointIndex++;
        }

        spawned = true;
    }

    public void AddWinner(Player player)
    {
        //Cualquiera puede ganar menos el ultimo
        if (winnersList.Count != PlayerManager.players.Count - 1)
            winnersList.Add(new Winner(player)); //Agregar ganador a la lista
        
        //Si todos menos el ultimo ganaron, terminar el minijuego
        if (winnersList.Count == PlayerManager.players.Count - 1)
            End(); 

    }

    public void End()
    {
        GameManager.ShowRanking();
    }

    public List<Player> GetWinners()
    {
        List<Player> winners = new List<Player>();
        foreach(Winner winner in winnersList) winners.Add(winner.Player);
        return winners;
    }


}
