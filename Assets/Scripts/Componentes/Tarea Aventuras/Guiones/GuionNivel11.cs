using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel11 : GuionAventura
{
    
    /*
        encontrar una gema de cada tipo
    */

    public override void ComprobacionesGuion()
    {
        int gemas = 0; 
        
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(
                item == ObjetosAventuras.Diamante || 
                item == ObjetosAventuras.Rubi ||
                item == ObjetosAventuras.Esmeralda
            )
                gemas++;
            
        }
        if(gemas > 2)
            Fin();
       
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra una esmeralda, un rubí y un diamante"
            ,0,null,Mensaje.TipoMensaje.Bonus)
        );
        
        
        
    }
   


}
