using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarList", menuName = "Liam/Player/AvatarList")]
public class AvatarSettings : ScriptableObject
{
    public enum ControllerType
    {
        CharacterController
    }

    [System.Serializable]
    public class Avatar
    {
        [field: SerializeField] public string Name {get; private set;}
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GameObject LobbyAvatar { get; private set; }

        [SerializeField] private PeppinoController characterPrefab;


        public PlayerBaseController GetAvatar(ControllerType controllerType)
        {
            switch (controllerType)
            {
                case ControllerType.CharacterController:
                    return characterPrefab;
                default: 
                    return null;
            }
        }
    }

    [field:SerializeField] public List<Avatar> AvatarList {get; private set;}
}
