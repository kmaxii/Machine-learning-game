using System.Collections.Generic;
using EveryProject.Scriptable_objects;
using UnityEngine;

public class Endscreen : MonoBehaviour
{

    [SerializeField] private CheckPointList checkPointList;
    
    [SerializeField] private PositionElement positionElement;
    [SerializeField] private GameObject restartButton;

    public Drivers drivers;

    private int _pos;
    public void ShowEndScreen()
    {
        
        var allDrivers = drivers.GetCopy();

        _pos = 1;
        for (var i = 0; i < checkPointList.finalposses.Count; i++)
        {
            SpawnPositionElement(checkPointList.finalposses[i]);
            allDrivers.Remove(checkPointList.finalposses[i]);
        }
        
        SpawnRemainingElements(allDrivers);
        Instantiate(restartButton, transform);
    } 
    
    private void SpawnRemainingElements(HashSet<CheckPointChecker> remainingDrivers)
    {
        List<CheckPointChecker> sortedDrivers = new List<CheckPointChecker>(remainingDrivers);
        
        sortedDrivers.Sort((a, b) => (a.currentLaps + a.currentCheckpoint * 0.01f).CompareTo(b.currentLaps + + b.currentCheckpoint * 0.01f));
        foreach (var driver in sortedDrivers)
        {
            SpawnPositionElement(driver);
        }
    }
    
    private void SpawnPositionElement(CheckPointChecker checkPointChecker)
    {
        PositionElement spawned = Instantiate(positionElement, transform);
        spawned.Show(_pos + ". " + checkPointChecker.name, checkPointChecker.Color);
        
        if (checkPointChecker.transform.name == "you")
        {
            spawned.SetBackgroundColour(Color.yellow);
        }

        _pos++;
    }
}
