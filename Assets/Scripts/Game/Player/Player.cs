using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
public class Player : MonoBehaviour
{
    [field:SerializeField] public Camera Cam { get; private set; }
    [field: SerializeField] public Canvas HUD { get; private set; }

    public int PlayerNumber { get; private set; } = 0;
    private static int totalPlayers = 0;


    public AvatarSettings.Avatar choosenAvatar;
    public PlayerInputs Input { get; private set; }
    public PlayerBaseController PlayerController { get; set; }
    [field: SerializeField] public PlayerScore Score { get; private set; }
    [field: SerializeField] public PlayerCombo Combo { get; private set; }

    private void Awake()
    {
        Input = GetComponent<PlayerInputs>();   
        PlayerNumber = totalPlayers;
        totalPlayers++;
        choosenAvatar = FindObjectOfType<PlayerManager>().AvatarSettings.AvatarList[0];
        SetChooseAvatarController();
        CamBackup();
        DontDestroyOnLoad(gameObject);
    }

    private void SetChooseAvatarController()
    {
        GameObject ChooseAvatarControllerGameObject = new GameObject();
        ChooseAvatarControllerGameObject.name = "Choose Avatar Controller";
        PlayerMenuController_ChooseAvatar playerChooseAvatarController = ChooseAvatarControllerGameObject.AddComponent<PlayerMenuController_ChooseAvatar>();
        PlayerController = playerChooseAvatarController;
        PlayerController.player = this;
    }

    private void Start()
    {
        if (PlayerController)
            PlayerController.PlayerStart();
    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        RecoverCam();

        yield return new WaitForEndOfFrame();

        if (PlayerController)
            PlayerController.PlayerStart();

        HudOnLoadScene();
    }

    private void Update()
    {
        if (PlayerController)
            PlayerController.PlayerUpdate();
    }

    private void FixedUpdate()
    {
        if (PlayerController)
            PlayerController.PlayerFixedUpdate();
    }

    private void LateUpdate()
    {
        if (PlayerController)
            PlayerController.PlayerLateUpdate();
    }

    private void HudOnLoadScene()
    {
        PlayerEnabler playerEnabler = FindObjectOfType<PlayerEnabler>();
        if(playerEnabler != null) HUD.enabled = playerEnabler.startHudEnabled;
    }


    #region When Load Scene...
    Camera camAuxOnLoadScene;
    private void CamBackup()
    {
        camAuxOnLoadScene = new GameObject("CamBackup").AddComponent<Camera>();
        camAuxOnLoadScene.CopyFrom(Cam);
        camAuxOnLoadScene.gameObject.SetActive(false);
        camAuxOnLoadScene.transform.parent = transform;
    }
    private void RecoverCam()
    {
        if(Cam == null)
        {
            Cam = new GameObject(gameObject.name + " Cam").AddComponent<Camera>();
            Cam.transform.parent = transform;
            Cam.transform.localPosition = Vector3.zero;
            Cam.transform.localEulerAngles = Vector3.zero;
            Cam.CopyFrom(camAuxOnLoadScene);
        }
    }
    #endregion

    #region When use ExternalCam...
    Camera camAuxOnExternalCam;
    private float previousLateralMove;
    public void SetExternalCam(Camera newCam, bool drift = true)
    {
        camAuxOnExternalCam = Cam;
        Cam.enabled = false;
        Cam = newCam;
        if (PlayerController.TryGetComponent(out PeppinoController peppino)) peppino.drift = drift;
        previousLateralMove = peppino.lateralMoveSensibility;
        peppino.lateralMoveSensibility = 1;
    }
    public void RemoveExternalCam()
    {
        if(camAuxOnExternalCam)
        Cam = camAuxOnExternalCam;
        Cam.enabled = true;
        if (PlayerController.TryGetComponent(out PeppinoController peppino)) peppino.drift = true;
        peppino.lateralMoveSensibility = previousLateralMove;
    }
    #endregion
}
