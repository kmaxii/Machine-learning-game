using UnityEngine;

public class CheckPointChecker : MonoBehaviour
{

    private CheckPointList _checkPointList;

    private int _currentCheckpoint;

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
            Debug.Log("WENT TO WRONG CHECKPOINT");
            return;
        }

        _currentCheckpoint++;
        Debug.Log("+1");


    }
}
