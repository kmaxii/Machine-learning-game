using System;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointChecker : MonoBehaviour
{

    private CheckPointList _checkPointList;

    private int _currentCheckpoint;


    [SerializeField] private UnityEvent reachedNextCheckpoint;
    [SerializeField] private UnityEvent collidedWithWall;
    [SerializeField] private UnityEvent stayedInWall;
    [SerializeField] private UnityEvent reachedWrongCheckpoint;


    public void ResetCheckPointPos()
    {
        _currentCheckpoint = 0;
    }
    
    private void Awake()
    {
        _checkPointList = FindObjectOfType<CheckPointList>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Checkpoint"))
            return;

        if (other.transform != _checkPointList.checkpoints[_currentCheckpoint % (_checkPointList.checkpoints.Count- 1)] )
        {
            reachedWrongCheckpoint.Invoke();
            return;
        }

        ReachedNextCheckpoint();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            collidedWithWall.Invoke();
            return;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Wall"))
        {
            stayedInWall.Invoke();
        }
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
