using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navegacion : MonoBehaviour
{
    private Vector3[] posiciones; 

    void Start()
    {
        GenerarPuntosNavegacion();
        StartCoroutine(CorrutinaTareaDisparo());
    }

    private void GenerarPuntosNavegacion()
    {        
        int numeroPosiciones = gameObject.transform.childCount; 
        posiciones = new Vector3[numeroPosiciones];
        for(int i=0; i<numeroPosiciones; i++)
        {
            // obtenemos el hijo y anotamos su posicion 
            Transform hijo = gameObject.transform.GetChild(i);
            hijo.gameObject.GetComponent<Renderer>().enabled = false; 
            posiciones[i] = hijo.position; 
        }
    }
    int bloqueActual = 0; 

    private IEnumerator CorrutinaTareaDisparo()
    {
        while(true)
        {
            // movemos la camara al siguiente bloque                            
            Vector3 posicion = posiciones[bloqueActual] + Vector3.up;
            GameObject jugador = Camera.main.gameObject; 
            float duracionAnimacionCamara = 3f; 
            iTween.MoveTo(jugador, posicion, duracionAnimacionCamara);
            
            // esperamos a que la camara llegue al nuevo bloque
            yield return new WaitForSeconds(duracionAnimacionCamara);
            
        

            // cambiamos al siguiente bloque
            bloqueActual++;
            if(bloqueActual >= posiciones.Length)
                bloqueActual = 0; 
            yield return null; 
        }
    }
   

    
}
