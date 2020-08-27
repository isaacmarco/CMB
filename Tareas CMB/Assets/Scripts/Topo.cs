using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topo : MonoBehaviour
{    
    public bool Escondido {
        get{ return this.escondido; }
    }
    // un topo solo puede ser golpeado si no esta escondido 
    private bool escondido = true; 
    private TareaTopos tarea; 

    public void SalirExterior()
    {
        StartCoroutine(CorutinaSalirExterior() );
    }


    public void Golpedo()
    {
        
    }

    void Start()
    {
        // crear referencias
        tarea = FindObjectOfType<TareaTopos>();
    }

    private IEnumerator CorutinaSalirExterior()
    {
        // animacion de salida al exterior

        // actualizamos el estado del topo
        escondido = false; 

        // esperamos el tiempo de exposicion
        yield return new WaitForSeconds(tarea.TiempoExposicionTopo);

        // empezamos la animacion de vuelta a la tierra

        // actualizamos el estado del topo
        escondido = true; 

        yield return null; 
    }


    
}
