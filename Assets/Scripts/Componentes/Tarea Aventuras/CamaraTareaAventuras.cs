using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTareaAventuras : MonoBehaviour
{
    
    private JugadorTareaAventuras jugador; 
    private Vector3 posicion; 
    [SerializeField] private GameObject dummyPosicionCamara; 

    void Awake()
    {
        jugador = FindObjectOfType<JugadorTareaAventuras>();
        posicion = gameObject.transform.position; 
    }    

    void Update()
    {
        if(dummyPosicionCamara!=null)    
        {
            // miramos siempre al objetivo
            gameObject.transform.LookAt(jugador.gameObject.transform.position);
            // nos desplazamos con suavidad
            Vector3 destino = dummyPosicionCamara.transform.position; 
            destino.y = posicion.y; 

            gameObject.transform.position = Vector3.Lerp(
                gameObject.transform.position, destino, Time.deltaTime * 1f
            );
        }
    }
}
