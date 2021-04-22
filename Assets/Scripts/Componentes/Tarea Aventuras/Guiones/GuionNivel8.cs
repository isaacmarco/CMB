using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel8 : GuionAventura
{
    
    /*
        Encuentra la llave para salir de la cripta “Encuentra la llave para salir de la cripta”



    */

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra la llave para salir del templo",
            0,null,Mensaje.TipoMensaje.Llave)
        );
        /*
        yield return StartCoroutine(tarea.MostrarMensaje(
            "¡Cuidado con los murciélagos",
            0,null,Mensaje.TipoMensaje.Topos)
        );*/
        
    }
    
 
    public override void ComprobacionesGuion()
    {
        bool llaveEncontrada = false; 
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];            
            if(item == ObjetosAventuras.Llave)
            {
                // abandonamos el bucle al encontrar la llave
                llaveEncontrada = true;
                break;
            } 
          
        }

        if(llaveEncontrada)
            Fin();
    }

 

    
}
