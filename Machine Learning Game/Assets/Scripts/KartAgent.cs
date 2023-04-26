using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

public class KartAgent : Agent
{
    [SerializeField] private CheckPointChecker checkPointChecker;
    private CarController _carController;
    
    private Vector3 _startPos;
    private Quaternion _startRotation;

    [SerializeField] private float passCheckPointReward = 1f;
    [SerializeField] private float moveTowardsCheckPoint = 0.02f;
    [SerializeField] private float speedReward = 0.02f;
    [SerializeField] private float hitWallPenalty = -1f;
    [SerializeField] private float stayWallPenalty = -0.1f;
    [SerializeField] private float wentToWrongCheckpointPenalty = -1f;

    private float _lastDistanceToCheckPoint;

    [SerializeField] private UnityEvent reset;
    
    // Start is called before the first frame update
    void Awake()
    {
        _carController = GetComponent<CarController>();

        var transform1 = _carController.sphere.transform;
        _startPos = transform1.position;
        _startRotation = transform.rotation;
        
    }

    public override void OnEpisodeBegin()
    {
        var transform1 = _carController.sphere.transform;
        transform1.position = _startPos;
        transform.rotation = _startRotation;
        reset.Invoke();
    }

    
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckpointForward = checkPointChecker.GetNextCheckpoint().forward;
        
        sensor.AddObservation(Vector3.Dot(transform.forward, nextCheckpointForward));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Vertical"); //Acceleration   
        action[1] = Input.GetAxis("Horizontal"); //Steering
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.ContinuousActions;
        
        _carController.verticalInput = action[0];
        _carController.horizontalInput = action[1];

    }

    public void GotToNextCheckpoint()
    {
        //Debug.Log("Reached checkpoint");

        AddReward(passCheckPointReward + speedReward * _carController.sphere.velocity.magnitude);
    }
    
    public void CollidedWithWall()
    {
        //Debug.Log("Collided with wall");
        AddReward(hitWallPenalty);
    }

    public void StayedInWall()
    {
        AddReward(stayWallPenalty);
    }
    
    public void WentToWrongCheckpoint()
    {
        //Debug.Log("WRONG checkpoint");

        AddReward(wentToWrongCheckpointPenalty);
    }

    public void SetToInterfere()
    {
        GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.InferenceOnly;
    }
}
