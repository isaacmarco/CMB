using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel10 : GuionAventura
{
    
    /*
        Encontrar tesoros “Encuentra los tesoros” - 
        “Cuidado con los esqueletos, si te ven te perseguirán”
    */

    public override void ComprobacionesGuion()
    {
        int diamantes = 0; 
        
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Diamante)
                diamantes++;
            
        }
        if(diamantes > 2)
            Fin();
       
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra los 3 diamantes"
            ,0,null,Mensaje.TipoMensaje.Bonus)
        );
         yield return StartCoroutine(tarea.MostrarMensaje(
            "¡Cuidado con los esqueletos!"
            ,0,null,Mensaje.TipoMensaje.Esqueletos)
        );
        
    }
   


}
