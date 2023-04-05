using DefaultNamespace;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody sphere;
    [SerializeField] private Vector3 offset;


    [Header("Car stats")] 
    [SerializeField] private float accelerationSpeed = 50;
    [SerializeField] private float steeringPower = 18f;
    [SerializeField] private float gravity = 11f;

    [Header("RotateToGround")] [SerializeField]
    private LayerMask raycastLaterMask;

    [SerializeField] private float raycastDistance;
    
    [Tooltip("The higher the faster")]
    [SerializeField] private float rotateToGroundSpeed = 4f;
    
    [Header("Drifting Settings")]
    [SerializeField] private float driftSteeringPower = 8f;
    [SerializeField] private float driftDrag = 1f;
    [SerializeField] private float driftAccelerationSpeed = 17f;
    [SerializeField] private float miniDriftEarnedPerSecond = 5f;
    [SerializeField] private float maxDriftEarnedPerSecond = 23f;
    [SerializeField] private float driftBoostDuration = 1f;
    [SerializeField] private AnimationCurve driftBoostCurve;

    [Header("Punishments")] 
    [SerializeField] private float offRoadSpeedChange = -24f;
    

    private float _currentSpeed, _defaultDrag, _steer, _driftPower, _boostTimeElapsed;
    
    private int _driftDir;
    private bool StartedDrift => Input.GetButtonDown("Jump") && Input.GetAxis("Horizontal") != 0 && !_isDrifting;
    private static bool IsHoldingDriftButton => Input.GetButton("Jump");
    private static bool EndedDrift => Input.GetButtonUp("Jump");

    private bool _isDrifting;
    
    private float _driftBoost;

    private Transform _lastRaycastHit;

    // Start is called before the first frame update
    void Start()
    {
        _defaultDrag = sphere.drag;
        
        RotateToGroundNormal();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = sphere.transform.position + offset;
        
        _currentSpeed = _isDrifting ? driftAccelerationSpeed * Input.GetAxis("Vertical") : accelerationSpeed * Input.GetAxis("Vertical");

        if (Input.GetAxis("Horizontal") != 0)
            _steer = Input.GetAxis("Horizontal") * steeringPower;
        
        HandleDrifting();

        HandleRotation();

        RotateToGroundNormal();
    }


    private void FixedUpdate()
    {
        float fromBoost = 0;
        if (_boostTimeElapsed < driftBoostDuration)
        {
            float t = _boostTimeElapsed / driftBoostDuration;
            fromBoost = driftBoostCurve.Evaluate(t) * _driftBoost;
            _boostTimeElapsed += Time.deltaTime;
        }

        float groundSpeedChange = GetGroundSpeedChange();

        var forward = transform.forward;
        sphere.AddForce(forward * _currentSpeed + fromBoost * forward + groundSpeedChange * forward, ForceMode.Acceleration);
    }

    private void HandleRotation()
    {
        var eulerAngles = transform.eulerAngles;

        float yRotation = Mathf.Lerp(eulerAngles.y, eulerAngles.y + _steer, Time.deltaTime * 5f);

        transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, yRotation, eulerAngles.z));
    }

    private void HandleDrifting()
    {
        if (EndedDrift)
        {
            EndDrift();
            return;
        }
        
        if (!IsHoldingDriftButton )
            return;
        
        if (StartedDrift)
            StartDrift();
        else if (!_isDrifting)
            return;


        float horizontalInput = Input.GetAxis("Horizontal");
        
        float currentDriftPower = (_driftDir == 1) ? horizontalInput.Map( -1, 1, 0, 2) : horizontalInput.Map(-1, 1, 2, 0);
       _driftPower += (_driftDir == 1) 
           ? horizontalInput.Map( -1, 1, miniDriftEarnedPerSecond, maxDriftEarnedPerSecond) * Time.deltaTime
           : horizontalInput.Map(-1, 1, maxDriftEarnedPerSecond, miniDriftEarnedPerSecond) * Time.deltaTime;

         _steer = driftSteeringPower * _driftDir * currentDriftPower;
    }

    private void EndDrift()
    {
        _isDrifting = false;
        sphere.drag = _defaultDrag;

        _driftBoost = _driftPower;
        _boostTimeElapsed = 0f;
    }
    
    private void StartDrift()
    {
        //Horizontal is somewhere between -1 and 1, to make it be between 0 and 2 we add 1
        _driftDir = Input.GetAxis("Horizontal") < 0 ? -1 : 1;

        _isDrifting = true;

        _driftPower = 0f;
        
        sphere.drag = driftDrag;
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
        if (_lastRaycastHit.transform.CompareTag("Road"))
            return 0f;

        return offRoadSpeedChange;
    }
}