using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinigameTutorial : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform footer;
    [SerializeField] private PlayerMenuController_TutorialReady playerReadyControllerPrefab;

    private void OnEnable() 
    { 
        if(footer.childCount > 0) 
            foreach(Transform child in footer) 
                Destroy(child.gameObject);

        if (content.childCount > 0)
            foreach (Transform child in content)
                Destroy(child.gameObject);

        foreach (Player p in PlayerManager.players)
        {
            PlayerMenuController_TutorialReady playerController = Instantiate(playerReadyControllerPrefab, footer);
            playerController.player = p;
            p.PlayerController = playerController;
        }

        Instantiate(GameManager.SelectedLevel.Tutorial, content);
    }
}
