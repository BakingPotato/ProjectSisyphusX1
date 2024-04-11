using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHolder : MonoBehaviour
{

    void Awake()
    {
        int numTimerHolders = FindObjectsOfType<TimerHolder>().Length;
        if (numTimerHolders > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
