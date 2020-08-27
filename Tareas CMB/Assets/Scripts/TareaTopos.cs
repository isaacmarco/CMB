using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaTopos : Tarea
{
    // lista de huecos donde puede aparecer el topo     
    public GameObject[] huecos; 
    // lista de disponibilidad de huecos
    private bool[] huecosDisponibles; 

    protected override void Inicio()
    {
    }

    // aparece un topo nuevo 
    private void NuevoTopo()
    {
        // obtener hueco al azar
        int indiceHueco = Random.Range(0, huecos.Length);
        // comprobar si esta ocupado
        if (!huecosDisponibles[indiceHueco])
        {
            // mostramos el nuevo topo
            huecos[indiceHueco].SetActive(true);
        } else {
            // no aparece el topo 
        }
    }

    // llamado por la interfaz de usuario cuando
    // miras un hueco 
    public void GolpearHueco(int indiceHueco)
    {}

    // se golpea el topo de ese hueco 
    private void GolpearTopo(Topo topo)
    {}

}
