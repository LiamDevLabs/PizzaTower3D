using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Bird : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private Transform[] spots;
    [SerializeField] private float moveSpeed/*, rotateSpeed*/;
    [SerializeField] private bool flying;
    [SerializeField] private float curveHeightMultiplier;
    [SerializeField] private bool onceTime = false;
    private bool ended = false;
    private Vector3 startPosition;
    private float moveTime=0;
    private float rotateTime = 0;

    private Transform selectedSpot;


    private void Start()
    {
        RestartFly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RestartFly();
            startPosition = transform.position;
            flying= true;
            selectedSpot = spots[Random.Range(0, spots.Length)];

        }
    }

    private void RestartFly()
    {
        flying = false;
        moveTime = 0;
        rotateTime = 0;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (ended)
        {
            animator.SetBool("flying", false);
            enabled = false;
            return;
        }

        if (flying)
        {
            moveTime += Time.deltaTime * moveSpeed;
            Vector3 position = new Vector3(startPosition.x, startPosition.y + moveCurve.Evaluate(moveTime) * curveHeightMultiplier, startPosition.z);
            transform.position = Vector3.Lerp(position, selectedSpot.position, moveTime);
            transform.LookAt(new Vector3(selectedSpot.position.x, transform.position.y, selectedSpot.position.z));

            if (moveTime >= 1)
            {
                RestartFly();

                if (onceTime)
                    ended = true;
            }
        }

        animator.SetBool("flying", flying);
    }
}
