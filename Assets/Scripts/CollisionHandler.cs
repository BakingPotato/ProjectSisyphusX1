using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float loadLevelDelay = 1f;

    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;    
    
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    GameObject gameController;
    TimerController timer;
    GameController GC;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool isCollisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController");
        if(gameController != null)
        {
            timer = gameController.GetComponent<TimerController>();
            GC = gameController.GetComponent<GameController>();
        }
    }

    private void Update()
    {
        //RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
            if (isCollisionDisabled)
            {
                Debug.Log("Colisiones desactivadas");
            }
            else
            {
                Debug.Log("Colisiones activadas");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || isCollisionDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Debug.Log("Amigable");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        if (GC != null)
            GC.CountDeath();
        GetComponent<MovementController>().enabled = false;
        Invoke("ReloadLevel", loadLevelDelay);
    }

    void StartSuccessSequence()
    {
        //Paramos el temporizador para no añadir tiempo muerto en la carga
        if(timer != null)
           timer.PauseTimer();

        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<MovementController>().enabled = false;
        Invoke("NextLevel", loadLevelDelay);
    }

    private void ReloadLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void NextLevel()
    {

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

        //Reanudamos el temporizador al cargar el siguiente nivel
        if (timer != null)
           timer.ResumeTimer();

        //Para debugear el final
        //SceneManager.LoadScene(18);
    }
}
