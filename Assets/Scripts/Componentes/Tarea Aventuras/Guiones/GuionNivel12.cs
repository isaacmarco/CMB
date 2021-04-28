using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel12 : GuionAventura
{
 

    public override void ComprobacionesGuion()
    {
        int troncos = 0;       
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Tronco)
                troncos++;
          

        }
        if(troncos > 2)
            Fin();
    }

     
    protected override IEnumerator Mensajes()
    {
         yield return StartCoroutine(tarea.MostrarMensaje(
            "Recoge 3 troncos para mantener el fuego"
            ,0,null,Mensaje.TipoMensaje.Troncos)
        );
        
        
        
    }
   


}
