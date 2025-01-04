using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Base Enemy")]
    [Header("Player in Range")]
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float radiusToSeePlayer;
    [SerializeField] protected Vector3 radiusPosition;
    protected PeppinoController player;
    protected bool isPlayerOnRange;

    // Update is called once per frame
    virtual protected void Update()
    {
        DetectPlayer();
        States();
    }

    virtual protected void DetectPlayer()
    {
        isPlayerOnRange = Physics.CheckSphere(transform.TransformPoint(radiusPosition), radiusToSeePlayer, whatIsPlayer);

        if (isPlayerOnRange)
        {
            if (player == null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.TransformPoint(radiusPosition), radiusToSeePlayer, whatIsPlayer);
                foreach (Collider col in colliders)
                    if (col.TryGetComponent(out PeppinoHitbox playerHitbox))
                    {
                        player = playerHitbox.Peppino;
                        break;
                    }
            }
        }
        else
        {
            player = null;
        }
    }

    abstract protected void States();

    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.TransformPoint(radiusPosition), radiusToSeePlayer);
    }
}