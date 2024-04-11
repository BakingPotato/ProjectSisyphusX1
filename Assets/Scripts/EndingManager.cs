using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{

    [SerializeField] GameObject rocket;

    [SerializeField] float endingDelay = 7f;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] AudioClip mainEngine;


    void Start()
    {
        StartCoroutine(CloseAfterTime(endingDelay));
        //TimerController.instance.StopTimer();
        //GlobalVariables.EXunlocked = true;
    }

    private void Update()
    {
        rocket.GetComponent<MovementController>().EternalThrusting();
        SkipCredits();
    }

    private static void SkipCredits()
    {
        if (Input.GetKey(KeyCode.S))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator CloseAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("MainMenu");
    }
}
