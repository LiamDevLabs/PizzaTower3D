using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretPortal : Teleporter
{
    [Header("Secret Portal")]
    [SerializeField] private bool isEntrance;
    [SerializeField] private bool disableFog;
    [SerializeField] private AudioSource[] levelMusics;
    [SerializeField] private AudioSource secretMusic;

    [SerializeField] private int id;
    private bool found;

    PizzaTimeManager pizzaTime;

    private void Awake() => pizzaTime = FindObjectOfType<PizzaTimeManager>();

    protected override void Trigger(Transform player)
    {
        base.Trigger(player);

        found = true;
        secretMusic.enabled = isEntrance;
        pizzaTime.Pause(isEntrance);

        if (isEntrance)
            foreach (AudioSource levelMusic in levelMusics) levelMusic.Pause();
        else
            foreach (AudioSource levelMusic in levelMusics) levelMusic.UnPause();
    }

    protected override void AfterTeleport()
    {
        if(isEntrance)
        {
            gameObject.SetActive(false);
            if(disableFog) RenderSettings.fog = false;
        }
        else
        {
            if (disableFog) RenderSettings.fog = true;
        }
    }

    public void SaveSecret()
    {
        if(found && !isEntrance) PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Secret_" + id, 1);
    }

    static public void SaveAllFoundSecrets()
    {
        SecretPortal[] secrets = FindObjectsOfType<SecretPortal>(true);
        foreach (SecretPortal secret in secrets) secret.SaveSecret();
    }


    
}
