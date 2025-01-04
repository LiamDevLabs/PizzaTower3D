using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;
    public static bool pause = false;
    public static bool unpausable = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private UI_Settings settings;

    private bool lockedButton; //es para que tenga un minimo de delay el boton

    private void Awake()
    {
        animator = pauseMenu.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (unpausable)
        {
            Pause(false);
            return;
        }


        if (!pause)
        {
            foreach (Player player in PlayerManager.players)
                if (player.Input.PauseButtonInput.down && !lockedButton)
                    PauseAnimation();

            if (Input.GetKeyDown(KeyCode.Escape))
                PauseAnimation();
        }
        else
        {
            foreach (Player player in PlayerManager.players)
                if (player.Input.PauseButtonInput.down)
                    AlternatePause();

            if (Input.GetKeyDown(KeyCode.Escape))
                AlternatePause();
        }
    }

    public void PauseAnimation()
    {
        //Si antes era pause...
        if (pause)
        {
            //Sacar la pausa
            Time.timeScale = 1;
            animator.SetBool("Pause", false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //Si antes NO era pause...
        else
        {
            //Poner pausa
            Enable(true);
            animator.SetBool("Pause", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        lockedButton = true;
        Invoke("UnlockButton", 0.2f);
        Debug.Log("Pausa " + pause);
    }

    public void AlternatePause()
    {
        //Si antes SI era pause...
        if (pause)
        {
            //QUITAR la pausa
            Time.timeScale = 1;
            Enable(false);
            settings.SaveSettings();
            animator.SetBool("Pause", false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //Si antes NO era pause
        else
        {
            //PONER pausa
            Time.timeScale = 0;
            Enable(true);
            animator.SetBool("Pause", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        pause = !pause;
        lockedButton = true;
        Invoke("UnlockButton", 0.2f);
    }

    public void Pause(bool pause)
    {

        //Si NO QUIERO pause...
        if (!pause)
        {
            //QUITAR la pausa
            Time.timeScale = 1;
            Enable(false);
            animator.SetBool("Pause", false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //Si QUIERO pause
        else
        {
            //PONER pausa
            Time.timeScale = 0;
            Enable(true);
            animator.SetBool("Pause", true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        lockedButton = true;
        Invoke("UnlockButton", 0.2f);
        UI_PauseMenu.pause = pause;
    }

    public void BackTo()
    {
        pause = false;
        Time.timeScale = 1;

        settings.SaveSettings();

        switch (GameManager.GetSceneName())
        {
            case "Lobby":
                MainDestroy();
                LoadScene.Load(gameManager.LevelSettings.Intro); //"Back to intro";
                break;
            case "TowerLobby":
                MainDestroy();
                LoadScene.Load(gameManager.LevelSettings.LobbyCharacter); //"Back to pizzeria";
                break;
            default:
                LoadScene.Load(gameManager.LevelSettings.LobbyLevels); //"Back to the tower";
                Pause(false);
                break;
        }
    }

    public void ResetScene()
    {
        pause = false;
        Pause(false);
        Time.timeScale = 1;
        settings.SaveSettings();

        switch (GameManager.GetSceneName())
        {
            case "Lobby":
                MainDestroy();
                break;
        }

        LoadScene.ResetScene();
    }

    public void Quit()
    {
        settings.SaveSettings();
        Application.Quit();
    }


    void Enable(bool enable)
    {
        foreach (Transform child in transform) if (child != pauseMenu.transform) child.gameObject.SetActive(false);
        pauseMenu.SetActive(enable);
    }

    void MainDestroy()
    {
        foreach (Player player in FindObjectsOfType<Player>()) Destroy(player.gameObject);
        Destroy(FindObjectOfType<UI_GameCanvas>().gameObject);
        Destroy(FindObjectOfType<GameManager>().gameObject);
        Destroy(GameObject.Find("AvatarCanvas"));
    }

    private void UnlockButton() => lockedButton = false;

    private bool CheckIsPaused()
    {
        foreach (Transform panel in transform)
        {
            if (panel.gameObject.activeSelf)
                return true;
        }
        return false;
    }
}