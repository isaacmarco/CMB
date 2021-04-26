using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionNivel0 : GuionAventura
{
   

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
            0,null,Mensaje.TipoMensaje.Refugio)
        );
        yield return StartCoroutine(tarea.MostrarMensaje(
            "Recoge los tesoros que veas",
            0,null,Mensaje.TipoMensaje.Tesoros)
        );
        
    }

   


}
