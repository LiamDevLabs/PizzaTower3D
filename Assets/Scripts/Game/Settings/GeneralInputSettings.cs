using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Liam/Player/InputSettings")]
public class GeneralInputSettings : ScriptableObject
{
    [System.Serializable]
    public class GamepadSettings
    {
        [field: SerializeField] public float moveDeadZoneX { get; private set; }
        [field: SerializeField] public float moveDeadZoneY { get; private set; }

        public bool IsInMoveDeadZone(Vector3 input) => Mathf.Abs(input.x) <= moveDeadZoneX && Mathf.Abs(input.z) <= moveDeadZoneY;
        public bool IsInMoveDeadZone(Vector2 input) => Mathf.Abs(input.x) <= moveDeadZoneX && Mathf.Abs(input.y) <= moveDeadZoneY;
        public bool IsInMoveDeadZoneX(float xInput) => Mathf.Abs(xInput) <= moveDeadZoneX;
        public bool IsInMoveDeadZoneY(float yInput) => Mathf.Abs(yInput) <= moveDeadZoneY;


    }

    [System.Serializable]
    public class KeyboardSettings
    {

    }

    [field: SerializeField] public GamepadSettings GamepadConfig { get; private set; }
    [field: SerializeField] public KeyboardSettings KeyboardConfig { get; private set; }
    [field: SerializeField] public FreeLookInputSettings FreeLookInputSettings { get; private set; }



}
