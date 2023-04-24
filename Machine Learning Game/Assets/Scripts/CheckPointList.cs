using System;
using System.Collections.Generic;
using EveryProject.Scriptable_objects;
using UnityEngine;

public class CheckPointList : MonoBehaviour
{
    [SerializeField] public int laps = 3;


    [HideInInspector] public List<Transform> checkpoints;
    [HideInInspector] public List<Transform> finalposses = new List<Transform>();


    private void Awake()
    {
        checkpoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints.Add(transform.GetChild(i));
        }
    }
}