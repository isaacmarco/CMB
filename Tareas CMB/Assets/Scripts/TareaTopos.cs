using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaTopos : Tarea
{
    // devuelve el tiempo que el topo es visible al salir
    public float TiempoExposicionTopo
    {
        get{return this.tiempoExposicionTopo; }
    }

    // lista topos
    public Topo[] topos;
    // tiempo entre salidas del topo
    private float tiempoParaNuevoTopo = 2f; 
    // tiempo durante el que el topo es visible
    private float tiempoExposicionTopo = 3f;

    protected override void Inicio()
    {
        StartCoroutine(CorutinaPartida());
    }

    private IEnumerator CorutinaPartida()
    {
        while(true)
        {
            NuevoTopo();
            yield return new WaitForSeconds(tiempoParaNuevoTopo);

        }
        yield return null; 
    }

    // aparece un topo nuevo 
    private void NuevoTopo()
    {
        // obtener topo al azar
        int indiceTopo = Random.Range(0, topos.Length);
       
    }

   

}
