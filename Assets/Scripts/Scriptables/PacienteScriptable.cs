using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class PacienteScriptable : ScriptableObject
{
    [Header("Datos del jugador")]
    public string nombre; 
    public string codigo; 
    public bool esZurdo; 
   

    [Header("Progreso del jugador")]
    public int ultimoNivelDesbloqueadoTareaTopos; 
    public int ultimoNivelDesbloqueadoTareaMemory;
    public int nivelActualTareaMemory; 
    // lleva la cuenta de niveles ganados para lanzar un nivel de bonus
    public int contadorNivelesGanadosParaBonus; 
    // indica que el nivel que se jugara es de bonus
    public bool jugandoNivelDeBonus = false; 
    
    [Header("Puntuaciones")]
    public int puntuacionTareaTopos;
    public int puntuacionTareaMemory; 
    
    [Header("Tiempos record para tarea memory")]
    public int[] tiemposRecordPorNivelTareaMemory; 
    

    
}
