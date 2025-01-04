using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UI_SecretLevelsButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnableButton;
    [SerializeField] private UnityEvent OnDisableButton;

    bool enable = false;

    private void Awake()
    {
        if (enable)
            OnEnableButton.Invoke();
        else
            OnDisableButton.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            enable = !enable;
            if (enable)
                OnEnableButton.Invoke();
            else
                OnDisableButton.Invoke();
        }
    }
}
