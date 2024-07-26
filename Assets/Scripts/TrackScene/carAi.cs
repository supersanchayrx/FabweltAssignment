using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carAi : MonoBehaviour
{

    [SerializeField] private Transform[] targetPositionTransforms;
    CarController carControllerScript;
    public Vector3 targetPosition;

    public int currentWayPointIndex;
    [SerializeField] private float stopDistance, maxForwardInput, maxTurnInput;

    private void Awake()
    {
        carControllerScript = GetComponent<CarController>();
    }
    void Start()
    {
        currentWayPointIndex = 0;
        if (targetPositionTransforms.Length > 0)
        {
            SetTargetPosition(targetPositionTransforms[currentWayPointIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        float forwardInput = 0f;
        float turnInput = 0f;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > stopDistance)
        {
            Vector3 directionToMovePosition = (targetPosition - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, directionToMovePosition);

            if (dot > 0)
            {
                forwardInput = maxForwardInput;
            }

            else
            {
                forwardInput = -maxForwardInput;
            }


            float angleToDir = Vector3.SignedAngle(transform.forward, directionToMovePosition, Vector3.up);

            if (angleToDir > 0)
            {
                turnInput = maxTurnInput;
            }

            else
            {
                turnInput = -maxTurnInput;
            }
        }

        else
        {
            if(carControllerScript.velocity >1.5f)
            {
                carControllerScript.currentBreakForce = 800f;
            }

            carControllerScript.currentBreakForce = 0f;
            forwardInput = 0f; turnInput = 0f;

            currentWayPointIndex = (currentWayPointIndex + 1) % targetPositionTransforms.Length;
            SetTargetPosition(targetPositionTransforms[currentWayPointIndex].position);
        }

        

        carControllerScript.SetInputs(forwardInput, turnInput);
    }


    void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
