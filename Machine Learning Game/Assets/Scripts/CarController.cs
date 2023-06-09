using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] public Rigidbody sphere;
    [SerializeField] private Vector3 offset;


    [Header("Car stats")] [SerializeField] private float accelerationSpeed = 50;
    [SerializeField] private float steeringPower = 18f;
    [SerializeField] private float gravity = 11f;

    [Header("RotateToGround")] [SerializeField]
    private LayerMask raycastLaterMask;

    [SerializeField] private float raycastDistance;

    [Tooltip("The higher the faster")] [SerializeField]
    private float rotateToGroundSpeed = 4f;

    [SerializeField] private Drifter drifter;
    [SerializeField] private RotationBehaviour rotationBehaviour;

    [Header("Punishments")] [SerializeField]
    private float offRoadSpeedChange = -24f;

    private float _currentSpeed, _steer, _driftPower;

    private Transform _lastRaycastHit;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [SerializeField] private bool enableControls;


    void Start()
    {
        drifter.defaultDrag = sphere.drag;

        RotateToGroundNormal();

        drifter.Sphere = sphere;
    }


    void Update()
    {
        /*if (enableControls)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }*/

        transform.parent.position = sphere.transform.position + offset;


        if (enableControls)
            _currentSpeed = drifter.IsDrifting
                ? drifter.driftAccelerationSpeed * verticalInput
                : accelerationSpeed * verticalInput;
        else _currentSpeed = accelerationSpeed * verticalInput;

        if (horizontalInput != 0)
            _steer = horizontalInput * steeringPower;

        if (enableControls && drifter.HandleDrifting(out var newSteer))
            _steer = newSteer;


        HandleRotation();

        RotateToGroundNormal();


        rotationBehaviour.RotateAllWheels(_currentSpeed);
        rotationBehaviour.Steer(horizontalInput);
    }


    private void FixedUpdate()
    {
        float fromBoost = drifter.GetCurrentDriftBoost();

        float groundSpeedChange = GetGroundSpeedChange();

        var forward = transform.forward;
        sphere.AddForce(forward * _currentSpeed + fromBoost * forward + groundSpeedChange * forward,
            ForceMode.Acceleration);
    }

    private void HandleRotation()
    {
        if (MathF.Abs(_currentSpeed) < 0.1f)
            return;

        var eulerAngles = transform.eulerAngles;

        float yRotation = Mathf.Lerp(eulerAngles.y, eulerAngles.y + _steer, Time.deltaTime * 5f);

        transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, yRotation, eulerAngles.z));
    }


    private void RotateToGroundNormal()
    {
        //Cast a ray downwards to get the normal of the ground below the object
        if (Physics.Raycast(transform.position, -Vector3.up, out var hit, raycastDistance, raycastLaterMask))
        {
            // calculate the rotation needed to match the ground normal
            var transform1 = transform;
            var rotation = transform1.rotation;
            Quaternion targetRotation = Quaternion.FromToRotation(transform1.up, hit.normal) * rotation;

            // smoothly rotate the object towards the target rotation
            rotation = Quaternion.Lerp(rotation, targetRotation, rotateToGroundSpeed * Time.deltaTime);
            transform.rotation = rotation;

            _lastRaycastHit = hit.transform;
        }
    }

    private float GetGroundSpeedChange()
    {
        if (!_lastRaycastHit)
            return 0;

        if (_lastRaycastHit.transform.CompareTag("Road"))
            return 0f;

        return offRoadSpeedChange;
    }
}