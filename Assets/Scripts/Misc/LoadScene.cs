using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;

    static public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    static public void Load(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }

    static public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLobby() => GameManager.LoadLobby();
}
