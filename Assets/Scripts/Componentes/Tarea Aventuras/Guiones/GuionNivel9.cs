using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel9 : GuionAventura
{
    
    /*
    Encontrar el refugio por el nivel (sin inventario) pero con ítems 
    que recoger que te den punto (monedas). “Encuentra el refugio y recoge los tesoros”
    */

    private bool aguaEncontrada = false; 
    private bool mensajeRefugio = false; 

    public override void SalidaAlcanzada()
    {
        // conseguido
        if(!tarea.TareaBloqueada)     
        {   
            if(aguaEncontrada)
            {
                tarea.GanarPartida();
            } else {
                // si llegamos al refugio pero no tenemos agua
                StartCoroutine(tarea.MostrarMensaje(
                    "¡Tienes que beber mas agua!"
                ,0,null,Mensaje.TipoMensaje.Refugio)
                );
            }
        }
    }
    
    

    public override void ComprobacionesGuion()
    {
        bool todoEsAgua = true; 
        for(int i=0; i<tarea.objetosUsados.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosUsados[i];
            if(item != ObjetosAventuras.Agua)
                todoEsAgua = false; 

        }
        if(todoEsAgua && tarea.objetosUsados.Count >= 3)
        {
            aguaEncontrada = true; 
            if(!mensajeRefugio)
            {
                mensajeRefugio = true; 
                StartCoroutine(tarea.MostrarMensaje(
                    "Ahora encuentra el refugio"
                ,0,null,Mensaje.TipoMensaje.Refugio)
                );
            }
        }
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Tienes que encontrar el campamento"
            ,0,null,Mensaje.TipoMensaje.Refugio)
        );
         yield return StartCoroutine(tarea.MostrarMensaje(
            "Recoge y bebe 3 botellas de agua"
            ,0,null,Mensaje.TipoMensaje.Ojo)
        );
        
    }
   


}
