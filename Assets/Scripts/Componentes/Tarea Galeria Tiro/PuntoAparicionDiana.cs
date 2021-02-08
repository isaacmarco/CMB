using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoAparicionDiana : MonoBehaviour
{
    public MovimientoDiana direccionMovimientoDiana; 
    private bool enUso; 
    public bool EnUso{
        get { return enUso; }
    }
    
    public void Usar()
    {        
        enUso = true; 
    }
    public void Liberar()
    {      
        enUso = false; 
    }
    void Awake()
    {
        // ocultamos el mesh
        gameObject.GetComponent<MeshRenderer>().enabled = false; 
    }

}
