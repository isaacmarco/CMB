using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mensaje : MonoBehaviour
{
   
    [SerializeField] private Text mensaje; 

    public void Ocultar()
    {
        GetComponent<CanvasGroup>().alpha = 0f; 
    }

    public void Mostrar(string mensaje)
    {
        Debug.Log("Mensaje: " + mensaje);
        // mostrar la ui e iniciar la corrutina
        this.mensaje.text = mensaje; 
        GetComponent<CanvasGroup>().alpha = 1f; 
        // StartCoroutine(CorrutinaMensaje(duracionMensaje));         
    }

    private IEnumerator CorrutinaMensaje(float duracionMensaje)
    {
        // mostramos el mensaje
        GetComponent<CanvasGroup>().alpha = 1f; 
        // esperamos el tiempo acordado
        yield return new WaitForSeconds(duracionMensaje);
        // ocultamos el mensaje 
        GetComponent<CanvasGroup>().alpha = 0f; 
    }

}
