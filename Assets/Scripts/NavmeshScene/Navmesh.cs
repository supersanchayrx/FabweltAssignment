using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navmesh : MonoBehaviour
{
    public NavMeshAgent agent;

    private PlayerMovement playerMovementScript;
    private GameObject[] waypoints;
    public Transform nearestWaypoint;
    public Transform[] waypointTransforms;

    public bool pathUpdated = false;

    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoints");
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = GetComponent<PlayerMovement>();
        waypointTransforms = new Transform[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointTransforms[i] = waypoints[i].transform;
        }

        nearestWaypoint = findNextWaypoint();
        pathUpdated = true;
    }

    void Update()
    {
        Transform nextWayPoint = findNextWaypoint();

        if (nearestWaypoint != nextWayPoint)
        {
            nearestWaypoint = nextWayPoint;
            pathUpdated = true;
        }


        agent.SetDestination(nearestWaypoint.position);


        if (Mathf.Abs(agent.velocity.magnitude) > 0.2f)
        {
            playerMovementScript.isMoving = true;
            playerMovementScript.isWalking = true;
        }
        else
        {
            playerMovementScript.isMoving = false;
            playerMovementScript.isWalking = false;
        }
    }

    Transform findNextWaypoint()
    {
        Transform nextWaypoint = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform waypointTransform in waypointTransforms)
        {
            float distance = Vector3.Distance(waypointTransform.position, transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nextWaypoint = waypointTransform;
            }
        }
        return nextWaypoint;
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navmesh : MonoBehaviour
{
    public NavMeshAgent agent;
    private PlayerMovement playerMovementScript;
    private GameObject[] waypoints;
    public Transform nearestWaypoint;
    public Transform[] waypointTransforms;

    public bool pathUpdated = false;

    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoints");
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = GetComponent<PlayerMovement>();
        waypointTransforms = new Transform[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointTransforms[i] = waypoints[i].transform;
        }

        nearestWaypoint = findNextWaypoint();
        agent.SetDestination(nearestWaypoint.position);
        pathUpdated = true;
    }

    void Update()
    {
        Transform nextWayPoint = findNextWaypoint();

        if (nearestWaypoint != nextWayPoint)
        {
            nearestWaypoint = nextWayPoint;
            agent.SetDestination(nearestWaypoint.position);
            pathUpdated = true; // Ensure this is set when waypoint changes
        }

        if (Mathf.Abs(agent.velocity.magnitude) > 0.2f)
        {
            playerMovementScript.isMoving = true;
            playerMovementScript.isWalking = true;
        }
        else
        {
            playerMovementScript.isMoving = false;
            playerMovementScript.isWalking = false;
        }
    }

    Transform findNextWaypoint()
    {
        Transform nextWaypoint = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform waypointTransform in waypointTransforms)
        {
            float distance = Vector3.Distance(waypointTransform.position, transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nextWaypoint = waypointTransform;
            }
        }
        return nextWaypoint;
    }
}*/
