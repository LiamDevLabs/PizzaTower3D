using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public static List<Player> players { get; private set;} = new List<Player>();

    [field: SerializeField] public PlayerInputManager PlayerInputManager { get; private set; }
    [field: SerializeField] public CheckPlayersReady CheckPlayersReady { get; private set; }

    [field:SerializeField] public AvatarSettings AvatarSettings { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdatePlayers()
    {
        players = new List<Player>(FindObjectsOfType<Player>());
        
        if(gameManager.GameCanvas.playersJoinedList)
            gameManager.GameCanvas.playersJoinedList.UpdateUI();

        CheckPlayersReady.UpdatePlayersCount();
    }

    static public void EnablePlayers(bool enable) { foreach (Player player in players) player.enabled = enable; }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != gameManager.LevelSettings.LobbyCharacter && gameManager.GameCanvas.playersJoinedList)
            gameManager.GameCanvas.playersJoinedList.gameObject.SetActive(false);
    }
}
