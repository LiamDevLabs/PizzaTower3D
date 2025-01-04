using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsByInput : MonoBehaviour
{
    PlayerInputs playerInputs;
    [SerializeField] GameObject[] keyboardObjects;
    [SerializeField] GameObject[] gamepadObjects;

    [SerializeField] bool activateOnLoadScene;


    void Start()
    {
        Activate();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (activateOnLoadScene) Activate();
    }

    private void Activate()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
        foreach (GameObject keyboardGm in keyboardObjects)
            keyboardGm.SetActive(playerInputs.ControllerType == DeviceSettings.ControllerType.Keyboard);
        foreach (GameObject gamepadGm in gamepadObjects)
            gamepadGm.SetActive(playerInputs.ControllerType != DeviceSettings.ControllerType.Keyboard);
    }
}
