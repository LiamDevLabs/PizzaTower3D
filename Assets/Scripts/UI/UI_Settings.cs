using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UI_Settings : MonoBehaviour
{
    #region General

    PeppinoController peppino;
    LookAtBoss lookAtBoss;
    LookInputManager lookInputManager;
    UI_HudReferences hud;

    [SerializeField] float[] defaultValues;
    bool isKeyboard = true;

    [Header("Controls")]
    [SerializeField] private RebindSaveLoad[] rebindSaveLoad;
    [SerializeField] private GameObject controlsLobbyButton, controlsGameButton;

    private IEnumerator OnLevelWasLoaded()
    {
        yield return null;
        Intialize();
        yield return null;
        LoadSettings();       
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    private void Intialize()
    {
        peppino = FindObjectOfType<PeppinoController>(true);
        if (peppino != null)
        {
            lookInputManager = peppino.GetComponentInChildren<LookInputManager>();
            isKeyboard = lookInputManager.GetPeppino().player.Input.ControllerType == DeviceSettings.ControllerType.Keyboard;
            if (isKeyboard)
            {
                horizontalSensibilitySlider.maxValue = hKeyboardSensMax;
                horizontalSensibilitySlider.minValue = hKeyboardSensMin;
                verticalSensibilitySlider.maxValue = vKeyboardSensMax;
                verticalSensibilitySlider.minValue = vKeyboardSensMin;
            }
            else
            {
                horizontalSensibilitySlider.maxValue = hGamepadSensMax;
                horizontalSensibilitySlider.minValue = hGamepadSensMin;
                verticalSensibilitySlider.maxValue = vGamepadSensMax;
                verticalSensibilitySlider.minValue = vGamepadSensMin;
            }

            lookAtBoss = peppino.GetComponent<LookAtBoss>();
            hud = peppino.player.HUD.GetComponent<UI_HudReferences>();
        }

        controlsGameButton.SetActive(SceneManager.GetActiveScene().name != "Lobby");
        controlsLobbyButton.SetActive(SceneManager.GetActiveScene().name == "Lobby");
        foreach (RebindSaveLoad rebindSL in rebindSaveLoad) rebindSL.Load();
    }
    public void LoadSettings()
    {
        LoadAudio();
        LoadFov();
        LoadCamPos();
        LoadCamRot();
        LoadSensibility();
        LoadInvertYAxis();
        LoadTargetBoss();
        LoadLateralMovement();
        LoadHud();
    }
    private void LoadAudio()
    {
        audioGeneralSlider.value = PlayerPrefs.GetFloat("Settings_audioGeneral", defaultValues[0]);
        audioEffectsSlider.value = PlayerPrefs.GetFloat("Settings_audioEffects", defaultValues[1]);
        audioMusicSlider.value = PlayerPrefs.GetFloat("Settings_audioMusic", defaultValues[2]);
        OnChangeAudioGeneral();
        OnChangeAudioEffects();
        OnChangeAudioMusic();
    }
    private void LoadFov()
    {
        tpFovSlider.value = PlayerPrefs.GetFloat("Settings_tpFov", defaultValues[3]);
        fpFovSlider.value = PlayerPrefs.GetFloat("Settings_fpFov", defaultValues[4]);
        OnChangeFov();
    }
    private void LoadCamPos()
    {
        topHeighCamSlider.value = PlayerPrefs.GetFloat("Settings_topHeightCam", defaultValues[5]);
        topRadiusCamSlider.value = PlayerPrefs.GetFloat("Settings_topRadiusCam", defaultValues[6]);
        midHeighCamSlider.value = PlayerPrefs.GetFloat("Settings_midHeightCam", defaultValues[7]);
        midRadiusCamSlider.value = PlayerPrefs.GetFloat("Settings_midRadiusCam", defaultValues[8]);
        botHeighCamSlider.value = PlayerPrefs.GetFloat("Settings_botHeighCam", defaultValues[9]);
        botRadiusCamSlider.value = PlayerPrefs.GetFloat("Settings_botRadiusCam", defaultValues[10]);
        OnChangeCamPosition();
    }
    private void LoadCamRot()
    {
        topAimCamSlider.value = PlayerPrefs.GetFloat("Settings_topAimCam", defaultValues[11]);
        midAimCamSlider.value = PlayerPrefs.GetFloat("Settings_midAimCam", defaultValues[12]);
        botAimCamSlider.value = PlayerPrefs.GetFloat("Settings_botAimCam", defaultValues[13]);
        OnChangeCamAim();
    }
    private void LoadSensibility()
    {
        if(isKeyboard)
        {
            horizontalSensibilitySlider.value = PlayerPrefs.GetFloat("Settings_keyboard_hSens", peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookKeyboardValues.XAxis.Speed);
            verticalSensibilitySlider.value = PlayerPrefs.GetFloat("Settings_keyboard_vSens", peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookKeyboardValues.YAxis.Speed);
        }
        else
        {
            horizontalSensibilitySlider.value = PlayerPrefs.GetFloat("Settings_gamepad_hSens", peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookGamepadValues.XAxis.Speed);
            verticalSensibilitySlider.value = PlayerPrefs.GetFloat("Settings_gamepad_vSens", peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookGamepadValues.YAxis.Speed);
        }
        OnChangeSensibility();
    }
    private void LoadInvertYAxis()
    {
        int value = PlayerPrefs.GetInt("Settings_InvertYAxis", 1);

        if (value == 1)
            invertYAxisToggle.isOn = true;
        else
            invertYAxisToggle.isOn = false;

        OnInvertYAxis();
    }

    private void LoadTargetBoss()
    {
        targetBossSlider.value = PlayerPrefs.GetFloat("Settings_TargetBossSpeed", 3f);
        OnChangeTargetBoss();
    }

    private void LoadLateralMovement()
    {
        lateralMovementSlider.value = PlayerPrefs.GetFloat("Settings_LateralMove", 1f);
        OnChangeLateralMove();
    }

    private void LoadHud()
    {
        int value = PlayerPrefs.GetInt("Settings_Hud", 1);

        if (value == 1)
            changeTo3DHudToggle.isOn = true;
        else
            changeTo3DHudToggle.isOn = false;

        OnChangeHUD();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Settings_audioGeneral", audioGeneralSlider.value);
        PlayerPrefs.SetFloat("Settings_audioEffects", audioEffectsSlider.value);
        PlayerPrefs.SetFloat("Settings_audioMusic", audioMusicSlider.value);
        PlayerPrefs.SetFloat("Settings_tpFov", tpFovSlider.value);
        PlayerPrefs.SetFloat("Settings_fpFov", fpFovSlider.value);
        PlayerPrefs.SetFloat("Settings_topHeightCam", topHeighCamSlider.value);
        PlayerPrefs.SetFloat("Settings_topRadiusCam", topRadiusCamSlider.value);
        PlayerPrefs.SetFloat("Settings_midHeightCam", midHeighCamSlider.value);
        PlayerPrefs.SetFloat("Settings_midRadiusCam", midRadiusCamSlider.value);
        PlayerPrefs.SetFloat("Settings_botHeighCam", botHeighCamSlider.value);
        PlayerPrefs.SetFloat("Settings_botRadiusCam", botRadiusCamSlider.value);
        PlayerPrefs.SetFloat("Settings_topAimCam", topAimCamSlider.value);
        PlayerPrefs.SetFloat("Settings_midAimCam", midAimCamSlider.value);
        PlayerPrefs.SetFloat("Settings_botAimCam", botAimCamSlider.value);
        int valueYAxis = invertYAxisToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Settings_InvertYAxis", valueYAxis);
        PlayerPrefs.SetFloat("Settings_TargetBossSpeed", targetBossSlider.value);
        PlayerPrefs.SetFloat("Settings_LateralMove", lateralMovementSlider.value);
        int valueHud = changeTo3DHudToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Settings_Hud", valueHud);

        if (isKeyboard)
        {
            PlayerPrefs.SetFloat("Settings_keyboard_hSens", horizontalSensibilitySlider.value);
            PlayerPrefs.SetFloat("Settings_keyboard_vSens", verticalSensibilitySlider.value);
        }
        else
        {
            PlayerPrefs.SetFloat("Settings_gamepad_hSens", horizontalSensibilitySlider.value);
            PlayerPrefs.SetFloat("Settings_gamepad_vSens", verticalSensibilitySlider.value);
        }

    }

    public void ResetSettings(int type) => LoadDefaultValues(type);

    #endregion

    #region Audio

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider audioGeneralSlider;
    [SerializeField] Slider audioEffectsSlider;
    [SerializeField] Slider audioMusicSlider;

    public void OnChangeAudioGeneral() => AudioChange("MainVolume", audioGeneralSlider);
    public void OnChangeAudioMusic() => AudioChange("MusicVolume", audioMusicSlider);
    public void OnChangeAudioEffects() => AudioChange("EffectsVolume", audioEffectsSlider);
    private void AudioChange(string type, Slider slider)
    {
        float value = slider.value;
        if (slider.value <= slider.minValue)
            value = -200;

        switch (type)
        {
            case "MainVolume":
                audioMixer.SetFloat("MainVolume", value);
                break;
            case "MusicVolume":
                audioMixer.SetFloat("MusicVolume", value);
                break;
            case "EffectsVolume":
                audioMixer.SetFloat("EffectsVolume", value);
                break;
        }
    }

    #endregion

    #region Gameplay

    [Header("Gameplay")]
    [SerializeField] Slider tpFovSlider;
    [SerializeField] Slider fpFovSlider;
    public void OnChangeFov() => lookInputManager?.ChangeFov(tpFovSlider.value, fpFovSlider.value);

    [SerializeField] Slider topHeighCamSlider;
    [SerializeField] Slider topRadiusCamSlider;
    [SerializeField] Slider midHeighCamSlider;
    [SerializeField] Slider midRadiusCamSlider;
    [SerializeField] Slider botHeighCamSlider;
    [SerializeField] Slider botRadiusCamSlider;
    public void OnChangeCamPosition() => lookInputManager?.ChangeCamPosition(
        new Vector2(topHeighCamSlider.value, topRadiusCamSlider.value),
        new Vector2(midHeighCamSlider.value, midRadiusCamSlider.value),
        new Vector2(botHeighCamSlider.value, botRadiusCamSlider.value));

    [SerializeField] Slider topAimCamSlider;
    [SerializeField] Slider midAimCamSlider;
    [SerializeField] Slider botAimCamSlider;
    public void OnChangeCamAim() => lookInputManager?.ChangeCamAim(topAimCamSlider.value, midAimCamSlider.value, botAimCamSlider.value);

    [SerializeField] Slider horizontalSensibilitySlider;
    [SerializeField] Slider verticalSensibilitySlider;
    [SerializeField] float hKeyboardSensMax, hKeyboardSensMin;
    [SerializeField] float vKeyboardSensMax, vKeyboardSensMin;
    [SerializeField] float hGamepadSensMax, hGamepadSensMin;
    [SerializeField] float vGamepadSensMax, vGamepadSensMin;
    public void OnChangeSensibility() => lookInputManager?.ChangeSensibility(horizontalSensibilitySlider.value, verticalSensibilitySlider.value);

    [SerializeField] Toggle invertYAxisToggle;
    public void OnInvertYAxis() => lookInputManager?.InvertYAxis(invertYAxisToggle.isOn);

    [SerializeField] Slider targetBossSlider;
    public void OnChangeTargetBoss() => lookAtBoss.speed = targetBossSlider.value;

    [SerializeField] Slider lateralMovementSlider;
    public void OnChangeLateralMove() => peppino.lateralMoveSensibility = lateralMovementSlider.value;

    #endregion

    #region Misc


    [SerializeField] Toggle changeTo3DHudToggle;
    public void OnChangeHUD()
    {
        int value = changeTo3DHudToggle.isOn ? 1 : 0;
        hud.ChangeHud(value);
    }


    public void RemoveData()
    {
        PlayerPrefs.DeleteAll();
        LoadDefaultValues();
    }

    private void LoadDefaultValues(int type=-1)
    {
        //AUDIO
        if(type == 0 || type == -1)
        {
            audioGeneralSlider.value = defaultValues[0];
            audioEffectsSlider.value = defaultValues[1];
            audioMusicSlider.value = defaultValues[2];
            OnChangeAudioGeneral();
            OnChangeAudioEffects();
            OnChangeAudioMusic();
        }
        //FOV
        if (type == 1 || type == -1)
        {
            tpFovSlider.value = defaultValues[3];
            fpFovSlider.value = defaultValues[4];
            OnChangeFov();
        }
        //CAM POS
        if (type == 2 || type == -1)
        {
            topHeighCamSlider.value = defaultValues[5];
            topRadiusCamSlider.value = defaultValues[6];
            midHeighCamSlider.value = defaultValues[7];
            midRadiusCamSlider.value = defaultValues[8];
            botHeighCamSlider.value = defaultValues[9];
            botRadiusCamSlider.value = defaultValues[10];
            OnChangeCamPosition();
        }
        //CAM ROT
        if (type == 3 || type == -1)
        {
            topAimCamSlider.value = defaultValues[11];
            midAimCamSlider.value = defaultValues[12];
            botAimCamSlider.value = defaultValues[13];
            OnChangeCamAim();
        }
        //Sensibility
        if (type == 4 || type == -1)
        {
            if (isKeyboard)
            {
                horizontalSensibilitySlider.value = peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookKeyboardValues.XAxis.Speed;
                verticalSensibilitySlider.value = peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookKeyboardValues.YAxis.Speed;
            }
            else
            {
                horizontalSensibilitySlider.value = peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookGamepadValues.XAxis.Speed;
                verticalSensibilitySlider.value = peppino.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookGamepadValues.YAxis.Speed;
            }
            OnChangeSensibility();
        }
        //Invert YAxis
        if (type == 5 || type == -1)
        {
            invertYAxisToggle.isOn = true;
            OnInvertYAxis();
        }

        //Lateral Movement
        if(type == 6 || type == -1)
        {
            targetBossSlider.value = 3f;
            OnChangeTargetBoss();
        }

        //Target Boss
        if (type == 7 || type == -1)
        {

            lateralMovementSlider.value = 1f;
            OnChangeLateralMove();
        }

        //ChangeHud
        if(type == 8 || type == -1)
        {
            changeTo3DHudToggle.isOn = true;
            OnChangeHUD();
        }
    }

    #endregion
}
