using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreeLookSettings", menuName = "Liam/Player/FreeLookSettings")]
public class FreeLookInputSettings : ScriptableObject
{
    [System.Serializable]
    public class FreeLookValues
    {
        [System.Serializable]
        public class Axis
        {
            [field: SerializeField] public float Speed { get; private set; }
            [field: SerializeField] public float AccelTime { get; private set; }
            [field: SerializeField] public float DecelTime { get; private set; }
            [field: SerializeField] public Cinemachine.AxisState.SpeedMode SpeedMode { get; private set; }
        }

        [field: SerializeField] public Axis YAxis { get; private set; }
        [field: SerializeField] public Axis XAxis { get; private set; }

    }

    [field: SerializeField] public FreeLookValues FreeLookKeyboardValues { get; private set; }
    [field: SerializeField] public FreeLookValues FreeLookGamepadValues { get; private set; }
}
