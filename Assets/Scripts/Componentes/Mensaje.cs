using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mensaje : MonoBehaviour
{
   
    [SerializeField] private Text mensaje; 
    [SerializeField] private Image imagen; 

    public void Ocultar()
    {
        GetComponent<CanvasGroup>().alpha = 0f; 
    }

    public void Mostrar(string mensaje, Sprite imagen = null)
    {
        Debug.Log("Mensaje: " + mensaje);        
        
        // asignar parametros 
        this.mensaje.text = mensaje;
        this.imagen.enabled = false; 

        if(imagen!=null) 
        {
            this.imagen.sprite = imagen;
            this.imagen.enabled = true;
        }
        
        // hacer el mensaje opaco 
        GetComponent<CanvasGroup>().alpha = 1f; 
    }


    /*
    // OBSOLETO, ES LA TAREA LA QUE CONTROLA EL 
    // TIEMPO DE ESPERA DE LOS MENSAJES
    private IEnumerator CorrutinaMensaje(float duracionMensaje)
    {
        // mostramos el mensaje
        GetComponent<CanvasGroup>().alpha = 1f; 
        // esperamos el tiempo acordado
        yield return new WaitForSeconds(duracionMensaje);
        // ocultamos el mensaje 
        GetComponent<CanvasGroup>().alpha = 0f; 
    }
    */

}
