using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class UI_PauseMenuAnimator : MonoBehaviour
{
    UI_PauseMenu menu;
    [SerializeField] TextMeshProUGUI quitText;

    private void Awake()
    {
        menu = GetComponentInParent<UI_PauseMenu>();
        UpdateQuitText();
    }
  
    private void Pause() => menu.AlternatePause(); //Se llama desde el animator

    private void UpdateQuitText()
    {
        switch (GameManager.GetSceneName())
        {
            case "Lobby":
                quitText.text = "Back to intro";
                break;
            case "TowerLobby":
                quitText.text = "Back to the pizzeria";
                break;
            default:
                quitText.text = "Back to the tower";
                break;
        }
    }

    private void OnEnable() => UpdateQuitText();
}