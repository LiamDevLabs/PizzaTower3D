using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UI_ButtonsInGame
{
    [SerializeField] private PlayerBaseController playerController;
    public Image buttonUI;
    [SerializeField] private DeviceSettings uiButtons;
    [SerializeField] private bool isBasicButton;
    

    Vector2 originalSize;
    bool firstTimePressed = false;
    bool firstTimeShowed = false;

    public void Press()
    {
        if(!firstTimePressed) originalSize = buttonUI.rectTransform.sizeDelta;
        buttonUI.sprite = uiButtons.GetControllerType(playerController.player.Input.ControllerType).GetButton(isBasicButton).pulsedButtonSprite;
        buttonUI.rectTransform.sizeDelta = originalSize * 0.75f;
        firstTimePressed = true;
        Debug.Log("press" + playerController.player.Input.ControllerType);
    }

    public void Press(bool press)
    {
        if (press)
            Press();
        else
            Release();
    }

    public void Release()
    {
        if(!firstTimePressed) originalSize = buttonUI.rectTransform.sizeDelta;
        buttonUI.sprite = uiButtons.GetControllerType(playerController.player.Input.ControllerType).GetButton(isBasicButton).buttonSprite;
        buttonUI.rectTransform.sizeDelta = originalSize;
        Debug.Log("release" + playerController.player.Input.ControllerType);

    }

    public void Show(bool show)
    {
        if (!firstTimeShowed)
        {
            buttonUI.sprite = uiButtons.GetControllerType(playerController.player.Input.ControllerType).GetButton(isBasicButton).buttonSprite;
            firstTimeShowed = true;
        }
        buttonUI.enabled = show;
    }
}
