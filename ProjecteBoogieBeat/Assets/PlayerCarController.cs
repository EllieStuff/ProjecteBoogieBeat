using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private float motorForce = 10.0f;
    [SerializeField] private float breakForce = 10.0f;
    [SerializeField] private float maxSteerAngle = 10.0f;


    private bool
        forwardPressed = false,
        backwardPressed = false,
        rightPressed = false,
        leftPressed = false;
    private float horizontalInput;
    private float verticalInput;
    private float currBreakForce;
    private bool isBreaking;
    private float currSteerAngle;

    private Rigidbody rb;
    private Vector3 newCenterOfMas = new Vector3(0.0f, -0.9f, 0.0f);


    void Start()
    {
        //InitWheels();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = newCenterOfMas;

    }

    private void Update()
    {
        GetInput();
        
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();

    }


    private void GetInput()
    {
        forwardPressed = Input.GetKey(KeyCode.W);
        backwardPressed = Input.GetKey(KeyCode.S);
        rightPressed = Input.GetKey(KeyCode.D);
        leftPressed = Input.GetKey(KeyCode.A);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);

    }


    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        currBreakForce = isBreaking ? breakForce : 0.0f;
        ApplyBreaking();

    }
    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currBreakForce;
        rearLeftWheelCollider.brakeTorque = currBreakForce;
        frontRightWheelCollider.brakeTorque = currBreakForce;
        rearRightWheelCollider.brakeTorque = currBreakForce;

    }


    private void HandleSteering()
    {
        if (!rightPressed && !leftPressed || rightPressed && leftPressed)
            horizontalInput = 0.0f;

        currSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currSteerAngle;
        frontRightWheelCollider.steerAngle = currSteerAngle;

    }


    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);

    }
    private void UpdateWheel(WheelCollider _wheelCollider, Transform _wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        _wheelCollider.GetWorldPose(out pos, out rot);

        _wheelTransform.position = pos;
        _wheelTransform.rotation = rot;

    }

}
