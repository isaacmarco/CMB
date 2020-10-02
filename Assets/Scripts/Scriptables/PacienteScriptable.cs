using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PacienteScriptable : ScriptableObject
{
    [Header("Datos del jugador")]
    public string nombre; 
    public bool esZurdo; 

    [Header("Progreso del jugador")]
    public int nivelActualTareaTopos; 
    public int nivelActualTareaMemory; 
    
    [Header("Puntuaciones")]
    public int puntuacionTareaTopos;
    public int puntuacionTareaMemory; 
    
}
