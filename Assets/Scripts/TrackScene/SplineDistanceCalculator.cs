using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using TMPro;

public class SplineDistanceCalculator : MonoBehaviour
{
    public SplineContainer splineContainer;
    public Transform playerCar; 
    public Transform aiCar; 
    public TextMeshProUGUI distanceText, lapCountText;

    public List<Vector3> splineKnots; 

    [SerializeField] private int playerLap, aiLap;

    private void Start()
    {
        PopulateSplineKnots();
        playerLap = 0;
        aiLap = 0;
    }

    void Update()
    {
        float distanceAlongSpline = CalculateTrackDistance(playerCar.position, aiCar.position, splineKnots);
        distanceText.text = distanceAlongSpline.ToString("F2") + " m"; //idk somestring stuff

        UpdateLapCount();
    }

    int FindClosestKnot(Vector3 carPosition, List<Vector3> splineKnots)
    {
        int closestKnotIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < splineKnots.Count; i++)
        {
            float distance = Vector3.Distance(carPosition, splineKnots[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestKnotIndex = i;
            }
        }

        return closestKnotIndex;
    }

    float CalculateSplineDistance(int startKnot, int endKnot, List<Vector3> splineKnots)
    {
        float distance = 0f;

        if (startKnot > endKnot)
        {   
            //simple sort kra and comparing ... below
            int temp = startKnot;
            startKnot = endKnot;
            endKnot = temp;
        }

        for (int i = startKnot; i < endKnot; i++)
        {
            distance += Vector3.Distance(splineKnots[i], splineKnots[i + 1]);
        }

        return distance;
    }

    Vector3 InterpolatePositionOnSpline(Vector3 carPosition, int closestKnot, List<Vector3> splineKnots)
    {
        //will increase precision because we're interpolating the distrarnces between each knot too. Yes increasing knots would increase the precision. YES I FORGOR TO DO THAT 

        Vector3 nextKnot = splineKnots[(closestKnot + 1) % splineKnots.Count];
        Vector3 direction = (nextKnot - splineKnots[closestKnot]).normalized;

        float distanceToNextKnot = Vector3.Distance(splineKnots[closestKnot], nextKnot);
        float distanceToCar = Vector3.Distance(splineKnots[closestKnot], carPosition);

        float t = distanceToCar / distanceToNextKnot;
        return Vector3.Lerp(splineKnots[closestKnot], nextKnot, t);
    }

    float CalculateTrackDistance(Vector3 playerCarPosition, Vector3 aiCarPosition, List<Vector3> splineKnots)
    {
        int playerKnot = FindClosestKnot(playerCarPosition, splineKnots);
        Debug.Log("Closest knot to player: " + playerKnot);
        int aiKnot = FindClosestKnot(aiCarPosition, splineKnots);
        Debug.Log("Closest knot to AI: " + aiKnot);


        //dono ka distances from knot found
        Vector3 playerSplinePos = InterpolatePositionOnSpline(playerCarPosition, playerKnot, splineKnots);
        Vector3 aiSplinePos = InterpolatePositionOnSpline(aiCarPosition, aiKnot, splineKnots);

        int startKnot = Mathf.Min(playerKnot, aiKnot);
        int endKnot = Mathf.Max(playerKnot, aiKnot);

        float distanceAlongSpline = CalculateSplineDistance(startKnot, endKnot, splineKnots);

        // Add the distances from the cars to their respective closest knots
        distanceAlongSpline += Vector3.Distance(playerCarPosition, playerSplinePos);
        distanceAlongSpline += Vector3.Distance(aiCarPosition, aiSplinePos);

        return distanceAlongSpline;
    }

    void PopulateSplineKnots()
    {

        //filling in the list for knots. Can do it manually but omg no

        splineKnots = new List<Vector3>();

        if (splineContainer != null)
        {
            Spline spline = splineContainer.Spline;

            for (int i = 0; i < spline.Count; i++)
            {
                // Convert local spline knot positions to world positions
                splineKnots.Add(splineContainer.transform.TransformPoint(spline[i].Position));
            }
        }
        else
        {
            Debug.LogError("SplineContainer is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //could be done in more efficient way idk
        if (other.name == "PlayerCar")
        {
            playerLap++;
        }

        if (other.name == "AiCar")
        {
            aiLap++;
        }
    }

    void UpdateLapCount()
    {
        //lap counter

        int lapDifference = playerLap - aiLap;

        if (lapDifference == 0)
        {
            lapCountText.text = null;
        }
        else
        {
            string sign = lapDifference > 0 ? "+" : "-";
            lapCountText.text = $"{sign} {Mathf.Abs(lapDifference)} lap";
        }
    }
}

