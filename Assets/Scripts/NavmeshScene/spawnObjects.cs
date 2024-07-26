using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnObjects : MonoBehaviour
{
    private Navmesh navmeshScript;
    [SerializeField] private float spawnInterval = 3.0f;
    [SerializeField] private GameObject spawnObject;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        navmeshScript = GameObject.Find("CastleGuard").GetComponent<Navmesh>();
        if (navmeshScript == null)
        {
            Debug.LogError("Navmesh script not found on CastleGuard.");
        }
    }

    void Update()
    {
        if (navmeshScript.pathUpdated)
        {
            UpdatePathAndSpawnObjects();
            navmeshScript.pathUpdated = false; // Reset flag after updating
        }
    }

    void UpdatePathAndSpawnObjects()
    {
        // Clear existing objects
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();

        if (navmeshScript.agent == null || navmeshScript.nearestWaypoint == null)
        {
            Debug.LogError("Navmesh agent or nearest waypoint is not set.");
            return;
        }

        NavMeshPath path = new NavMeshPath();
        navmeshScript.agent.CalculatePath(navmeshScript.nearestWaypoint.position, path);

        if (/*path.status != NavMeshPathStatus.Complete ||*/ path.corners.Length <= 1)
        {
            Debug.LogWarning("Path calculation is incomplete or has no valid corners.");
            return;
        }

        float distanceTraveled = 0f;
        Vector3 previousCorner = path.corners[0];

        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 currentCorner = path.corners[i];
            float segmentLength = Vector3.Distance(previousCorner, currentCorner);

            while (distanceTraveled + segmentLength >= spawnInterval)
            {
                float remainingDistance = spawnInterval - distanceTraveled;
                Vector3 spawnPosition = Vector3.Lerp(previousCorner, currentCorner, remainingDistance / segmentLength);
                GameObject obj = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(obj);
                distanceTraveled = 0f;
                previousCorner = spawnPosition;
                segmentLength -= remainingDistance;
            }

            distanceTraveled += segmentLength;
            previousCorner = currentCorner;
        }
    }
}
