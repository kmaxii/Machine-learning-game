using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{


    [SerializeField] private float accelerationSpeed = 500;
    [SerializeField] private float breakingStrength = 300;
    [SerializeField] private float maxTurnAngle = 20;


    private float _currentSpeed = 0f;
    private float _currentBreakForce = 0f;
    private float _currentTurnAngle = 0f;
    
    
    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheel, frontRightWheel, backLeftWheel, backRightWheel;
    [SerializeField] private Transform frontLeftTransform, frontRightTransform, backLeftTransform, backRightTransform;


    private void FixedUpdate() {
        GetInput();
    }

    private void GetInput() {
        // Steering Input
    //    horizontalInput = Input.GetAxis("Horizontal");

        HandleInput();
        UpdateMotor();
    }


    private void HandleInput()
    {
        // Acceleration Input
        _currentSpeed = accelerationSpeed * Input.GetAxis("Vertical");

        _currentBreakForce = Input.GetButton("Jump") ? breakingStrength : 0f;

        _currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

    }

    private void UpdateMotor()
    {
        UpdateEverything(frontLeftWheel, frontLeftTransform);
        UpdateEverything(frontRightWheel, frontRightTransform);
        UpdateAllButSteering(backLeftWheel, backLeftTransform);
        UpdateAllButSteering(backRightWheel, backRightTransform);
    }


    private void UpdateEverything(WheelCollider wheelCollider, Transform wheelTransform)
    {
        UpdateSteeringAngel(wheelCollider);
        UpdateAllButSteering(wheelCollider, wheelTransform);

    }

    private void UpdateAllButSteering(WheelCollider wheelCollider, Transform wheelTransform)
    {
        UpdateBrakeForce(wheelCollider);
        UpdateWheelVisuals(wheelCollider, wheelTransform);
        UpdateWheelMotor(wheelCollider);
    }

    private void UpdateWheelMotor(WheelCollider wheelCollider)
    {
        wheelCollider.motorTorque = _currentSpeed;
    }

    private void UpdateSteeringAngel(WheelCollider wheelCollider)
    {
        wheelCollider.steerAngle = _currentTurnAngle;
    }

    private void UpdateBrakeForce(WheelCollider wheelCollider)
    {
        wheelCollider.brakeTorque = _currentBreakForce;
    }
    
    private void UpdateWheelVisuals(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out var pos, out var rotation);
        wheelTransform.position = pos;
        wheelTransform.rotation = rotation;
    }
}