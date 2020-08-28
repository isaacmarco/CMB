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
    private bool golpeado = false; 
    private TareaTopos tarea; 
    

    public void SalirExterior()
    {
        StartCoroutine(CorutinaSalirExterior() );
    }


    public void Golpedo()
    {
        // feedback al recibir el golpe
        if(!golpeado)
        {
            iTween.ShakeScale(gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.6f);
            golpeado = true; 
        }
    }

    void Start()
    {
        // crear referencias
        tarea = FindObjectOfType<TareaTopos>();
    }

    private IEnumerator CorutinaSalirExterior()
    {
        

        Vector3 posicion = gameObject.transform.position; 
        float tiempoAnimacion = 0.5f; 
        float alturaObjetivo = 1f; 

        // animacion de salida al exterior        
        iTween.MoveTo(gameObject, new Vector3(posicion.x, alturaObjetivo, posicion.z), tiempoAnimacion);

        // actualizamos el estado del topo
        escondido = false; 
        golpeado = false; 

        // esperamos el tiempo de exposicion
        yield return new WaitForSeconds(3f); //tarea.TiempoExposicionTopo);


        // empezamos la animacion de vuelta a la tierra
        iTween.MoveTo(gameObject, new Vector3(posicion.x, 0, posicion.z), tiempoAnimacion);

        // actualizamos el estado del topo
        escondido = true; 

        yield return null; 
    }


    
}
