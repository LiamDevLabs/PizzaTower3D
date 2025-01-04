using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class UI_PlayerJoined : MonoBehaviour
{
    public Player Player {get; private set;}
    [SerializeField] private TextMeshProUGUI playerNumberText;
    [SerializeField] private Image choosenAvatar;
    [SerializeField] private Image readyCheckMark;
    [SerializeField] private GameObject noReadySign;

    GameObject avatarModel;
    RandomDanceAnimation avatarDance;
    Vector3 positionPlayers3D;
    Vector3 anglePlayers3D;
    float spaceAvatarPosition;

    private void Awake()
    {
        readyCheckMark.enabled = false;
    }

    public void SetPlayer(Player player, Vector3 positionPlayers3D, Vector3 anglePlayers3D, float avatarPosition)
    {
        Player = player;
        this.spaceAvatarPosition = avatarPosition;
        this.positionPlayers3D = positionPlayers3D;
        this.anglePlayers3D = anglePlayers3D;
        playerNumberText.text = "" + player.PlayerNumber;
        ChangeAvatarImage(player.choosenAvatar.Icon);

        if(player.PlayerController.TryGetComponent(out PlayerMenuController_ChooseAvatar chooseAvatarController))
        {
            chooseAvatarController.playerJoinedUI = this;
        }
    }

    public void ChangeAvatarImage(Sprite icon)
    {
        choosenAvatar.sprite = icon;

        //Model
        if(avatarModel != null) Destroy(avatarModel);
        avatarModel = Instantiate(Player.choosenAvatar.LobbyAvatar, new Vector3(spaceAvatarPosition + positionPlayers3D.x, positionPlayers3D.y, positionPlayers3D.z), Quaternion.Euler(anglePlayers3D));

        avatarModel.TryGetComponent(out RandomDanceAnimation avatarDance);
        if(avatarDance) avatarDance.Dance(readyCheckMark.enabled);
    }

    public void ReadyCheckMark(bool ready)
    {
        readyCheckMark.enabled = ready;
        noReadySign.SetActive(!ready);
        if (avatarDance) avatarDance.Dance(ready);
    }

    private void OnDestroy()
    {
        if (avatarModel != null) Destroy(avatarModel);
    }
}
