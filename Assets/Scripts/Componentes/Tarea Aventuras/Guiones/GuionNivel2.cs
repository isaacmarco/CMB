using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel2 : GuionAventura
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

}
