using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel1 : GuionAventura
{
    /*
    Encontrar 4 frutas para comer y comerlas desde el inventario. 
    “Algunas cosas que recojas van a tus bolsillos para usarlas cuando quieras” - 
    “Recoge 4 frutas y cómetelas”
    */


    public override void ComprobacionesGuion()
    {
        bool todosSonFrutas = true; 
        for(int i=0; i<tarea.objetosConsumidos.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosConsumidos[i];
            if(item != ObjetosAventuras.Manzana)
                todosSonFrutas = false; 

        }
        if(todosSonFrutas && tarea.objetosConsumidos.Count >= 4)
            Fin();
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Busca 4 frutas para alimentarte"
            ,0,null,Mensaje.TipoMensaje.Frutas)
        );
         yield return StartCoroutine(tarea.MostrarMensaje(
            "Puedes comer la fruta después de recogerla"
            ,0,null,Mensaje.TipoMensaje.Ojo)
        );
        
    }

}
