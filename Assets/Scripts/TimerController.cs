using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public TMP_Text timeCounter;

    private TimeSpan timePlaying;
    private bool timerGoing = false;

    private float elapsedTime;


    private void Start()
    {
        //timeCounter.text = "00:00:00";
        //timerGoing = false;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void ResumeTimer()
    {
        timerGoing = true;
        StartCoroutine(UpdateTimer());
    }

    public void PauseTimer()
    {
        timerGoing = false;
        StopCoroutine(UpdateTimer());
    }

    public void StopTimer(bool EX)
    {
        timerGoing = false;
        float actualTime = UpdateTimerText();
        StopCoroutine(UpdateTimer());

        //Comprobar si hay que actualizar tiempos de partida EX o normal
        if (!EX)
        {
            float bestTimeSeconds = PlayerPrefs.GetFloat("bestTime", Mathf.Infinity);
            if (bestTimeSeconds > actualTime)
            {
                PlayerPrefs.SetFloat("bestTime", actualTime);
            }
        }
        else
        {
            float bestTimeEXSeconds = PlayerPrefs.GetFloat("bestTimeEX", Mathf.Infinity);
            if (bestTimeEXSeconds > actualTime)
            {
                PlayerPrefs.SetFloat("bestTimeEX", actualTime);
            }
        }

    }

    public void Reset()
    {
        elapsedTime = 0f;
    }

    public IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            UpdateTimerText();

            yield return null;
        }
    }

    private float UpdateTimerText()
    {
        elapsedTime += Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        string timePlayingStr = timePlaying.ToString("mm':'ss'.'ff");
        if(timeCounter!= null)
            timeCounter.SetText(timePlayingStr);
        return elapsedTime;
    }

}
