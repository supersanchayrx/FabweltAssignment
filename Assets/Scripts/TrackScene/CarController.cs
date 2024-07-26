using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public bool isBreaking;

    public float horizontalInput,verticalInput, currentBreakForce, currentSteeringAngle;

    Rigidbody rb;
    public float velocity;


    //wheel colliders
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;


    //wheel transform meshes
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;


    //values
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        velocity = rb.velocity.magnitude;
    }

    void GetInput()
    {

        if(this.name== "PlayerCar")
        {
            horizontalInput = Input.GetAxis(HORIZONTAL);
            verticalInput = Input.GetAxis(VERTICAL);
            isBreaking = Input.GetKey(KeyCode.Space);
        }
        

    }

    void HandleMotor()
    {
        /*frontLeft.motorTorque = verticalInput * motorForce;
        frontRight.motorTorque = verticalInput * motorForce;*/

        backLeft.motorTorque = verticalInput * motorForce;
        backRight.motorTorque = verticalInput * motorForce;


        currentBreakForce = isBreaking ? breakForce : 0f;

        if(isBreaking)
        {
            ApplyBreaking();
        }

        else
        {
            frontLeft.brakeTorque = 0f;
            frontRight.brakeTorque = 0f;
            backLeft.brakeTorque = 0f;
            backRight.brakeTorque = 0f;
        }

    }

    void ApplyBreaking()
    {
        frontLeft.brakeTorque = currentBreakForce;
        frontRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
    }

    void HandleSteering()
    {
        currentSteeringAngle = maxSteerAngle * horizontalInput;
        frontLeft.steerAngle = currentSteeringAngle;
        frontRight.steerAngle = currentSteeringAngle;

    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftTransform, frontLeft);
        UpdateSingleWheel(frontRightTransform, frontRight);
        UpdateSingleWheel(backLeftTransform, backLeft);
        UpdateSingleWheel(backRightTransform, backRight);

    }


    void UpdateSingleWheel(Transform wheelTransform, WheelCollider wheelCollider)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.position = pos;
        wheelTransform.rotation = rot;

    }

    public void SetInputs(float a, float b)
    {
        horizontalInput = b; verticalInput = a;
    }
}
