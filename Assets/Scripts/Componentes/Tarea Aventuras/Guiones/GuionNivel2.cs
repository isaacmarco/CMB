using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel2 : GuionAventura
{
     
    /*
        Encontrar 3 troncos y el hacha “Encuentra suficiente leña y 
        un hacha para hacer un campamento”
    */

    public override void ComprobacionesGuion()
    {
        int troncos = 0; 
        bool hachaEncontrada = false; 
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Tronco)
                troncos++;
            if(item == ObjetosAventuras.Espada)
                hachaEncontrada = true; 

        }
        if(hachaEncontrada && troncos > 2)
            Fin();
    }

      
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Busca 3 troncos de leña y una espada para cortar"
            ,0,null,Mensaje.TipoMensaje.Troncos)
        );
      
        
    }
}
