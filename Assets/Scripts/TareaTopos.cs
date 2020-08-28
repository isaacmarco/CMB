﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class TareaTopos : Tarea
{
    private GazeAware gazeAware;

    // devuelve el tiempo que el topo es visible al salir
    public float TiempoExposicionTopo
    {
        get{return this.tiempoExposicionTopo; }
    }

    void Awake()
    {
        gazeAware = GetComponent<GazeAware>();
    }
    void Update()
    {
        if(gazeAware!=null)
        {
            GameObject focusedObject = TobiiAPI.GetFocusedObject();
            if(focusedObject!=null)
                Debug.Log(focusedObject.name);
        }
    }

    void Start()
    {
        Debug.Log("INICIO");
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
        yield return null; 
    }

    // aparece un topo nuevo 
    private void NuevoTopo()
    {
        // obtener topo al azar
        int indiceTopo = Random.Range(0, topos.Length);
        if(topos[indiceTopo].Escondido)
            topos[indiceTopo].SalirExterior();
       
    }

   

}
