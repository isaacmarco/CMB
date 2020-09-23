using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class TareaScriptable : ScriptableObject
{
    [Header("Configuracion de la tarea")]
    public string nombre;     
    public int numeroDeNiveles; 
    [Header("Progreso del jugador")]
    public int nivelActual; 
    public int puntuacionActual; 
    
}
