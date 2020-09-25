using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarjetaTareaMemory : MonoBehaviour
{
  
    private EstimulosTareaMemory estimulo; 
    public EstimulosTareaMemory Estimulo {
        get { return estimulo;}
    }

    public void AsignarEstimulo(EstimulosTareaMemory estimulo)
    {
        this.estimulo = estimulo; 
    }

}
