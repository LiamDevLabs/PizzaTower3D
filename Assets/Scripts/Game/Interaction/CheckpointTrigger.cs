using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    //Respawnpoint
    static public Transform CurrentRespawnpoint { get; private set; }
    [SerializeField] private Transform checkpointRespawnpoint;

    //Checkpoint order
    [SerializeField] [Tooltip("Can't be a Negative value")] private int order;
    private static int lastCheckpoint = -1;
    private bool checkpointChecked = false;

    //Event
    public delegate void CheckpointAction();
    static public event CheckpointAction OnCheckpoint;

    //Animator
    [SerializeField] Animator animator;

    private void Awake()
    {
        checkpointChecked = false;
        if (!checkpointRespawnpoint) checkpointRespawnpoint = transform;
        if (!CurrentRespawnpoint) SetFirstRespawnPoint();
    }

    void SetFirstRespawnPoint()
    {
        Transform player = null;
        try {
            player = FindObjectOfType<PeppinoController>().Rigidbody.transform; }
        catch(System.NullReferenceException e) { Debug.LogWarning("Player not found"); }

        if (player != null)
        {
            GameObject firstRespawnpoint = new GameObject("firstRespawnpoint");
            firstRespawnpoint.transform.position = player.position;
            firstRespawnpoint.transform.rotation = player.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkpointChecked && order >= lastCheckpoint && other.CompareTag("Player"))
            Checkpoint();
    }

    private void Checkpoint()
    {
        checkpointChecked = true;
        lastCheckpoint = Mathf.Abs(order);
        CurrentRespawnpoint = checkpointRespawnpoint;
        OnCheckpoint?.Invoke();
        if (animator) animator.SetBool("Checked", true);
    }
}
