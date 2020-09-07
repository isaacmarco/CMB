using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class TareaTopos : Tarea
{
    public int contadorTopos, aciertos, errores; 

    private GazeAware gazeAware;

    // devuelve el tiempo que el topo es visible al salir
    public float TiempoExposicionTopo
    {
        get{return this.tiempoExposicionTopo; }
    }

    public void Acierto()
    {
        aciertos++;
    }
    public void Error()
    {
        errores++;
    }
/*
    void Awake()
    {
        gazeAware = GetComponent<GazeAware>();
    }*/
    
    void Update()
    {
        /*
        if(gazeAware!=null)
        {
            GameObject focusedObject = TobiiAPI.GetFocusedObject();
            if(focusedObject!=null)
                Debug.Log(focusedObject.name);
        }*/
    }

    // lista topos
    public Topo[] topos;
    // tiempo entre salidas del topo
    private float tiempoParaNuevoTopo = 3f; 
    // tiempo durante el que el topo es visible
    private float tiempoExposicionTopo = 3f;

    protected override void Inicio()
    {
        // inciar cortuina de la partida 
        StartCoroutine(CorutinaPartida());
    }

    private IEnumerator CorutinaPartida()
    {
        Debug.Log("Partida en curso");

        // tiempo de espera inicial
        yield return new WaitForSeconds(1f);

        // comienzo del game loop
        while(true)
        {
            // generar un nuevo topo
            NuevoTopo();
            // esperar un tiempo antes de mostrar otro
            yield return new WaitForSeconds(tiempoParaNuevoTopo);

        }
    }

    // aparece un topo nuevo 
    private void NuevoTopo()
    {
        Debug.Log("Nuevo topo");

        // obtener topo al azar
        int indiceTopo = Random.Range(0, topos.Length);
        if(topos[indiceTopo].Escondido)
        {
            topos[indiceTopo].SalirExterior();
            contadorTopos++;
        }
       
    }

   

}
