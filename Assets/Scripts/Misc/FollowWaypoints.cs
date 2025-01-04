using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    enum MoveType
    {
        RigidbodyForce, RigidbodyVelocity, RigidbodyHorizontalVelocity, TransformPosition
    }
    enum ReachToEnd
    {
        CirclePath, ReversePath, Stop
    }


    [field:Header("References")]
    [field: SerializeField] public Rigidbody Rb { get; private set; }

    [Header("Waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] Vector3[] relativeWaypointDirections;
    [SerializeField] private bool relativeWaypoints;
    private int waypointIndex, maxWaypoints;
    Vector3 direction = Vector3.zero;
    Vector3 currentWaypointPosition = Vector3.zero;
    bool reachedWaypoint = false;

    [Header("Movement")]
    [SerializeField] private MoveType moveType;
    [SerializeField] private ReachToEnd onReachToEnd;
    [SerializeField] private float speed;
    [SerializeField] private bool normalizeSpeedDistance = true;
    [SerializeField] private bool reversePath = false;

    [Header("Reach Waypoints")]
    [SerializeField] private float minDistance;
    [SerializeField] private bool changeWaypointByTimeMoved;
    [SerializeField] private float maxTimeMoving;
    float time = 0;

    [Header("Look ")]
    [SerializeField] bool lookForward = false;
    [SerializeField] bool onlyHorizontalLook = false;
    [SerializeField] private float lookSpeed;




    void Start()
    {
        if (!Rb)
        {
            TryGetComponent(out Rigidbody rigidbody);
            Rb = rigidbody;
        }
        SetRelativeWaypointsDirections();
    }

    void SetRelativeWaypointsDirections()
    {
        if (waypoints == null || waypoints.Length == 0 || relativeWaypointDirections.Length > 0) return;
        relativeWaypointDirections = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            //Calcular direcciones de todos los waypoints menos el ultimo
            if (i < waypoints.Length - 1)
                relativeWaypointDirections[i] = waypoints[i + 1].position - waypoints[i].position;

            //Calcular direccion del último waypoint
            else
            {
                switch (onReachToEnd)
                {
                    case ReachToEnd.CirclePath:
                        relativeWaypointDirections[i] = waypoints[0].position - waypoints[i].position;
                        break;
                    case ReachToEnd.ReversePath:
                        relativeWaypointDirections[i] = waypoints[i - 1].position - waypoints[i].position;
                        break;
                    case ReachToEnd.Stop:
                        relativeWaypointDirections[i] = Vector3.zero;
                        break;
                }
            }
        }
    }

    private void Update()
    {
        //Waypoint position
        if (relativeWaypoints)
        {
            if (waypointIndex == 0 && currentWaypointPosition == Vector3.zero)
                //currentWaypointPosition = transform.TransformVector(relativeWaypointDirections[waypointIndex]);
                currentWaypointPosition = transform.position + relativeWaypointDirections[waypointIndex];
            maxWaypoints = relativeWaypointDirections.Length;
        }
        else
        {
            maxWaypoints = waypoints.Length;
            if (waypointIndex >= 0 && waypointIndex < maxWaypoints) 
                currentWaypointPosition = waypoints[waypointIndex].position;
        }

        //Waypoint direction
        direction = CalculateDirection(currentWaypointPosition, transform.position);

        //Check if reached waypoint
        if (!changeWaypointByTimeMoved)
            reachedWaypoint = Vector3.Distance(transform.position, currentWaypointPosition) <= minDistance;
        else
            reachedWaypoint = (Vector3.Distance(transform.position, currentWaypointPosition) <= minDistance) || (time >= maxTimeMoving);

        //Move
        if (!reachedWaypoint)
        {
            Move();
        }
        //REACH WAYPOINT
        else
        {
            //Next Waypoint
            if(!reversePath)
                waypointIndex++;
            else
                waypointIndex--;

            //REACH END PATH
            if (waypointIndex > maxWaypoints-1 || waypointIndex < 0)
            switch (onReachToEnd)
            {
                case ReachToEnd.CirclePath:
                        //Index is outside of array
                        if (waypointIndex > maxWaypoints - 1) waypointIndex = 0;
                        if (waypointIndex < 0) waypointIndex = maxWaypoints - 1;
                        break;
                case ReachToEnd.ReversePath:
                        //Index is outside of array
                        if (waypointIndex > maxWaypoints - 1) waypointIndex = maxWaypoints - 1;
                        if (waypointIndex < 0) waypointIndex = 0;
                        //Reverse path
                        reversePath = !reversePath;
                    break;
                case ReachToEnd.Stop:
                        //Index is outside of array
                        if (waypointIndex > maxWaypoints - 1) waypointIndex = maxWaypoints - 1;
                        if (waypointIndex < 0) waypointIndex = 0;
                        //Stop
                        enabled = false;
                    break;
            }

            //Set current waypoint in relative position
            if (relativeWaypoints) //currentWaypointPosition = transform.TransformPoint(relativeWaypointDirections[waypointIndex]);
                currentWaypointPosition = transform.position + relativeWaypointDirections[waypointIndex];

            //Reset time (changeWaypointByTimeMoved)
            time = 0;
        }
    }

    private void LateUpdate()
    {
        //Look to move direction
        Look();
    }

    Vector3 CalculateDirection(Vector3 targetPosition, Vector3 currentPosition)
    {
        if (normalizeSpeedDistance)
            return (targetPosition - currentPosition).normalized;
        else
            return targetPosition - currentPosition;
    }

    void Move()
    {
        if (!reachedWaypoint)
        {
            //Move
            switch (moveType)
            {
                case MoveType.RigidbodyForce:
                    Rb.AddForce(direction * speed);
                    break;
                case MoveType.RigidbodyVelocity:
                    Rb.velocity = direction * speed;
                    break;
                case MoveType.RigidbodyHorizontalVelocity:
                    Rb.velocity = new Vector3(direction.x * speed, Rb.velocity.y, direction.z * speed);
                    break;
                case MoveType.TransformPosition:
                    transform.position += direction * speed * Time.deltaTime;
                    break;
            }
        }

        //changeWaypointByTimeMoved
        time += Time.deltaTime;
    }

    void Look()
    {

        //Look to move direction
        if (lookForward)
        {
            Vector3 lookDirection = direction;
            if (onlyHorizontalLook) lookDirection.y = 0;
            Rb.MoveRotation(Quaternion.Slerp(Rb.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), lookSpeed * Time.deltaTime));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(relativeWaypointDirections.Length > 0)
        {
            Gizmos.color = Color.black;
            foreach (Vector3 dir in relativeWaypointDirections)
                Gizmos.DrawRay(transform.position, dir);
        }
        
    }
}


