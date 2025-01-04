using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInRanking : MonoBehaviour
{
    public Player Player { get; private set; }

    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [field:SerializeField] public TextMeshProUGUI ScoreText {get; private set;}

    public void SetPlayer(Player player)
    {
        Player = player;
        rankingText.text = "" + (GameRankingManager.GetPlayerRanking(player)+1);
        avatarImage.sprite = player.choosenAvatar.Icon;
        nameText.text = player.choosenAvatar.Name;
        ScoreText.text = ""+player.Score;
    }

}
