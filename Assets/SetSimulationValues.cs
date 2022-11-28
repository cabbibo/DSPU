using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSimulationValues : MonoBehaviour
{

    public float _Frequency = 440;
    public AudioLife life;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        life.shader.SetFloat("_Frequency",_Frequency);
    }
}
