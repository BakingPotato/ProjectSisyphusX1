using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    const float tau = Mathf.PI * 2; //6.283
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) { return; } //Comparar float a 0 es peligroso, mejor a epsilon que es el float más pequeño

        float cycles = Time.time / period; // Crece continuamente
        float rawSinWave = Mathf.Sin(cycles * tau); // Yendo de -1 a 1

        movementFactor = (rawSinWave + 1f) / 2f; // Recalculado para que vaya de 0 a 1 así que es más fácil de usar

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
