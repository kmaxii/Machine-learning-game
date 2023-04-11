using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KartAgent : Agent
{
    [SerializeField] private CheckPointChecker checkPointChecker;
    private CarController _carController;
    
    private Vector3 _startPos;
    private Quaternion _startRotation;
    // Start is called before the first frame update
    void Awake()
    {
        _carController = GetComponent<CarController>();

        var transform1 = transform;
        _startPos = transform1.position;
        _startRotation = transform1.rotation;
    }

    public override void OnEpisodeBegin()
    {
        var transform1 = transform;
        transform1.position = _startPos;
        transform1.rotation = _startRotation;
    }

    
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((checkPointChecker.GetNextCheckpoint().position - transform.position).normalized);
        
        AddReward(-0.1f * Time.deltaTime);
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
        AddReward(3f);
    }
    
    public void CollidedWithWall()
    {
        AddReward(-1f);
    }
}
