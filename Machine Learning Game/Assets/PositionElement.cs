using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PositionElement : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private TMP_Text text;
    
    
    
    public void Show(string message, Color color)
    {
        text.text = message;
        color.a = 0.95f;
        text.color = color;
    }
    
    public void SetBackgroundColour(Color colour)
    {
        background.color = colour;
    }
    
    
}
