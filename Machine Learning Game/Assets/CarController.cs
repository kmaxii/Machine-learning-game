using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody sphere;
    [SerializeField] private Vector3 offset;


    [Header("Car stats")] [SerializeField] private float accelerationSpeed = 50;
    [SerializeField] private float steeringPower = 18f;
    [SerializeField] private float driftSteeringPower = 10f;
    [SerializeField] private float gravity = 11f;

    [Header("RotateToGround")] [SerializeField]
    private LayerMask raycastLaterMask;

    [SerializeField] private float raycastDistance;
    [SerializeField] private float rotateToGroundSpeed = 7.5f;

    private float _currentSpeed;

    private float _driftDir;
    private bool StartedDrift => Input.GetButtonDown("Jump") && Input.GetAxis("Horizontal") != 0 && !_isDrifting;
    private bool IsDrifting => Input.GetButton("Jump");
    private bool EndedDrift => Input.GetButtonUp("Jump");

    private bool _isDrifting;

    private float _steer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = sphere.transform.position + offset;


        _currentSpeed = accelerationSpeed * Input.GetAxis("Vertical");

        if (Input.GetAxis("Horizontal") != 0)
            _steer = Input.GetAxis("Horizontal") * steeringPower;
        
        HandleDrifting();

        HandleRotation();

        RotateToGroundNormal();
    }


    private void FixedUpdate()
    {
        sphere.AddForce(transform.forward * _currentSpeed, ForceMode.Acceleration);
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
            _isDrifting = false;
            return;
        }
        
        if (!IsDrifting )
            return;
        
        if (StartedDrift)
            StartDrift();
        
        
        Debug.Log("Is drifting");

        
        float currentDriftPower = Input.GetAxis("Horizontal") < 0 ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");

        _steer = driftSteeringPower * _driftDir * currentDriftPower;
    }

    private void StartDrift()
    {
        //Horizontal is somewhere between -1 and 1, to make it be between 0 and 2 we add 1

        _driftDir = Input.GetAxis("Horizontal") < 0 ? -1 : 1;

        _isDrifting = true;
    }

    private void RotateToGroundNormal()
    {
        // cast a ray downwards to get the normal of the ground below the object

        if (Physics.Raycast(transform.position, -Vector3.up, out var hit, raycastDistance, raycastLaterMask))
        {
            Debug.Log("Raycast hit");
            // calculate the rotation needed to match the ground normal
            var transform1 = transform;
            var rotation = transform1.rotation;
            Quaternion targetRotation = Quaternion.FromToRotation(transform1.up, hit.normal) * rotation;

            // smoothly rotate the object towards the target rotation
            rotation = Quaternion.Lerp(rotation, targetRotation, rotateToGroundSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
}