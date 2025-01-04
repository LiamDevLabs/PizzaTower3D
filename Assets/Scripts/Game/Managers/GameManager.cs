using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public UI_GameCanvas GameCanvas { get; private set; }
    [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
    [field:SerializeField] public LevelSettings LevelSettings { get; private set; }

    [field: SerializeField] public bool InGame { get; private set; } = false;

    static public LevelSettings.Level SelectedLevel { get; private set; }
    static private int currentRound = 0;

    //Agregar una lista de niveles para que se ejecuten en un orden (aleatorio) (no es aleatorio todavia)
    [SerializeField] public List<LevelSettings.Level>[] LevelsToPlay { get; private set; }

    private bool endScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameCanvas.gameObject);
        SetLevels();
    }

    private void SetLevels()
    {
        LevelsToPlay = new List<LevelSettings.Level>[LevelSettings.Rounds];
        currentRound = 0;

        for (int i = 0; i < LevelsToPlay.Length; i++)
            for (int j = 0; j < LevelSettings.LevelsList.Count; j++)
            {
                if (j == 0)
                {
                    LevelsToPlay[i] = new List<LevelSettings.Level>(LevelSettings.LevelsList.Count);
                }
                LevelsToPlay[i].Add(LevelSettings.LevelsList[j]);
            }
    }

    private void SelectMinigame()
    {
        //Si al querer seleccionar un nuevo nivel ya no quedan niveles (en esta ronda), pasar a la siguiente ronda
        if (LevelsToPlay[currentRound].Count == 0)
            currentRound++;

        //Si ya no quedan rondas de niveles, terminar la partida
        if (currentRound > LevelsToPlay.Length - 1)
        {
            GameCanvas.UnableAll();
            LoadScene.Load(LevelSettings.EndGameScene);
            endScene= true;
            return;
        }

        int randomSceneIndex = Random.Range(0, LevelsToPlay[currentRound].Count); // Elegir un index aleatorio entre el rango de los niveles que quedan en la ronda actual
        SelectedLevel = LevelsToPlay[currentRound].ToArray()[randomSceneIndex]; // Guardar en variable un nivel random disponible en la ronda actual    
        LevelsToPlay[currentRound].RemoveAt(randomSceneIndex); // Borrar nivel de las escenas disponibles en la ronda (para que no se vuelva a elegir)
    }

    public void PassToNextMinigame()
    {
        SelectMinigame();
        if(!endScene) GameCanvas.MinigameTutorial();
    }

    public void LoadSelectedMinigame()
    {
        LoadScene.Load(SelectedLevel.SceneName); //Cargar nivel seleccionado
        GameCanvas.UnableAll(); //Deshabilitar todas las interfaces de menús
        //PlayerManager.EnablePlayers(false); //Desactivar jugadores (después del contador del minijuego se vuelven a activar)
    }

    public void ShowRanking()
    {
        LoadScene.Load(LevelSettings.LobbyLevels);
        GameCanvas.RankingView();
    }

    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name != LevelSettings.LobbyCharacter && SceneManager.GetActiveScene().name != LevelSettings.LobbyLevels && SceneManager.GetActiveScene().name != LevelSettings.EndGameScene)
        {
            //Desactivar jugadores
            //PlayerManager.EnablePlayers(false);

            //Activar UI de minijuego
            GameCanvas.GameView();
            //yield return new WaitUntil(() => GameCanvas.countdownPanel.Ended == true);

            //Activar jugadores
            PlayerManager.EnablePlayers(true);

            InGame = true;
        }
        else
            InGame = false;          


        if(InGame)
            PlayerManager.PlayerInputManager.DisableJoining();
        else
        {
            PlayerManager.PlayerInputManager.EnableJoining();
            if (SceneManager.GetActiveScene().name == LevelSettings.LobbyCharacter)
                SetLevels();
        }
    }

    public static void Loose() => SceneManager.LoadScene(FindObjectOfType<GameManager>().LevelSettings.LooseScene);
    public static void LoadLobby() => SceneManager.LoadScene(FindObjectOfType<GameManager>().LevelSettings.LobbyLevels);
    public static void LoadLevel(string scene) => SceneManager.LoadScene(scene);
    public static void LoadLevelFromList(string scene) => SceneManager.LoadScene(FindObjectOfType<GameManager>().LevelSettings.LevelsList.Where(level => level.SceneName == scene).Select(level => level.SceneName).SingleOrDefault());
    public static string GetSceneName() => SceneManager.GetActiveScene().name;
    public static int GetSceneIndex() => SceneManager.GetActiveScene().buildIndex;
}
