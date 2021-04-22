using System.Collections;
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
            "¡La pradera está en llamas, debes volver al campamento!",
            0,null,Mensaje.TipoMensaje.Fuego)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Puedes curarte recogiendo corazones",
            0,null,Mensaje.TipoMensaje.Corazon)
        );
        
    }
    
    public override void SalidaAlcanzada()
    {
        // conseguido
        if(!tarea.TareaBloqueada)        
            tarea.GanarPartida();
    }
    
}
