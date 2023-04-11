using UnityEngine;
using UnityEngine.Events;

public class CheckPointChecker : MonoBehaviour
{

    private CheckPointList _checkPointList;

    private int _currentCheckpoint;


    [SerializeField] private UnityEvent reachedNextCheckpoint;
    [SerializeField] private UnityEvent collidedWithWall;


    
    private void Awake()
    {
        _checkPointList = FindObjectOfType<CheckPointList>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            collidedWithWall.Invoke();
            return;
        }

        if (!other.CompareTag("Checkpoint"))
            return;

        if (other.transform != _checkPointList.checkpoints[_currentCheckpoint % (_checkPointList.checkpoints.Count- 1)] )
        {
            //Debug.Log("WENT TO WRONG CHECKPOINT");
            return;
        }

        ReachedNextCheckpoint();

    }

    private void ReachedNextCheckpoint()
    {
        _currentCheckpoint++;
        //Debug.Log("+1");
        reachedNextCheckpoint.Invoke();
    }

    public Transform GetNextCheckpoint()
    {
        return _checkPointList.checkpoints[_currentCheckpoint % (_checkPointList.checkpoints.Count - 1)];
    }
}
