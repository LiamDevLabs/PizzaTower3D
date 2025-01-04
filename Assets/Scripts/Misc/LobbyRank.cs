using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRank : MonoBehaviour
{
    [SerializeField] private string levelName;

    [Tooltip("0=d, 1=c, 2=b, 3=a, 4=s, 5=p")]
    [SerializeField] GameObject[] ranks;
    [SerializeField] GameObject nextLevelBarrier;
    [SerializeField] private bool invertBarrierState;

    [SerializeField] TMPro.TextMeshPro score;

    void Awake()
    {
        if (PlayerPrefs.HasKey("Rank_"+levelName))
        {
            foreach (GameObject rank in ranks) rank.SetActive(false);
            int rankId = PlayerPrefs.GetInt("Rank_" + levelName);
            ranks[rankId].SetActive(true);
            score.text = "" + PlayerPrefs.GetInt("Score_" + levelName);
            if(nextLevelBarrier)nextLevelBarrier.SetActive(invertBarrierState);
        }
        else
        {
            foreach (GameObject rank in ranks) rank.SetActive(false);
            score.gameObject.SetActive(false);
            if (nextLevelBarrier) nextLevelBarrier.SetActive(!invertBarrierState);
        }
    }
}
