using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartGame : MonoBehaviour
{
    public List<KartAgent> agents;
    
    public TMP_Text startText;

    public UnityEvent startGame;

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }
    
    private IEnumerator Countdown()
    {
        GetComponent<AudioSource>().Play();
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(1);
        startText.text = "GO!";
        StartAgents();
        yield return new WaitForSeconds(1);
        startText.text = "";
    }
    
    public void StartAgents()
    {
        foreach (var agent in agents)
        {
            agent.enabled = true;
        }
        startGame.Invoke();
    }
}
