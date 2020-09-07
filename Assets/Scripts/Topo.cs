using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tipos 
{
    Topo, Pato, Obeja, Pinguino
}

public class Topo : MonoBehaviour
{    
    public GameObject modeloTopo, modeloPato, modeloObeja, 
    modeloPinguino; 

    public bool Escondido {
        get{ return this.escondido; }
    }

    // un topo solo puede ser golpeado si no esta escondido 
    private bool escondido = true; 
    private bool golpeado = false; 
    private TareaTopos tarea; 
    

    public void SalirExterior()
    {
        Tipos tipo = Tipos.Topo; 
        if(Random.value > 0.5f)
            tipo = (Tipos) Random.Range(0, 4);

        StartCoroutine(CorutinaSalirExterior( tipo) );
    }


    public void Golpedo()
    {
        if(escondido)
            return; 
            
        // feedback al recibir el golpe
        if(!golpeado)
        {
            Debug.Log("topo golpeado");
            iTween.ShakeScale(gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.6f);
            golpeado = true; 
            FindObjectOfType<TareaTopos>().Acierto();
        }
    }

    void Start()
    {
        // crear referencias
        //tarea = FindObjectOfType<TareaTopos>();
    }

    private void OcultarModelos()
    {
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloObeja, modeloPinguino
        };

        foreach(GameObject modelo in modelos)
            modelo.SetActive(false);
    }

    private IEnumerator CorutinaSalirExterior(Tipos tipo)
    {
        // ocultar los modelos y mostrar el adecuado 
        OcultarModelos();
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloObeja, modeloPinguino
        };
        modelos[ (int) tipo].SetActive(true);

        Vector3 posicion = gameObject.transform.position; 
        float tiempoAnimacion = 0.5f; 
        float alturaObjetivo = 0.5f; 

        // animacion de salida al exterior        
        iTween.MoveTo(gameObject, new Vector3(posicion.x, alturaObjetivo, posicion.z), tiempoAnimacion);

        // actualizamos el estado del topo
        escondido = false; 
        golpeado = false; 

        // esperamos el tiempo de exposicion
        yield return new WaitForSeconds(3f); //tarea.TiempoExposicionTopo);


        // empezamos la animacion de vuelta a la tierra
        iTween.MoveTo(gameObject, new Vector3(posicion.x, -1, posicion.z), tiempoAnimacion);

        // contabilizar error
        if(!golpeado)
            FindObjectOfType<TareaTopos>().Error();
            
        // actualizamos el estado del topo
        escondido = true; 

        yield return null; 
    }


    
}
