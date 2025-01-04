using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    private void Start()
    {
        Load();
    }

    public void OnEnable()
    {
        Load();
    }

    public void OnDisable()
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Load()
    {
        var rebinds = PlayerPrefs.GetString("bindings");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void Save()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("bindings", rebinds);
    }
}
