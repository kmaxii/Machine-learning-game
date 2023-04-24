using EveryProject.Scriptable_objects;
using UnityEngine;

public class Endscreen : MonoBehaviour
{

    [SerializeField] private CheckPointList checkPointList;
    
    [SerializeField] private PositionElement positionElement;
    [SerializeField] private GameObject restartButton;
    
    public void ShowEndScreen()
    {
        for (var i = 0; i < checkPointList.finalposses.Count; i++)
        {
            PositionElement spawned = Instantiate(positionElement, transform);
            
            spawned.Show((i + 1) + ". " + checkPointList.finalposses[i].name, checkPointList.finalposses[i].Color);
            
            if (checkPointList.finalposses[i].transform.name == "player")
            {
                spawned.SetBackgroundColour(Color.yellow);
            }
        }
        
        Instantiate(restartButton, transform);
    } 
}
