using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonSettings", menuName = "Liam/Player/ButtonSettings")]
public class DeviceSettings : ScriptableObject
{
    public enum ControllerType
    {
        Keyboard,
        Xbox,
        Playstation
    }

    [System.Serializable]
    public class UI_Button
    {
        public Sprite buttonSprite, pulsedButtonSprite;
    }

    [System.Serializable]
    public class UI_Buttons
    {
        public UI_Button basicButton, specialButton;

        public UI_Button GetButton(int n)
        {
            switch (n)
            {
                case 0:
                    return basicButton;
                case 1:
                    return specialButton;
                default:
                    return basicButton;
            }
        }
        public UI_Button GetButton(bool basicButton)
        {
            switch (basicButton)
            {
                case true:
                    return this.basicButton;
                case false:
                    return specialButton;
            }
        }
    }


    [SerializeField] private UI_Buttons Keyboard, Xbox, Playstation;

    public UI_Buttons GetControllerType(ControllerType type)
    {
        switch (type)
        {
            case ControllerType.Keyboard:
                return Keyboard;
            case ControllerType.Xbox:
                return Xbox;
            case ControllerType.Playstation:
                return Playstation;
            default:
                return Keyboard;
        }
    }

}
