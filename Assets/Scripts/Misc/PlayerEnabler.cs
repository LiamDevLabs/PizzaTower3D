using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnabler : MonoBehaviour
{
    Player player;

    [SerializeField] public bool startHudEnabled;
    [SerializeField] public bool startPlayerInputEnabled;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.HUD.enabled = startHudEnabled;
        player.Input.PlayerInput.enabled = startPlayerInputEnabled;
    }

    public void Enable(bool enable)
    {
        player.HUD.enabled = enable;
        player.Input.PlayerInput.enabled = enable;
    }
}
