using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MenuController : MonoBehaviour
{
    public TMP_Text bestTimeText;
    public TMP_Text bestTimeEXText;
    public GameObject partidaEXUI;

    public GameObject menuCanvas;
    public GameObject infoCanvas;

    [SerializeField] float skyboxRotation = 0.35f;

    float bestTimeSeconds;
    float bestTimeEXSeconds;

    private void Awake()
    {

        //InitialSaveState();

        cleanPreviousGame();
    }

    private static void InitialSaveState()
    {
        PlayerPrefs.DeleteKey("EXunlocked");
        PlayerPrefs.DeleteKey("bestTime");
        PlayerPrefs.DeleteKey("bestTimeEX");
    }

    private static void cleanPreviousGame()
    {
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("MusicController"));
        Destroy(GameObject.Find("Canvas"));
    }

    // Start is called before the first frame update
    void Start()
    {
        updateBestTimes();

        isEXUnlocked();
    }

    private void updateBestTimes()
    {
        bestTimeSeconds = PlayerPrefs.GetFloat("bestTime", -1);
        bestTimeEXSeconds = PlayerPrefs.GetFloat("bestTimeEX", -1);

        if (bestTimeSeconds == -1)
        {
            bestTimeText.text = "Best Time: 00:00:00";
        }
        else
        {
            TimeSpan bestTime = TimeSpan.FromSeconds(bestTimeSeconds);
            bestTimeText.text = "Best Time: " + bestTime.ToString("mm':'ss'.'ff");
        }

        if (bestTimeEXSeconds == -1)
        {
            bestTimeEXText.text = "Best Time: 00:00:00";
        }
        else
        {
            TimeSpan bestTimeEX = TimeSpan.FromSeconds(bestTimeEXSeconds);
            bestTimeEXText.text = "Best Time: " + bestTimeEX.ToString("mm':'ss'.'ff");
        }
    }

    private void isEXUnlocked()
    {
        if (PlayerPrefs.GetInt("EXunlocked", 0) == 1)
        {
            unlockEX();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RespondToDebugKeys();
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotation);
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.X))
        {
            PlayerPrefs.SetInt("EXunlocked", 1);
            unlockEX();
        }
    }

    public void unlockEX()
    {
        partidaEXUI.SetActive(true);
        bestTimeEXText.enabled = true;
    }

    public void empezarPartida()
    {
        SceneManager.LoadScene("1. Despegue");
    }
    
    public void empezarPartidaEX()
    {
        SceneManager.LoadScene("1. Despegue EX");
    }

    public void mostrarControles()
    {
        menuCanvas.SetActive(false);
        infoCanvas.SetActive(true);
    }

    public void ocultarControles()
    {
        menuCanvas.SetActive(true);
        infoCanvas.SetActive(false);
    }
}
