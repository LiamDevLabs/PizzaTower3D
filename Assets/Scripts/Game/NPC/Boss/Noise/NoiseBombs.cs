using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBombs : MonoBehaviour
{
    [field: Header("References")]
    [SerializeField] private AnimatorNamedParameters animatorNamedParameters;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    private PeppinoController player;

    [Header("General")]
    [SerializeField] private float jumpImpulse;
    [SerializeField] private float toPlayerImpulse;
    [SerializeField] private float explosionTime;
    bool exploded = false;

    [Header("General Collision Check")]
    [SerializeField] private float collisionCheckRadius;
    [SerializeField] private Vector3 collisionCheckPosition;
    [SerializeField] private LayerMask collisionCheckLayerMask;
    [SerializeField] private LayerMask playerCheckLayerMask;
    [SerializeField] private bool collisionCheckGizmos;
    private bool isColliding, isPlayerColliding;

    //----------
    float time = 0;

    private void Awake()
    {
        time = 0;
        exploded = false;
        explosion.SetActive(false);
    }

    private IEnumerator Start()
    {
        yield return null;
        player = FindObjectOfType<PeppinoController>();
    }

    // Update is called once per frame
    void Update()
    {
        Checkers();
        ExplosionTimer();
    }

    private void FixedUpdate()
    {
        Jump();
    }

    void Checkers()
    {
        isColliding = Physics.CheckSphere(rb.transform.TransformPoint(collisionCheckPosition), collisionCheckRadius, collisionCheckLayerMask);
        isPlayerColliding = isColliding && Physics.CheckSphere(rb.transform.TransformPoint(collisionCheckPosition), collisionCheckRadius, playerCheckLayerMask);
    }

    void Jump()
    {
        if (isColliding && player)
        {
            //Jump Force
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            //Player Horizontal Direction
            Vector3 hDirection = (player.Rigidbody.position - rb.position).normalized * toPlayerImpulse;
            hDirection.y = rb.velocity.y;
            //Horizontal Force
            rb.velocity = hDirection;
            //Animation
            //animatorNamedParameters.SetString("Jump");
        }
        else
        {
            //animatorNamedParameters.SetString("Normal");
        }
    }

    void ExplosionTimer()
    {
        time += Time.deltaTime;
        if ((time > explosionTime || isPlayerColliding) && !exploded)
            Explode();
    }

    void Explode()
    {
        exploded = true;
        explosion.transform.parent = null;
        explosion.SetActive(true);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (collisionCheckGizmos)
        {
            Gizmos.color = isColliding ? Color.cyan : Color.yellow;
            Gizmos.DrawWireSphere(rb.transform.TransformPoint(collisionCheckPosition), collisionCheckRadius);
        }
    }
}
