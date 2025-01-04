using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayersRankingList : MonoBehaviour
{
    [SerializeField] UI_PlayerInRanking playerInRankingPrefab;
    [SerializeField] RectTransform playersPanelParent;
    [SerializeField] TextMeshProUGUI readySign;
    [SerializeField] float startVerticalPosition;
    [SerializeField] float spaceBetweenPlayers;

    [SerializeField] float timeToStartScoreAnimation = 2;
    [SerializeField] float scoreAnimationTime = 1;
    List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();

    bool scoreAnimationFinished = false;

    private void OnEnable()
    {
        StartCoroutine(ShowRankingCoroutine());
    }

    IEnumerator ShowRankingCoroutine()
    {
        readySign.enabled = false;
        PlayerManager.EnablePlayers(false);
        ClearPanel();
        AddPlayersInRanking();
        yield return new WaitForSeconds(timeToStartScoreAnimation);
        StartCoroutine(ScoreAnimation(scoreTexts));
        yield return new WaitUntil(() => scoreAnimationFinished == true);
        ClearPanel();
        GameRankingManager.UpdatePlayersScore();
        AddPlayersInRanking();
        PlayerManager.EnablePlayers(true);
        readySign.enabled = true;
    }

    private void AddPlayersInRanking()
    {
        int i = 0;
        scoreTexts.Clear();
        foreach (Player p in GameRankingManager.GetPlayersSortedByScore())
        {
            UI_PlayerInRanking playerInRanking = Instantiate(playerInRankingPrefab, playersPanelParent);

            playerInRanking.SetPlayer(p);

            if (playerInRanking.TryGetComponent(out RectTransform rectTransform))
            {
                rectTransform.anchoredPosition = new Vector3(i * spaceBetweenPlayers, startVerticalPosition, 0);
            }

            scoreTexts.Add(playerInRanking.ScoreText);

            //Debug.Log(p.choosenAvatar.Name + " " + i);

            i++;
        }
    }

    private IEnumerator ScoreAnimation(List<TextMeshProUGUI> texts)
    {
        scoreAnimationFinished = false;
        float currentTime = 0;

        while (currentTime < scoreAnimationTime)
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = "" + Random.Range(0, 9999999);
            }

            currentTime += Time.deltaTime / 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        scoreAnimationFinished =true;
    }

    private void ClearPanel()
    {
        if (playersPanelParent.childCount == 0) return;

        foreach (Transform child in playersPanelParent)
        {
            Destroy(child.gameObject);
        }

        scoreTexts.Clear();
    }
}
