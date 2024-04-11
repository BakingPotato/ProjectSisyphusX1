using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject fuel;
    [SerializeField] TMP_Text deathsCounter;
    [SerializeField] TMP_Text levelCounter;

    public bool partidaEX = false;
    bool objectDestroyed = false;
    bool pause = false;

    TimerController timer;


    int actualScene;
    int deathNumber = 0;


    // Start is called before the first frame update
    void Awake()
    {
        int numGameControllers = FindObjectsOfType<GameController>().Length;
        int actualScene = SceneManager.GetActiveScene().buildIndex;
        timer = GetComponent<TimerController>();

        if (numGameControllers > 1)
        {
            Destroy(gameObject);
            objectDestroyed = true;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        if ((actualScene == SceneManager.GetSceneByName("1. Despegue").buildIndex || actualScene == SceneManager.GetSceneByName("1. Despegue EX").buildIndex) && !objectDestroyed)
        {
             timer.BeginTimer();
        }

    }

    private void Start()
    {
        if (partidaEX)
        {
            Physics.gravity = new Vector3(0f, 7f, 0f);
        }
        else
        {
            Physics.gravity = new Vector3(0f, -7f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResetRun();
        PauseGame();
    }

    private void OnLevelWasLoaded(int level)
    {
        //Este if esta aquí porque si no, ejecuta esto después de haber sido destruido y peta
        if (!objectDestroyed)
        {
            int actualScene = SceneManager.GetActiveScene().buildIndex;

            if (actualScene == SceneManager.GetSceneByName("18. La superficie").buildIndex || actualScene == SceneManager.GetSceneByName("17. La superficie EX").buildIndex)
                EndingSequence(actualScene);
            else
            {
                UpdateCounterText(actualScene);
            }

        }
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                pauseMenu.SetActive(false);
                if (fuel != null)
                    fuel.SetActive(true);
                if (deathsCounter != null)
                    deathsCounter.SetText("");
                Time.timeScale = 1.0f;
                pause = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                if (fuel != null)
                    fuel.SetActive(false);
                if (deathsCounter != null)
                    deathsCounter.SetText("Deaths: " + deathNumber);
                Time.timeScale = 0.0f;
                pause = true;

            }
        }
    }

    private void ResetRun()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 1.0f;
            Destroy(GameObject.Find("GameController"));
            Destroy(GameObject.Find("MusicController"));
            Destroy(GameObject.Find("Canvas"));
            if (partidaEX)
            {
                SceneManager.LoadScene("1. Despegue EX");
            }
            else
            {
                SceneManager.LoadScene("1. Despegue");
            }
        }
    }

    public void backToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }


    private void EndingSequence(int actualScene)
    {
        //Paramos el temporizador en el nivel final y desbloqueamos la partida EX
        if (actualScene == SceneManager.GetSceneByName("18. La superficie").buildIndex)
        {
            timer.StopTimer(false);
        }
        else if (actualScene == SceneManager.GetSceneByName("17. La superficie EX").buildIndex)
        {
            timer.StopTimer(true);
        }
        if (deathsCounter != null)
            deathsCounter.SetText("Deaths: " + deathNumber);
        if (fuel != null)
            fuel.SetActive(false);
        PlayerPrefs.SetInt("EXunlocked", 1);
        levelCounter.enabled = false;
    }

    private void UpdateCounterText(int actualScene)
    {
        if (levelCounter != null)
            if (partidaEX)
            {
                levelCounter.SetText(actualScene - 18 + "/16");
            }
            else
            {
                levelCounter.SetText(actualScene + "/17");
            }
    }

    public void CountDeath()
    {
        if(deathNumber < 9999)
            deathNumber++;
    }

}
