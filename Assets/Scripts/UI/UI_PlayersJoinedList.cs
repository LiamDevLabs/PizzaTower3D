using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayersJoinedList : MonoBehaviour
{
    [SerializeField] UI_PlayerJoined playerJoinedPrefab;
    [SerializeField] RectTransform playersPanelParent;
    [SerializeField] float spaceBetweenPlayersUI;
    [SerializeField] float spaceBetweenPlayers3D;
    [SerializeField] Vector3 positionPlayers3D;
    [SerializeField] Vector3 anglePlayers3D;
    //[SerializeField] bool enableSpaceOnCorners;

    public void UpdateUI()
    {
        ClearPanel();
        StartCoroutine(AddPlayers(PlayerManager.players));
    }

    private void ClearPanel()
    {
        foreach (Transform child in playersPanelParent)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator AddPlayers(List<Player> players)
    {
        int i = players.Count-1;
        foreach (Player p in players)
        {
            UI_PlayerJoined playerJoined = Instantiate(playerJoinedPrefab, playersPanelParent);

            yield return null;

            playerJoined.SetPlayer(p, positionPlayers3D, anglePlayers3D, i * spaceBetweenPlayers3D);

            if (playerJoined.TryGetComponent(out RectTransform rectTransform))
            {
                rectTransform.anchoredPosition = new Vector3(i * spaceBetweenPlayersUI, 0, 0);
            }

            i--;
        }
    }
}
