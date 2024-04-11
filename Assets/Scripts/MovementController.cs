using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    Rigidbody rb;
    RigidbodyConstraints originalConstraints;
    StopFuelBar sfb;


    //Parametros
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 800f;

    [SerializeField] float stopFuel = 100;
    [SerializeField] float fuelDepletion = 0.75f;


    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    [SerializeField] ParticleSystem mainStopParticles;
    [SerializeField] ParticleSystem leftStopParticles;
    [SerializeField] ParticleSystem rightStopParticles;

    [SerializeField] Light backLight;

    AudioSource mainEngine;
    AudioSource stopEngine;

    bool depleteFuel = false;
    //Cache

    //Estado


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalConstraints = rb.constraints;
        sfb = FindObjectOfType<StopFuelBar>();
        mainEngine = mainEngineParticles.GetComponent<AudioSource>();
        stopEngine = mainStopParticles.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessStop();
        ProcessRotation();
        UpdateFuelBar();
    }

    private void UpdateFuelBar()
    {
        if (sfb == null) sfb = FindObjectOfType<StopFuelBar>(); ;
        if (sfb == null) return;
        sfb.GetComponent<TMP_Text>().SetText("Fuel: " + Math.Round(stopFuel));
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessStop()
    {
        if(stopFuel > 0)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                StartStopping();
                depleteFuelRoutine();
            }
            else if (!Input.GetKey(KeyCode.Mouse0))
            {
                StopStopping();
            }
        }

    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            RotateStop();
        }
    }

    private void StartThrusting()
    {
        rb.constraints = originalConstraints;
        //addRelative añade fuerza dependiendo del sistema de coordenadas del objeto
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        PlayAudio(mainEngine);
        if (!mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
        CeaseStopParticles();
        backLight.enabled = true;
    }

    public void EternalThrusting()
    {
        //addRelative añade fuerza dependiendo del sistema de coordenadas del objeto
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    }

    private void StartStopping()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | originalConstraints;
        PlayAudio(stopEngine);
        depleteFuel = true;

        if (!leftStopParticles.isPlaying)
            leftStopParticles.Play();
        if (!rightStopParticles.isPlaying)
            rightStopParticles.Play();
        if (!mainStopParticles.isPlaying)
            mainStopParticles.Play();
        backLight.enabled = true;
    }

    private void StopThrusting()
    {
        mainEngine.Stop();
        mainEngineParticles.Stop();
        backLight.enabled = false;
    }

    private void StopStopping()
    {
        depleteFuel = false;
        stopEngine.Stop();
        rb.constraints = originalConstraints;
        CeaseStopParticles();
    }

    void depleteFuelRoutine()
    {
         stopFuel -= fuelDepletion * Time.deltaTime;

        if(stopFuel <= 0)
        {
            StopStopping();
        }
    }

    private void CeaseStopParticles()
    {
        mainStopParticles.Stop();
        rightStopParticles.Stop();
        leftStopParticles.Stop();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying)
            leftThrusterParticles.Play();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying)
            rightThrusterParticles.Play();
    }

    private void RotateStop()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }


    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Congela la rotación para que podamos rotar manualmente
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        // Descongela las rotaxiones x e y asi como la posicion z 
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }
    private void PlayAudio(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

}
