using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScoreManager : MonoBehaviour
{
    int maxCollectableScorePossible;
    enum RankCalculationType
    {
        ScoreNumber, Percentage
    }

    [SerializeField] RankCalculationType rankCalculationType;
    [SerializeField] private int cRankMinValue, bRankMinValue, aRankMinValue, sRankMinValue;
    [SerializeField] private bool includeInactiveCollectablesAsTotal;
    [SerializeField] private bool isBossFight;
    [SerializeField] private bool hasLap = true;

    private int cMin, bMin, aMin, sMin;

    GameManager gameManager;
    PlayerCombo playerCombo;

    private void Awake()
    {
        //Obtener la cantidad de puntos maximos posibles en el nivel (a través de las cosas recolectables)
        maxCollectableScorePossible = 0;
        foreach (Collectable collectable in FindObjectsOfType<Collectable>(includeInactiveCollectablesAsTotal))
            maxCollectableScorePossible += collectable.Score;

        //Calcular el puntaje minimo para cada rango
        switch (rankCalculationType)
        {
            case RankCalculationType.ScoreNumber:
                cMin = cRankMinValue;
                bMin = bRankMinValue;
                aMin = aRankMinValue;
                sMin = sRankMinValue;
                break;
            case RankCalculationType.Percentage:
                cMin = GetScoreByPercentage(cRankMinValue);
                bMin = GetScoreByPercentage(bRankMinValue);
                aMin = GetScoreByPercentage(aRankMinValue);
                sMin = GetScoreByPercentage(sRankMinValue);
                break;
        }
    }
    private int GetScoreByPercentage(float percentage) => (int)percentage * maxCollectableScorePossible / 100;

    public PlayerScore.Ranks GetRankByScore(int score, int hits, bool firstCombo = false, int lap=1)
    {
        if (isBossFight)
            return GetRanksScore_BossFight(hits, score);
        else
            return GetRanksScore_Default(score, firstCombo, lap);
    }

    private PlayerScore.Ranks GetRanksScore_Default(int score, bool firstCombo = false, int lap = 1)
    {
        if (score >= sMin && (lap > 1 || !hasLap))
        {
            if (firstCombo)
            {
                if (lap == 2 || !hasLap)
                    return PlayerScore.Ranks.P;
                else
                    return PlayerScore.Ranks.P2;
            }
            else
                return PlayerScore.Ranks.S;
        }
        else if (score >= aMin) return PlayerScore.Ranks.A;
        else if (score >= bMin) return PlayerScore.Ranks.B;
        else if (score >= cMin) return PlayerScore.Ranks.C;
        else return PlayerScore.Ranks.D;
    }

    private PlayerScore.Ranks GetRanksScore_BossFight(int hits, int score)
    {
        if (hits <= 0)
        {
            if(score >= sMin)
                return PlayerScore.Ranks.P2;
            else
                return PlayerScore.Ranks.P;
        }
        else if (hits <= 2) return PlayerScore.Ranks.S;
        else if (hits <= 4) return PlayerScore.Ranks.A;
        else if (hits <= 6) return PlayerScore.Ranks.B;
        else if (hits <= 8) return PlayerScore.Ranks.C;
        else return PlayerScore.Ranks.D;
    }

    public void Finish()
    {
        FindObjectOfType<PlayerCombo>().CalculateComboScore();
        SecretPortal.SaveAllFoundSecrets();
        gameManager = FindObjectOfType<GameManager>();
        SceneManager.LoadScene(gameManager.LevelSettings.ScoreScene);
    }
}
