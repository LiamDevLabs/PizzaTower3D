using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class LookInputManager : MonoBehaviour
{
    [SerializeField] private PeppinoController playerController;
    FreeLookInputSettings.FreeLookValues values;

    [Header("Third Person")]
    [SerializeField] private CinemachineFreeLook cinemachineThirdPerson;
    [SerializeField] private float thirdPersonFov;

    [Header("First Person")]
    [SerializeField] private CinemachineVirtualCamera cinemachineFirstPerson;
    [SerializeField] private float firstPersonFov;
    [SerializeField] private Renderer[] firstPersonDisables;
    [SerializeField] private Renderer mach2effect;
    CinemachinePOV pov;

    bool thirdPerson = true;

    IEnumerator Start()
    {
        yield return null;
        Intialize();
    }

    private void OnEnable()
    {
        Intialize();
    }

    void Intialize()
    {
        thirdPerson = true;
        if (playerController.player.Input.ControllerType == DeviceSettings.ControllerType.Keyboard)
            values = playerController.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookKeyboardValues;
        else
            values = playerController.player.Input.GeneralInputSettings.FreeLookInputSettings.FreeLookGamepadValues;

        pov = cinemachineFirstPerson.GetCinemachineComponent<CinemachinePOV>();

        LookForwardInThirdPerson();

        SetValues();
    }

    void SetValues()
    {
        //ThirdPerson
        //Y axis
        cinemachineThirdPerson.m_YAxis.m_MaxSpeed = values.YAxis.Speed;
        cinemachineThirdPerson.m_YAxis.m_AccelTime = values.YAxis.AccelTime;
        cinemachineThirdPerson.m_YAxis.m_DecelTime = values.YAxis.DecelTime;
        cinemachineThirdPerson.m_YAxis.m_SpeedMode = values.YAxis.SpeedMode;
        //X axis
        cinemachineThirdPerson.m_XAxis.m_MaxSpeed = values.XAxis.Speed;
        cinemachineThirdPerson.m_XAxis.m_AccelTime = values.XAxis.AccelTime;
        cinemachineThirdPerson.m_XAxis.m_DecelTime = values.XAxis.DecelTime;
        cinemachineThirdPerson.m_XAxis.m_SpeedMode = values.XAxis.SpeedMode;

        //FirstPerson
        //Y axis
        pov.m_VerticalAxis.m_MaxSpeed = values.XAxis.Speed;
        pov.m_VerticalAxis.m_AccelTime = values.YAxis.AccelTime;
        pov.m_VerticalAxis.m_DecelTime = values.YAxis.DecelTime;
        pov.m_VerticalAxis.m_SpeedMode = values.YAxis.SpeedMode;
        //X axis
        pov.m_HorizontalAxis.m_MaxSpeed = values.XAxis.Speed;
        pov.m_HorizontalAxis.m_AccelTime = values.XAxis.AccelTime;
        pov.m_HorizontalAxis.m_DecelTime = values.XAxis.DecelTime;
        pov.m_HorizontalAxis.m_SpeedMode = values.XAxis.SpeedMode;
    }

    void AlternateCamView(bool thirdPerson)
    {
        if (thirdPerson)
        {
            cinemachineFirstPerson.Priority = -10;
            cinemachineThirdPerson.Priority = 10;
            playerController.player.Cam.fieldOfView = thirdPersonFov;
            foreach (Renderer renderer in firstPersonDisables) renderer.gameObject.layer = LayerMask.NameToLayer("Player");
            mach2effect.gameObject.layer = LayerMask.NameToLayer("MachEffect2_Visible");
        }
        else
        {
            cinemachineThirdPerson.Priority = -10;
            cinemachineFirstPerson.Priority = 10;
            playerController.player.Cam.fieldOfView = firstPersonFov;
            LookForwardInFirstPerson();
            foreach (Renderer renderer in firstPersonDisables) renderer.gameObject.layer = LayerMask.NameToLayer("PlayerPOV");
            mach2effect.gameObject.layer = LayerMask.NameToLayer("MachEffect2_Invisible");
        }
    }


    float delay = 0.1f;
    float lastPress;
    private void Update()
    {
        if(Time.realtimeSinceStartup > 0.1f)
        if (playerController.player.Input.AlternateViewButtonInput.down && Time.time > lastPress+ delay)
        {
            thirdPerson = !thirdPerson;
            AlternateCamView(thirdPerson);
            lastPress = Time.time;
        }
    }

    /*----------------------------------------------------------//
    /*----------------------------------------------------------//
    //------------------------Public Set------------------------*/

    public void ChangeFov(float tpFov, float fpFov)
    {
        thirdPersonFov = tpFov;
        firstPersonFov = fpFov;
        if (thirdPerson)
            playerController.player.Cam.fieldOfView = thirdPersonFov;
        else
            playerController.player.Cam.fieldOfView = firstPersonFov;
    }

    public void ChangeCamPosition(Vector2 top, Vector2 mid, Vector2 bot)
    {
        cinemachineThirdPerson.m_Orbits[0].m_Height = top[0];
        cinemachineThirdPerson.m_Orbits[0].m_Radius = top[1];

        cinemachineThirdPerson.m_Orbits[1].m_Height = mid[0];
        cinemachineThirdPerson.m_Orbits[1].m_Radius = mid[1];

        cinemachineThirdPerson.m_Orbits[2].m_Height = bot[0];
        cinemachineThirdPerson.m_Orbits[2].m_Radius = bot[1];
    }

    public void ChangeCamAim(float yTargetTop, float yTargetMid, float yTargetBot)
    {
        cinemachineThirdPerson.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = yTargetTop;
        cinemachineThirdPerson.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = yTargetMid;
        cinemachineThirdPerson.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = yTargetBot;
    }

    public void LookForwardInFirstPerson()
    {
        pov.m_HorizontalAxis.Value = playerController.Rigidbody.transform.localEulerAngles.y;
        pov.m_VerticalAxis.Value = 0;
    }

    public void LookForwardInThirdPerson()
    {
        float angle = Vector3.SignedAngle(playerController.player.Cam.transform.forward, playerController.Rigidbody.transform.forward, playerController.Rigidbody.transform.up);
        cinemachineThirdPerson.m_XAxis.Value += angle;
        cinemachineThirdPerson.m_YAxis.Value = 0.5f;
    }

    public void ChangeSensibility(float hSens, float vSens, bool povSens=false)
    {
        //ThirdPerson
        if (!povSens)
        {
            //Y axis
            cinemachineThirdPerson.m_YAxis.m_MaxSpeed = vSens;
            //X axis
            cinemachineThirdPerson.m_XAxis.m_MaxSpeed = hSens;
        }
        //FirstPerson
        else
        {
            //Y axis
            pov.m_VerticalAxis.m_MaxSpeed = vSens;
            //X axis
            pov.m_HorizontalAxis.m_MaxSpeed = hSens;
        }
    }

    public void InvertYAxis(bool isOn)
    {
        cinemachineThirdPerson.m_YAxis.m_InvertInput = isOn;
    }

    /*----------------------------------------------------------//
    /*----------------------------------------------------------//
    //------------------------Public Get------------------------*/

    public PeppinoController GetPeppino() => playerController;
    public float GetFov(bool firstPerson)
    {
        if (firstPerson)
            return firstPersonFov;
        else
            return thirdPersonFov;
    }

    public CinemachineFreeLook.Orbit GetOrbit(int n) => cinemachineThirdPerson.m_Orbits[n];

    public float GetCamAimValue(int n) => cinemachineThirdPerson.GetRig(n).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y;

    public float GetSensibility(bool h)
    {
        if (h)
            return values.XAxis.Speed;
        else
            return values.YAxis.Speed;
    }
}
