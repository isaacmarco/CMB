using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel13 : GuionAventura
{
 

    public override void ComprobacionesGuion()
    {
        bool libroEncontrado = false; 
        for(int i=0; i<tarea.objetosRecogidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosRecogidos[i];
            if(item == ObjetosAventuras.Libro)
                libroEncontrado = true;
        }

        if(libroEncontrado)
            Fin();
    }

     
    protected override IEnumerator Mensajes()
    {
         yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra el libro sagrado"
            ,0,null,Mensaje.TipoMensaje.Libro)
        );
        
        
        
    }
   


}
