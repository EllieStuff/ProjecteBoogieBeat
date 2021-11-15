using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
    const float BREAK_INCREMENT = 2.5f;

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
        leftPressed = false,
        breakDown = false;
    private float
        currBreakForce,
        horizontalInput,
        realVerticalInput,
        realIsBreaking,
        //usedHorizontalInput,  //no utilizamos el ritmo para este, asi que no deberia ser necesario dividirlo
        usedVerticalInput,
        usedIsBreaking;
    public bool RealForwardInput { get { return realVerticalInput > 0; } }
    public bool RealBackwardInput { get { return realVerticalInput < 0; } }
    public bool RealBreakInput { get { return realIsBreaking > 0; } }
    public bool RealTurboInput { get { return realTurboPressed > 0; } }
    public bool RealAnyInput { get { return RealForwardInput || RealBackwardInput || RealBreakInput || RealTurboInput; } }
    public bool UsedForwardInput { get { return usedVerticalInput > 0; } }
    public bool UsedBackwardInput { get { return usedVerticalInput < 0; } }
    public bool UsedBreakInput { get { return usedIsBreaking > 0; } }
    public bool UsedTurboInput { get { return usedTurboPressed > 0; } }
    public bool UsedAnyInput { get { return UsedForwardInput || UsedBackwardInput || UsedBreakInput || UsedTurboInput; } }
    public bool IsPlaying { get { return isPlaying; } }



    //"Boogie Beat" extra controls
    private float
        realTurboPressed = 0,  //TO DO: el turbo té una duració acumulada màxima
        usedTurboPressed = 0;
    private float overdriveMotorForce;
    [SerializeField] private float overdriveMultiplier = 1.0f;
    private bool leftDrift = false;
    private bool rightDrift = false;
    private Vector3 angularVel;
    private int consecutiveMistakes = 0;

    private float currSteerAngle;

    private Rigidbody rb;
    private Vector3 newCenterOfMas = new Vector3(0.0f, -0.9f, 0.0f);
    private bool isPlaying = true;
    private bool wrongTimingTriggered = false;

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
        ////angularVel = rb.angularVelocity;
        //Debug.Log("Vertical: " + verticalInput);
        //Debug.Log("Horizontal: " + horizontalInput);
        //Debug.Log("motorTorque: " + frontLeftWheelCollider.motorTorque);
        ////Debug.Log("tmpMaxSteerAngle: " + tmpMaxSteerAngle);
        ////Debug.Log("maxSteerAngle: " + maxSteerAngle);
        ////Debug.Log("currSteerAngle: " + currSteerAngle);
        ////Debug.Log("angleVelocity: " + angularVel);
        ////Debug.Log("rb.inertiaTensor: " + rb.inertiaTensor);
        ////Debug.Log("rb.inertiaTensorRotation: " + rb.inertiaTensorRotation);
        //Debug.Log("isBreaking: " + isBreaking);
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();

    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        realVerticalInput = context.ReadValue<float>();
    }
    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }
    public void OnBrake(InputAction.CallbackContext context)
    {
        realIsBreaking = context.ReadValue<float>();
    }
    public void OnTurbo(InputAction.CallbackContext context)
    {
        realTurboPressed = context.ReadValue<float>();
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
        if(usedVerticalInput > 0)
            overdriveMotorForce = usedTurboPressed == 1 ? motorForce * overdriveMultiplier : 0.0f;
        frontLeftWheelCollider.motorTorque = usedVerticalInput * (motorForce + overdriveMotorForce);
        frontRightWheelCollider.motorTorque = usedVerticalInput * (motorForce + overdriveMotorForce);


        if (UsedBreakInput)
        {
            // Castigs
            if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_3) currBreakForce = breakForce * BREAK_INCREMENT;
            else if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_2) currBreakForce = breakForce;
            else if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_1) currBreakForce = breakForce / BREAK_INCREMENT;
            
            // Fre normal
            if (consecutiveMistakes == (int)Const.MistakesTiers.NONE) currBreakForce = breakForce;
        }
        else
        {
            if (!breakDown) currBreakForce = 0.0f;
        }
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

    public void TriggerWrongTiming()
    {
        wrongTimingTriggered = true;
        consecutiveMistakes++;

        usedIsBreaking = 1.0f;
        //if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_3)
        //{
        //    breakDown = true;
        //}

    }

    public void RefreshInputs()
    {
        if (wrongTimingTriggered)
        {
            Debug.Log(1);
            wrongTimingTriggered = false;

            //if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_3) breakDown = false;

        }
        else
        {
            Debug.Log(2);

            if (consecutiveMistakes > 0)
                consecutiveMistakes = 0;

            usedVerticalInput = realVerticalInput;
            usedIsBreaking = realIsBreaking;
            usedTurboPressed = realTurboPressed;
        }

    }

}
