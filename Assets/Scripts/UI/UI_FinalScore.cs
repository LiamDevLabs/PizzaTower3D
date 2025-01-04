using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FinalScore : MonoBehaviour
{
    [SerializeField] private AnimatorNamedParameters animatorParameters;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    public void SetScore(PlayerScore.Ranks rank, int score)
    {
        switch (rank)
        {
            case PlayerScore.Ranks.D: animatorParameters.SetString("D"); break;
            case PlayerScore.Ranks.C: animatorParameters.SetString("C"); break;
            case PlayerScore.Ranks.B: animatorParameters.SetString("B"); break;
            case PlayerScore.Ranks.A: animatorParameters.SetString("A"); break;
            case PlayerScore.Ranks.S: animatorParameters.SetString("S"); break;
            case PlayerScore.Ranks.P: animatorParameters.SetString("P"); break;
            case PlayerScore.Ranks.P2: animatorParameters.SetString("P2"); break;
        }

        scoreText.text = "" + score;

    }
}
