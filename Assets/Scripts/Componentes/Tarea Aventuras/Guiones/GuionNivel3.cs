﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel3 : GuionAventura
{
    
    /*
        Encontrar la salida por el nivel. “La pradera está en llamas, encuentra una salida” - 
        “Puedes curarte recogiendo los corazones”
    */

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "¡La pradera está en llamas, debes encontrar una salida!",
            0,null,Mensaje.TipoMensaje.Topos)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Puedes curarte recogiendo corazones",
            0,null,Mensaje.TipoMensaje.Topos)
        );
        
    }
    
    public override void SalidaAlcanzada()
    {
        // conseguido
        if(!tarea.TareaBloqueada)        
            tarea.GanarPartida();
    }
    
}