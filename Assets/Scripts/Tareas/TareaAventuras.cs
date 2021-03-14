using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaAventuras : MonoBehaviour
{
    private ItemInventario[] inventario; 

    // devuelve verdadero si hay espacio en el inventario
    private bool HayEspacioInventario()
    {        
        foreach(ItemInventario espacioInventario in inventario)
            if(espacioInventario.Libre)
                return true; 
        return false;
    }

    // devuelve un epacio libre en el inventario
    private ItemInventario ObtenerEspacioInventarioLibre()
    {
        // se debe comprobar que existe espacio libre antes
        // de usar esta funcion 
        foreach(ItemInventario espacioInventario in inventario)
        {
            // devolvemos el primer espacio libre
            if(espacioInventario.Libre)
                return espacioInventario; 
        }
        // No deberia
        return null; 
    }

    private void AgregarInventario(ObjetosAventuras item)
    {        
        Debug.Log(item + " recogido");
        // buscar un hueco libre        
        ItemInventario espacioLibre = ObtenerEspacioInventarioLibre();
        // añadirlo al inventario
        espacioLibre.Agregar(item);
    }

    public bool RecogerItem(ObjetosAventuras item)
    {
        // comprobamos si hay espacio
        if(!HayEspacioInventario())
            return false; 

        switch(item)
        {
            case ObjetosAventuras.Cofre:
                // no se agrega, se suman puntos
            break;
            case ObjetosAventuras.Comida:
                AgregarInventario(item);
            break;
            case ObjetosAventuras.Llave:
                AgregarInventario(item);
            break;
        }

        // devolvemos true para que el objeto sea destruido
        return true; 
    }



    public void UsarItem()
    {
        // al mirar el item 
    }


}
