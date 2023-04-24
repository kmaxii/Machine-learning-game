using System;
using EveryProject.Scriptable_objects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CheckPointChecker : MonoBehaviour
{

    private CheckPointList _checkPointList;

     public int currentCheckpoint;


    [SerializeField] private Drivers drivers;

    [SerializeField] public Color Color;
    
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
        currentCheckpoint = 0;
    }
    
    private void Awake()
    {
        _checkPointList = FindObjectOfType<CheckPointList>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Checkpoint"))
            return;

        if (other.transform != _checkPointList.checkpoints[currentCheckpoint % (_checkPointList.checkpoints.Count- 1)] )
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
        currentCheckpoint++;
        reachedNextCheckpoint.Invoke();

        if (currentCheckpoint % (_checkPointList.checkpoints.Count - 1) == 0)
        {
            lapped.Invoke();
            currentLaps++;
            if (currentLaps == _checkPointList.laps)
            {
                _checkPointList.finalposses.Add(this);
                finishedRace.Invoke();
            }
        }
    }

    public Transform GetNextCheckpoint()
    {
        return _checkPointList.checkpoints[currentCheckpoint % (_checkPointList.checkpoints.Count - 1)];
    }
}
