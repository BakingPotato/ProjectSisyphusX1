using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RenderHolder : MonoBehaviour
{

    void Awake()
    {
        int numTimerHolders = FindObjectsOfType<RenderHolder>().Length;
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
