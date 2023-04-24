
using System;
using UnityEngine;

[Serializable]
public class RotationBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float wheelSteerSpeed = 10f;
    [Range(0, 360)]
    [SerializeField] private float maxWheelSteerAmount = 45f;
    
    
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform backRightWheel;
    [SerializeField] private Transform backLeftWheel;
    [SerializeField] private Transform steeringWheel;


    public void RotateAllWheels(float speed)
    {
        RotateWheel(speed, frontLeftWheel);
        RotateWheel(speed, frontRightWheel);
        RotateWheel(speed, backLeftWheel);
        RotateWheel(speed, backRightWheel);
    }
    
    
    
    private void RotateWheel(float speed, Transform wheel)
    {
        float rotation = speed * rotationSpeed * Time.deltaTime;
        wheel.Rotate(rotation, 0, 0);
    }

    public void Steer(float dir)
    {
        SteerWheel(dir, frontLeftWheel);
        SteerWheel(dir, frontRightWheel);
        RotateSteeringWheel(dir);
    }
    
    private void SteerWheel(float dir, Transform wheel)
    {
        Quaternion targetRotation = Quaternion.Euler(0, dir * maxWheelSteerAmount, 0);

        wheel.transform.localRotation = targetRotation;
        //wheel.transform.rotation = Quaternion.Lerp(wheel.transform.rotation, targetRotation, Time.deltaTime * wheelSteerSpeed);
    }

    private void RotateSteeringWheel(float dir)
    {
        steeringWheel.Rotate(steeringWheel.forward, dir * 100f * Time.deltaTime);
        Debug.DrawLine(steeringWheel.position, steeringWheel.position + steeringWheel.forward * 10f);
    }
    
    
}