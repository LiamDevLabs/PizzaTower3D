using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private PeppinoController player;
    [SerializeField] private PlayerDamageTrigger forwardDamage;

    [Header("Always Match Effects")]
    [SerializeField] private GameObject[] match0AlwaysEffects;
    [SerializeField] private GameObject[] match1AlwaysEffects, match2AlwaysEffects, match3AlwaysEffects;
    int alwaysEffectNumber = -2;

    [Header("Grounded Match Effects")]
    [SerializeField] private GameObject[] match0GroundedEffects;
    [SerializeField] private GameObject[] match1GroundedEffects, match2GroundedEffects, match3GroundedEffects;
    int groundedEffectNumber = -2;

    [Header("Heavy Body Slam")]
    [SerializeField] private GameObject bodySlamEffect;

    [Header("Taunt")]
    [SerializeField] private GameObject tauntBackground;
    bool tauntEffect = false;


    void Start()
    {
        alwaysEffectNumber = -2;
        groundedEffectNumber = -2;
        tauntEffect = false;
        tauntBackground.SetActive(false);
        UnableAllAlwaysEffects();
        UnableAllGroundedEffects();
    }

    // Update is called once per frame
    void Update()
    {
        AlwaysEffects();
        GroundedEffects();
        TauntEffect();
        BodySlamEffect();
    }

    #region AlwaysEffects
    void AlwaysEffects() //not always xdxdxddd
    {
        if (player.CurrentMachRun != alwaysEffectNumber)
        {
            UnableAllAlwaysEffects();
            switch (player.CurrentMachRun)
            {
                case -1:
                    alwaysEffectNumber = -1;
                    break;
                case 0:
                    foreach (GameObject effect in match0AlwaysEffects) effect.SetActive(true);
                    alwaysEffectNumber = 0;
                    break;
                case 1:
                    foreach (GameObject effect in match1AlwaysEffects) effect.SetActive(true);
                    alwaysEffectNumber = 1;
                    break;
                case 2:
                    foreach (GameObject effect in match2AlwaysEffects) effect.SetActive(true);
                    alwaysEffectNumber = 2;
                    break;
                case 3:
                    foreach (GameObject effect in match3AlwaysEffects) effect.SetActive(true);
                    alwaysEffectNumber = 3;
                    break;
            }
        }

        //UnableAllEffects if value != 1
        if (alwaysEffectNumber != -1 
        && (
        //UnableAllEffects in exceptions (super jump or bodyslam)
        (player.StateManager.GetCurrentState() == "PeppinoSuperJumpPreparingState" || player.StateManager.GetCurrentState() == "PeppinoSuperJumpState" || player.StateManager.GetCurrentState() == "PeppinoBodySlamState")
        ||
        //If damage forward is not enabled, mach2 effects will not be enabled
        !(forwardDamage.gameObject.activeSelf && (forwardDamage.DestroyEnemies || forwardDamage.DestroyMetal))
        ))
        {
            UnableAllAlwaysEffects();
            alwaysEffectNumber = -1;
        }
    }

    void UnableAllAlwaysEffects()
    {
        foreach (GameObject effect in match0AlwaysEffects) effect.SetActive(false);
        foreach (GameObject effect in match1AlwaysEffects) effect.SetActive(false);
        foreach (GameObject effect in match2AlwaysEffects) effect.SetActive(false);
        foreach (GameObject effect in match3AlwaysEffects) effect.SetActive(false);
    }

    #endregion

    #region GroundedEffects

    void GroundedEffects()
    {
        if (player.CurrentMachRun != groundedEffectNumber)
        {
            UnableAllGroundedEffects();
            switch (player.CurrentMachRun)
            {
                case -1:
                    groundedEffectNumber = -1;
                    break;
                case 0:
                    foreach (GameObject effect in match1GroundedEffects) effect.SetActive(true);
                    groundedEffectNumber = 0;
                    break;
                case 1:
                    foreach (GameObject effect in match1GroundedEffects) effect.SetActive(true);
                    groundedEffectNumber = 1;
                    break;
                case 2:
                    foreach (GameObject effect in match2GroundedEffects) effect.SetActive(true);
                    groundedEffectNumber = 2;
                    break;
                case 3:
                    foreach (GameObject effect in match3GroundedEffects) effect.SetActive(true);
                    groundedEffectNumber = 3;
                    break;
            }
        }

        //UnableAllEffects in air
        if (groundedEffectNumber != -1 &&
            (!player.IsGrounded))
        {
            groundedEffectNumber = -1;
            UnableAllGroundedEffects();
        }
            
    }

    void UnableAllGroundedEffects()
    {
        foreach (GameObject effect in match0GroundedEffects) effect.SetActive(false);
        foreach (GameObject effect in match1GroundedEffects) effect.SetActive(false);
        foreach (GameObject effect in match2GroundedEffects) effect.SetActive(false);
        foreach (GameObject effect in match3GroundedEffects) effect.SetActive(false);
    }

    #endregion

    #region Taunt

    void TauntEffect()
    {
        if (player.Parry.Taunting)
        {
            if (!tauntEffect)
            {
                tauntBackground.SetActive(true);
                tauntEffect = true;
            }
        }
        else
        {
            if (tauntEffect)
            {
                tauntBackground.SetActive(false);
                tauntEffect = false;
            }
        }
    }

    #endregion

    void BodySlamEffect()
    {
        bodySlamEffect.SetActive(player.HeavyBodySlamming && !player.IsGrounded);
    }

}
