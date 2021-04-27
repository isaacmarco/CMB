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
        for(int i=0; i<tarea.objetosUsados.Count; i++)        
        {
            ObjetosAventuras item = (ObjetosAventuras) tarea.objetosUsados[i];
            if(item != ObjetosAventuras.Manzana)
                todosSonFrutas = false; 

        }
        if(todosSonFrutas && tarea.objetosUsados.Count >= 4)
            Fin();
    }

     
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Debes recoger y comer 4 frutas"
            ,0,null,Mensaje.TipoMensaje.Frutas)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Cuando recoges cosas van a tu inventario"
            ,0,null,Mensaje.TipoMensaje.Aviso)
        );
         yield return StartCoroutine(tarea.MostrarMensaje(
            "Usa la vista para comer/beber lo que recojas"
            ,0,null,Mensaje.TipoMensaje.Ojo)
        );
        
    }

}
