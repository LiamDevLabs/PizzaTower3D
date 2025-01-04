using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeppinoHitbox : MonoBehaviour
{
    [field: SerializeField] public PeppinoController Peppino { get; private set; }
    [field: SerializeField] public float DamagedForceUp { get; private set; }
    [field: SerializeField] public float DamagedForceBackward { get; private set; }
    public bool Inmortality { get; private set; } = false;

    [SerializeField] private float delayToHitAgain;


    public void Hit(Vector3 hitOrigin, float substractComboTime, bool kill = false)
    {
        Peppino.player.Combo.RemoveTime(substractComboTime);
        Hit(hitOrigin, kill);
        Inmortal();
    }

    public void Hit(Vector3 hitOrigin, bool kill = false)
    {
        if (kill) { GameManager.Loose(); return; }
        if (Peppino.Damaged || Inmortality) return;
        Peppino.Health.Damage();
        Peppino.StateManager.SwitchState(Peppino.StateManager.States.DamagedState);
        Peppino.Rigidbody.velocity = Vector3.zero;
        hitOrigin.y = Peppino.Rigidbody.transform.position.y;
        Peppino.Rigidbody.AddForce(DamagedForceUp * Peppino.Rigidbody.transform.up, ForceMode.Impulse);
        Peppino.Rigidbody.AddForce(DamagedForceBackward * (Peppino.Rigidbody.transform.position - hitOrigin).normalized, ForceMode.Impulse);
    }

    public void Inmortal()
    {
        if (!Inmortality)
        {
            Inmortality = true;
            Invoke("Mortal", delayToHitAgain);
        }
    }

    private void Mortal() => Inmortality = false;
}
