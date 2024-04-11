using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 Angle;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Angle * Time.deltaTime);
    }
}
