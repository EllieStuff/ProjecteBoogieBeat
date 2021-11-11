using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //Basic controls
    private bool
        forwardPressed = false,
        backwardPressed = false,
        rightPressed = false,
        leftPressed = false;
    private float horizontalInput;
    private float verticalInput;
    private float currBreakForce;
    private float isBreaking;

    //"Boogie Beat" extra controls
    private float turboPressed = 0;  //TO DO: el turbo té una duració acumulada màxima
    private float overdriveMotorForce;
    [SerializeField] private float overdriveMultiplier = 1.0f;
    private bool leftDrift = false;
    private bool rightDrift = false;
    private Vector3 angularVel;

    private float currSteerAngle;

    private Rigidbody rb;
    private Vector3 newCenterOfMas = new Vector3(0.0f, -0.9f, 0.0f);

    //Controller type
    int controllerType; //0 for keyboard, 1 for gamepad

    void Start()
    {
        //InitWheels();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = newCenterOfMas;
        
        
    }

    private void Update()
    {
        //GetInput();
        //angularVel = rb.angularVelocity;
        Debug.Log("Vertical: " + verticalInput);
        Debug.Log("Horizontal: " + horizontalInput);
        Debug.Log("motorTorque: " + frontLeftWheelCollider.motorTorque);
        //Debug.Log("tmpMaxSteerAngle: " + tmpMaxSteerAngle);
        //Debug.Log("maxSteerAngle: " + maxSteerAngle);
        //Debug.Log("currSteerAngle: " + currSteerAngle);
        //Debug.Log("angleVelocity: " + angularVel);
        //Debug.Log("rb.inertiaTensor: " + rb.inertiaTensor);
        //Debug.Log("rb.inertiaTensorRotation: " + rb.inertiaTensorRotation);
        Debug.Log("isBreaking: " + isBreaking);
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();

    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>();
    }
    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }
    public void OnBrake(InputAction.CallbackContext context)
    {
        isBreaking = context.ReadValue<float>();
    }
    public void OnTurbo(InputAction.CallbackContext context)
    {
        turboPressed = context.ReadValue<float>();
    }




    //private void GetInput()
    //{
    //    //Basic controls
    //    //forwardPressed = Input.GetKey(KeyCode.W);
    //    //backwardPressed = Input.GetKey(KeyCode.S);
    //    //rightPressed = Input.GetKey(KeyCode.D);
    //    //leftPressed = Input.GetKey(KeyCode.A);
        
    //    //horizontalInput = Input.GetAxis("Horizontal");
    //    //verticalInput = Input.GetAxis("Vertical");
    //    //isBreaking = Input.GetKey(KeyCode.Space);

    //    ////"Boogie Beat" extra controls
    //    //turboPressed = Input.GetKey(KeyCode.LeftShift);


    //}


    private void HandleMotor()
    {
        if(verticalInput > 0)
            overdriveMotorForce = turboPressed==1 ? motorForce * overdriveMultiplier : 0.0f;
        frontLeftWheelCollider.motorTorque = verticalInput * (motorForce + overdriveMotorForce);
        frontRightWheelCollider.motorTorque = verticalInput * (motorForce + overdriveMotorForce);

        currBreakForce = isBreaking==1 ? breakForce : 0.0f;
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
        //if (!rightPressed && !leftPressed || rightPressed && leftPressed)
            //horizontalInput = 0.0f;

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
