using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY FOR BOSSES
public class PlayerHealth : MonoBehaviour
{
    private UI_Health healthUI;
    [HideInInspector] public PlayerScore score;

    [field: SerializeField] public int Health { get; protected set; }
    [field: SerializeField] public int CurrentHealth { get; protected set; }
    public int Hits { get; protected set; }
    public bool activated { get; set; }


    //<--- Poner aca la interfaz

    private void Awake()
    {
        CurrentHealth = Health;
        healthUI = FindObjectOfType<UI_Health>(true);
        healthUI.SetPlayerUI(Health);
        Hits = 0;
    }

    public bool Damage(int damage=1)
    {
        if (!activated) return false;
        CurrentHealth -= damage;
        CurrentHealth = (CurrentHealth < 0) ? 0 : CurrentHealth;
        CurrentHealth = (CurrentHealth > Health) ? Health : CurrentHealth;
        if (healthUI) healthUI.UpdatePlayerHealth(CurrentHealth);
        if(damage > 0) Hits++;
        score.UpdateScore();

        if (CurrentHealth <= 0)
            GameManager.Loose();

        return true;
    }
}
