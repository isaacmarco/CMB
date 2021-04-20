using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel0 : GuionAventura
{
    
    /*
    Encontrar el refugio por el nivel (sin inventario) pero con ítems 
    que recoger que te den punto (monedas). “Encuentra el refugio y recoge los tesoros”
    */

    public override void SalidaAlcanzada()
    {
        // conseguido
        if(!tarea.TareaBloqueada)        
            tarea.GanarPartida();
    }
    
    
    protected override IEnumerator Mensajes()
    {
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Encuentra el refugio antes de que anochezca",
            0,null,Mensaje.TipoMensaje.Topos)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Recoge los tesoros que veas",
            0,null,Mensaje.TipoMensaje.Topos)
        );
        
    }

   


}
