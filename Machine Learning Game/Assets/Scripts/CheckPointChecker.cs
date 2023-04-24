using System;
using EveryProject.Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointChecker : MonoBehaviour
{

    private CheckPointList _checkPointList;

    private int _currentCheckpoint;


    [SerializeField] private Drivers drivers;

    
    [SerializeField] private UnityEvent reachedNextCheckpoint;
    [SerializeField] private UnityEvent collidedWithWall;
    [SerializeField] private UnityEvent stayedInWall;
    [SerializeField] private UnityEvent reachedWrongCheckpoint;
    [SerializeField] private UnityEvent lapped;
    [SerializeField] private UnityEvent finishedRace;
    [SerializeField] private UnityEvent collidedWithDeathZone;

    [HideInInspector] public int currentLaps;

    private void OnEnable()
    {
        drivers.Add(this);
    }

    private void OnDisable()
    {
        drivers.Add(this);
    }

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

        if (other.collider.CompareTag("DeathZone"))
        {
            collidedWithDeathZone.Invoke();
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
        reachedNextCheckpoint.Invoke();

        if (_currentCheckpoint % (_checkPointList.checkpoints.Count - 1) == 0)
        {
            lapped.Invoke();
            currentLaps++;
            if (currentLaps == _checkPointList.laps)
            {
                _checkPointList.finalposses.Add(transform);
                finishedRace.Invoke();
            }
        }
    }

    public Transform GetNextCheckpoint()
    {
        return _checkPointList.checkpoints[_currentCheckpoint % (_checkPointList.checkpoints.Count - 1)];
    }
}