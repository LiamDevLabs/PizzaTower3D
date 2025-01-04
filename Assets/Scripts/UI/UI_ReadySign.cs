using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ReadySign : MonoBehaviour
{
    [SerializeField] private CheckPlayersReady checkPlayersReady;
    [SerializeField] private GameObject sign;

    void Awake() => checkPlayersReady.OnPlayerReady += SignState;

    private void OnEnable()
    {
        sign.SetActive(true);
    }

    void SignState(int playersReady) => sign.SetActive(playersReady == 0);
}
