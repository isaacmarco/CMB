using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveJugador : MonoBehaviour
{    
    private TareaNaves tarea; 
    // energia de la nave 
    private int vida = 100; 
    // el momento actual para evaluar las curvas
    private float tiempo; 
    private float velocidad = 1; 

    void Start()
    {
        tarea = FindObjectOfType<TareaNaves>(); 
        gameObject.transform.position = tarea.PosicionInicial; 
    }

    void Update()
    {
        // calcular el momento actual
        tiempo += Time.deltaTime * velocidad; 
        // calcular momento siguiente
        float incrementoTiempo = 0.1f; 
        float tiempoSiguiente = tiempo + incrementoTiempo; 
        // la nave se posiciona evaluando la curva en t, 
        // y orienandose a t+1
        if(tarea!=null)
        {
            gameObject.transform.position = tarea.EvaluarCurvas(tiempo);
            gameObject.transform.LookAt(tarea.EvaluarCurvas(tiempoSiguiente));
        }
    }

    void OnGUI()
    {
        GUILayout.Label(tiempo.ToString());
    }
    
    public void Disparar()
    {        
    }

    public void RecibirImpacto()
    {
    }


}
