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
            "Tienes que encontrar las 4 esmeraldas",
            0,null,Mensaje.TipoMensaje.Bonus)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "¡Ten cuidado con las serpientes!",
            0,null,Mensaje.TipoMensaje.Serpientes)
        );
        
    }
    
    public override void ComprobacionesGuion()
    {
        int esmeraldas = 0;         
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Esmeralda)
                esmeraldas++;
          
        }
        if(esmeraldas > 3)
            Fin();
    }


    
}
