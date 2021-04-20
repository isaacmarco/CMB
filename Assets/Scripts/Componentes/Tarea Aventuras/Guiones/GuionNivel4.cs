using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel4 : GuionAventura
{
    
    /*
        Encontrar los tesoros y esquivar serpientes (explicación serpientes) 
        “Encuentra todos los tesoros” - “Ten cuidado con las serpientes”

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
