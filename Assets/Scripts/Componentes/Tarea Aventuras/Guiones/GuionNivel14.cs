using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel14 : GuionAventura
{
 
    private bool rubiEncontrado = false; 
    private bool mensajeRefugio = false; 

    
    public override void SalidaAlcanzada()
    {
        // conseguido
        if(!tarea.TareaBloqueada)     
        {   
            if(rubiEncontrado)
            {
                tarea.GanarPartida();
            } else {
                // si llegamos al refugio pero no tenemos agua
                StartCoroutine(tarea.MostrarMensaje(
                    "¡Tienes que encontrar el gran rubí!"
                ,0,null,Mensaje.TipoMensaje.Bonus)
                );
            }
        }
    }
    

    public override void ComprobacionesGuion()
    {
        
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Rubi)
                rubiEncontrado = true;
        }

        
        if(rubiEncontrado)
        {            
            if(!mensajeRefugio)
            {
                mensajeRefugio = true; 
                StartCoroutine(tarea.MostrarMensaje(
                    "Ahora encuentra la salida"
                ,0,null,Mensaje.TipoMensaje.Puerta)
                );
            }
        }
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra el gran rubí"
            ,0,null,Mensaje.TipoMensaje.Bonus)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra la salida"
            ,0,null,Mensaje.TipoMensaje.Puerta)
        );
        
        
        
    }
   


}
