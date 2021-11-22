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

    [SerializeField] private Transform centerOfMass;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private float motorForce = 10.0f;
    [SerializeField] private float breakForce = 10.0f;
    [SerializeField] private float maxSteerAngle = 10.0f;
    [SerializeField] private float zRotationLimit = 20.0f;

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
    public bool RealDriftInput { get { return realDriftPressed > 0; } }
    public bool RealAnyInput { get { return RealForwardInput || RealBackwardInput || RealBreakInput || RealTurboInput; } }
    public bool UsedForwardInput { get { return usedVerticalInput > 0; } }
    public bool UsedBackwardInput { get { return usedVerticalInput < 0; } }
    public bool UsedBreakInput { get { return usedIsBreaking > 0; } }
    public bool UsedTurboInput { get { return usedTurboPressed > 0; } }
    public bool UsedDriftInput { get { return usedDriftPressed > 0; } }
    public bool UsedAnyInput { get { return UsedForwardInput || UsedBackwardInput || UsedBreakInput || UsedTurboInput; } }
    public bool IsPlaying { get { return isPlaying; } }



    //"Boogie Beat" extra controls
    private float
        realTurboPressed = 0,  //TO DO: el turbo té una duració acumulada màxima
        usedTurboPressed = 0,
        realDriftPressed = 0,
        usedDriftPressed = 0;
        
    private float turboMotorForce;
    [SerializeField] private float turboForce = 10.0f;
    private bool drift = false;
    private Vector3 angularVel;
    private int consecutiveMistakes = 0;
    
    [Range(0.5f, 10.0f)] [SerializeField] private float downForce = 1.0f;

    private float currSteerAngle;
    private float maxAngularVel;
    [SerializeField] private float maxAngularVelForJJ = 0.5f;
    [SerializeField] private float maxVelocity = 20.0f;
    [SerializeField] private float maxTurboVelocity = 50.0f;

    //Speed of turn
    [Range(0.001f, 1.0f)]
    [SerializeField] float steerSpeed = 0.2f;
    public float SteerSpeed { get { return steerSpeed; } set { steerSpeed = Mathf.Clamp(value, 0.001f, 1.0f); } }

    [Range(0.0f, 2f)]
    [SerializeField] float driftIntensity = 1f;
    public float DriftIntensity { get { return driftIntensity; } set { driftIntensity = Mathf.Clamp(value, 0.0f, 2.0f); } }

    [SerializeField] float angularForce = 7f;
    [SerializeField] float angularMomentumPercent = 0.1f;

    PlayerCarTurbo turboScript;
    private Rigidbody rb;
    private Vector3 newCenterOfMass = new Vector3(0.0f, -0.9f, 0.0f);
    private bool isPlaying = true;
    private bool wrongTimingTriggered = false;

    //Controller type
    int controllerType; //0 for keyboard, 1 for gamepad

    void Start()
    {
        turboScript = GetComponent<PlayerCarTurbo>();

        rb = GetComponent<Rigidbody>();
        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
        maxAngularVel = rb.maxAngularVelocity;

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleDrift();
        UpdateWheels();
        ApplyDownForce();

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

    public void OnDrift(InputAction.CallbackContext context)
    {
        realDriftPressed = context.ReadValue<float>();
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
        if (UsedTurboInput && turboScript.HasFuel)
        {
            Debug.Log("InTurbo");
            rb.AddForce(transform.forward * turboForce, ForceMode.Force);
            if (rb.velocity.x > maxTurboVelocity)
                rb.velocity = new Vector3(maxTurboVelocity, rb.velocity.y, rb.velocity.z);
            if (rb.velocity.y > maxTurboVelocity)
                rb.velocity = new Vector3(rb.velocity.x, maxTurboVelocity, rb.velocity.z);
            if (rb.velocity.z > maxTurboVelocity)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxTurboVelocity);

            turboScript.DecreaseTurboFuel();
        }
        else
        {
            turboMotorForce = 0;
        }

        frontLeftWheelCollider.motorTorque = usedVerticalInput * (motorForce /*+ turboMotorForce*/);
        frontRightWheelCollider.motorTorque = usedVerticalInput * (motorForce /*+ turboMotorForce*/);


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

        if (!(UsedTurboInput && turboScript.HasFuel))
        {
            if (rb.velocity.x > maxVelocity)
                rb.velocity = new Vector3(maxVelocity, rb.velocity.y, rb.velocity.z);
            if (rb.velocity.z > maxVelocity)
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxVelocity);
            //Debug.Log("Linear Velocity: " + rb.velocity);
        }

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
        //Limit a la velocitat angular del cotxe perquè no volqui (cutre, ho sé)
        //if (transform.rotation.eulerAngles.z < zRotationLimit || transform.rotation.eulerAngles.z > 360 - zRotationLimit)
        //{
        //    rb.maxAngularVelocity = maxAngularVel;
        //}
        //else if (transform.rotation.eulerAngles.z > zRotationLimit && transform.rotation.eulerAngles.z <= 180)
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zRotationLimit);
        //    rb.maxAngularVelocity = 1.5f;
        //}
        //else if (transform.rotation.eulerAngles.z < 360 - zRotationLimit && transform.rotation.eulerAngles.z > 180)
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 360 - zRotationLimit);
        //    rb.maxAngularVelocity = 1.5f;
        //}

        //El mateix de dalt però per al nivell JJ (no entenc perquè en aquesta escena la velocitat angular és sobre y i a l'altre és de x i z)
        if (rb.angularVelocity.y >= -maxAngularVelForJJ && rb.angularVelocity.y <= maxAngularVelForJJ) { }
        else if (rb.angularVelocity.y > maxAngularVelForJJ)
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, maxAngularVelForJJ, rb.angularVelocity.z);
        else if (rb.angularVelocity.y < -maxAngularVelForJJ)
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, -maxAngularVelForJJ, rb.angularVelocity.z);

        if (horizontalInput == 0)
            rb.angularVelocity = new Vector3(0, 0, 0);

        //Debug.Log("Z rotation: " + transform.rotation.eulerAngles.z);
        //Debug.Log("Angular Velocity: " + rb.angularVelocity);

        currSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currSteerAngle;
        frontRightWheelCollider.steerAngle = currSteerAngle;
    }

    private void HandleDrift()
    {
        if (UsedDriftInput)
        {
            float currSpeed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;

            Vector3 driftGForce = -transform.right;
            driftGForce.y = 0.0f;
            driftGForce.Normalize();
            //Vector3 ForwardDriftForce = transform.forward;
            //ForwardDriftForce.y = 0.0f;
            //ForwardDriftForce.Normalize();

            if (currSteerAngle != 0)
            {
                driftGForce *= rb.mass * currSpeed / angularForce /** throttle*/ * currSteerAngle / maxSteerAngle;
                //ForwardDriftForce *= rb.mass * currSpeed / angularForce /** throttle*/ * currSteerAngle / maxSteerAngle;
            }
            Vector3 driftTorque = transform.up * angularMomentumPercent * currSteerAngle / maxSteerAngle;

            frontLeftWheelCollider.motorTorque += motorForce /*+ turboMotorForce*/;
            frontRightWheelCollider.motorTorque += motorForce /*+ turboMotorForce*/;

            rb.AddForce(driftGForce * driftIntensity, ForceMode.Force);
            //rb.AddForce(ForwardDriftForce * driftIntensity, ForceMode.Force);
            rb.AddTorque(driftTorque * driftIntensity, ForceMode.VelocityChange);

        }
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

    private void ApplyDownForce()
    {
        float currSpeed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;
        rb.AddForce(-transform.up * currSpeed * downForce);
    }


    public void TriggerWrongTiming()
    {
        wrongTimingTriggered = true;
        consecutiveMistakes++;

        usedIsBreaking = 1.0f;
        usedTurboPressed = 0.0f;
        //if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_3)
        //{
        //    breakDown = true;
        //}

    }

    public void StopCompletely()
    {
        usedIsBreaking = 1.0f;
        usedTurboPressed = 0.0f;
        frontLeftWheelCollider.motorTorque = frontRightWheelCollider.motorTorque = 0.0f;
        rb.velocity = Vector3.zero;

    }

    public void RefreshInputs()
    {
        if (wrongTimingTriggered)
        {
            //Debug.Log(1);
            wrongTimingTriggered = false;

            //if (consecutiveMistakes >= (int)Const.MistakesTiers.TIER_3) breakDown = false;

        }
        else
        {
            //Debug.Log(2);

            if (consecutiveMistakes > 0)
                consecutiveMistakes = 0;

            usedVerticalInput = realVerticalInput;
            usedIsBreaking = realIsBreaking;
            usedTurboPressed = realTurboPressed;
            
        }
        usedDriftPressed = realDriftPressed;
    }

}
