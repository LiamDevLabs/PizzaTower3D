using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsList", menuName = "Liam/Level/LevelsList")]
public class LevelSettings : ScriptableObject
{

    [System.Serializable]
    public class Level
    {
        [field: SerializeField] public string SceneName { get; private set; }
        [field: SerializeField] public GameObject Tutorial { get; private set; }
    }

    [field: SerializeField] public string Intro { get; private set; } = "Intro";
    [field: SerializeField] public string LobbyCharacter { get; private set; } = "Lobby";
    [field:SerializeField] public string LobbyLevels { get; private set; } = "TowerLobby";
    [field: SerializeField] public string LooseScene { get; private set; } = "TimesUp";
    [field: SerializeField] public string ScoreScene { get; private set; } = "ScoreScene";
    [field: SerializeField] public string EndGameScene { get; private set; } = "EndGameScene";

    [field: SerializeField] public List<Level> LevelsList { get; private set; }
    [field: SerializeField] public int Rounds { get; private set; } = 2;

}
