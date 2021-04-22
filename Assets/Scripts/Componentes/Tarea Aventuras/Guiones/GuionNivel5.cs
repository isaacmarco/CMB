using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel5 : GuionAventura
{
    
    /*
        Encontrar la salida - 
        “Recoge y usa las pócimas rojas para curarte”


    */

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Tienes que encontrar la entrada a la cripta",
            0,null,Mensaje.TipoMensaje.Puerta)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Recoge y usa las pócimas rojas para curarte",
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
