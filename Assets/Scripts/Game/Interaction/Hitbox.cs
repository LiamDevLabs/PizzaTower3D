using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private GameObject destroyedEffect;
    [SerializeField] private bool isMetal = false;
    [SerializeField] private bool isEnemy = false;
    [SerializeField] private bool isPillarJohn = false;

    public delegate void KillAction();
    public event KillAction OnKill;

    [SerializeField] private UnityEvent OnKillUnityEvent;

    private void Awake()
    {
        if (objectToDestroy == null) objectToDestroy = gameObject;
        if (destroyedEffect) destroyedEffect.SetActive(false);

        gameObject.tag = "Hitbox";
        gameObject.layer = LayerMask.NameToLayer("Hitbox");

    }

    public bool Kill(PlayerCombo playerCombo, bool destroyMetal = false, bool destroyEnemy = true, Rigidbody playerRB = null)
    {
        if (Kill(destroyMetal, destroyEnemy))
        {
            if(playerCombo != null)
            {
                if(isEnemy) playerCombo.AddCombo();
                if (playerRB && destroyedEffect.TryGetComponent(out DestroyedEffect destroyedEffectScript)) destroyedEffectScript.SetPlayer(playerRB);
            }
            return true;
        }
        else
            return false;
    }

    public bool Kill(bool destroyMetal = false, bool destroyEnemy = true)
    {
        if (isMetal && !destroyMetal)
            return false;

        if (isEnemy && !isPillarJohn && !destroyEnemy)
            return false;

        if (destroyedEffect)
        {
            destroyedEffect.SetActive(true);
            destroyedEffect.transform.parent = null;
        }

        OnKill?.Invoke();
        OnKillUnityEvent.Invoke();

        Destroy(objectToDestroy.gameObject);
        return true;
    }
}
