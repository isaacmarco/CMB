using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel6 : GuionAventura
{
    
    /*
        Encontrar tesoros dentro de la cripta “Encuentra los tesoros” - “Cuidado con los murciélagos”


    */

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra los 3 rubíes",
            0,null,Mensaje.TipoMensaje.Bonus)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "¡Cuidado con los murciélagos",
            0,null,Mensaje.TipoMensaje.Fallo)
        );
        
    }
    
 
    public override void ComprobacionesGuion()
    {
        int rubies = 0;         
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Rubi)
                rubies++;
          
        }
        if(rubies > 2)
            Fin();
    }

 

    
}
