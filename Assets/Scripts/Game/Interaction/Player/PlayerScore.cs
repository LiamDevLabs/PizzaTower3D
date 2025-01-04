using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScore : MonoBehaviour
{
    public enum Ranks
    {
        D,C,B,A,S,P,P2
    }
    public int Score { get; private set; }
    public Ranks CurrentRank { get; private set; }

    int lap = 1;

    [HideInInspector] public PlayerHealth health;
    [SerializeField] private PlayerCombo combo;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private Animator rankAnimator;
    [SerializeField] private UI_HudReferences hudReferences;

    Ranks previousRank = Ranks.D;

    [SerializeField] private GameObject canvasUI;
    GameManager gameManager;
    LevelScoreManager levelScoreManager;
    bool intialize;
    string levelName;

    private void Awake() => Initialize();

    private void OnLevelWasLoaded()
    {
        //On level
        Initialize();

        //On finish level
        if(gameManager.LevelSettings.ScoreScene == SceneManager.GetActiveScene().name)
        {
            UI_FinalScore finalScore = FindObjectOfType<UI_FinalScore>();
            finalScore.SetScore(CurrentRank, Score);
            SaveScore();
        } 
    }

    public void Initialize()
    {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>(); //Is always
        levelScoreManager = FindObjectOfType<LevelScoreManager>();       //Is in specific level
        canvasUI.SetActive(levelScoreManager != null);                   //UI
        if (!levelScoreManager) return;                                  //Don't initialize if isn't levelScoreManager
        if(gameManager.LevelSettings.ScoreScene != SceneManager.GetActiveScene().name) levelName = SceneManager.GetActiveScene().name; //Get level name

        //Start from 0
        Score = 0;
        //Rank
        CurrentRank = Ranks.D;
        //Laps
        lap = 1; LapPortal[] lapPortals = FindObjectsOfType<LapPortal>(true); if (lapPortals.Length>0) { foreach(LapPortal lapPortal in lapPortals) lapPortal.OnTeleport += CountLaps; }  
        //UpdateScore
        UpdateScore(0);
    }

    private void CountLaps()
    {
        lap++;
        UpdateScore(0);
    }

    public void UpdateScore(int addedOrSubtractedScore=0)
    {
        //Score
        Score += addedOrSubtractedScore;
        //HUD
        scoreText.text = "" + Score;
        switch (levelScoreManager.GetRankByScore(Score, health.Hits, combo.PerfectCombo, lap))
        {
            case Ranks.D: hudReferences.ChangeToRank("d");  CurrentRank = Ranks.D; break;
            case Ranks.C: hudReferences.ChangeToRank("c");  CurrentRank = Ranks.C; break;
            case Ranks.B: hudReferences.ChangeToRank("b");  CurrentRank = Ranks.B; break;
            case Ranks.A: hudReferences.ChangeToRank("a");  CurrentRank = Ranks.A; break;
            case Ranks.S: hudReferences.ChangeToRank("s");  CurrentRank = Ranks.S; break;
            case Ranks.P: hudReferences.ChangeToRank("p");  CurrentRank = Ranks.P; break;
            case Ranks.P2:hudReferences.ChangeToRank("p+"); CurrentRank = Ranks.P2; break;
        }

        //Animator
        if (previousRank != CurrentRank)
        {
            rankAnimator.SetTrigger("Change");
            previousRank = CurrentRank;
        }
    }

    private void SaveScore()
    {
        //Current Rank to int
        int rank = 0;
        switch (CurrentRank)
        {
            case Ranks.D: rank = 0; break;
            case Ranks.C: rank = 1; break;
            case Ranks.B: rank = 2; break;
            case Ranks.A: rank = 3; break;
            case Ranks.S: rank = 4; break;
            case Ranks.P: rank = 5; break;
            case Ranks.P2: rank = 6; break;
        }

        //Check if the previous score is better
        if (PlayerPrefs.HasKey("Score_" + levelName))
        {
            int previousRank = PlayerPrefs.GetInt("Rank_" + levelName);
            int previousScore = PlayerPrefs.GetInt("Score_" + levelName);
            if (previousRank > rank || (previousRank == rank && previousScore > Score)) return;
        }

        //Save
        PlayerPrefs.SetInt("Rank_" + levelName, rank);
        PlayerPrefs.SetInt("Score_" + levelName, Score);
    }
}
