using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaFace : MonoBehaviour
{
    PeppinoController player;
    Rigidbody rb;
    FollowTarget followTarget;
    LookAtTarget lookTarget;

    [SerializeField] private Animator animator;
    [SerializeField] private Collider killerTrigger;
    [SerializeField] private float delayToChaseOnTeleport;
    [SerializeField] private float increaseSpeed;
    [SerializeField] private float maxSpeed;

    Coroutine teleportPizzaFace;

    float originalSpeed;
    bool chasing;

    private void Awake()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        followTarget = GetComponent<FollowTarget>();
        lookTarget = GetComponent<LookAtTarget>();
        originalSpeed = followTarget.speed;
        yield return null;
        player = FindObjectOfType<PeppinoController>();
        Teleporter.OnAnyTeleport += StartTeleportPizzaFace;
        Teleporter.OnAnyTrigger += OnTriggerTeleport;
    }

    private void OnLevelWasLoaded()
    {
        Teleporter.OnAnyTeleport -= StartTeleportPizzaFace;
        Teleporter.OnAnyTrigger -= OnTriggerTeleport;
        Destroy(gameObject);
    }

    private void Update()
    {
        if (increaseSpeed != 0 && followTarget.speed <= maxSpeed && chasing)
            followTarget.speed += Time.deltaTime * increaseSpeed;  
    }

    private void OnEnable()
    {
        StartTeleportPizzaFace();
    }

    void StartTeleportPizzaFace()
    {
        if (teleportPizzaFace != null) StopCoroutine(teleportPizzaFace);
        teleportPizzaFace = StartCoroutine("TeleportPizzaFace");
    }


    IEnumerator TeleportPizzaFace()
    {
        yield return null;
        yield return null;
        killerTrigger.enabled = false;
        rb.velocity = Vector3.zero;
        followTarget.enabled = false;
        followTarget.speed = originalSpeed;
        chasing = false;
        animator.SetBool("Waiting", true);
        yield return null;
        yield return null;
        transform.position = player.Rigidbody.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        lookTarget.enabled = false;
        transform.forward = Vector3.up;
        yield return new WaitUntil(() => !player.Rigidbody.isKinematic);
        yield return new WaitForSeconds(delayToChaseOnTeleport);
        animator.SetBool("Waiting", false);
        followTarget.enabled = true;
        killerTrigger.enabled = true;
        lookTarget.enabled = true;
        chasing = true;
    }

    void OnTriggerTeleport()
    {
        killerTrigger.enabled = false;
        followTarget.enabled = false;
    }

}
