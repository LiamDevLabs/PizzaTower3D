using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalCam : MonoBehaviour
{
    private Player player;
    private PeppinoController peppinoController;
    private Camera cam;
    private bool activatedEvent = false;

    private delegate void ActivateCamAction();
    static private event ActivateCamAction OnActivate;

    [SerializeField] private bool forceActivation = false;
    [SerializeField] private bool disableDrift = false;
    [SerializeField] private bool disableHud = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        OnActivate += DisableUnusedCam;
    }

    IEnumerator Start()
    {
        yield return null;
        if (!player) player = FindObjectOfType<Player>();
        if (player && player.PlayerController.TryGetComponent(out peppinoController))
            yield return null;
        ForceActivation();
    }

    private void OnEnable()
    {
        SetCam();
    }

    private void DisableUnusedCam()
    {
        if (!activatedEvent)
        {
            gameObject.SetActive(false);
        }
        activatedEvent = false;
    }

    private void OnDisable()
    {
        if (player.Cam.gameObject == gameObject)
        {
            player.RemoveExternalCam();
            if (peppinoController)
            {
                peppinoController.MovementMode = PeppinoController.MoveForceMode.OnlyForwardFlatVelocity3;
                peppinoController.GrabToPeppinoDireccion = false;

            }
        }
    }

    private void OnLevelWasLoaded()
    {
        OnActivate -= DisableUnusedCam;
    }

    void SetCam()
    {
        if (!player || !cam) return;
        if (player.Cam) player.Cam.enabled = false;
        activatedEvent = true;
        if (peppinoController)
        {
            peppinoController.MovementMode = PeppinoController.MoveForceMode.Force;
            peppinoController.GrabToPeppinoDireccion = true;
        }
        OnActivate?.Invoke();
        player.SetExternalCam(cam, !disableDrift);
    }

    bool ForceActivation()
    {
        if (forceActivation)
        {
            SetCam();
            Hud();
        }
        gameObject.SetActive(forceActivation);
        return forceActivation;
    }

    private void Hud()
    {
        player.HUD.enabled = !disableHud;
    }
}
