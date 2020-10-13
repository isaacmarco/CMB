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
    public int nivelActualTareaTopos; 
    public int nivelActualTareaMemory;
    public int partidasDeMemoryGanadas; // se reinicia cada X partidas para cargar el nivel de bonus 
    
    [Header("Puntuaciones y records")]
    public int puntuacionTareaTopos;
    public int puntuacionTareaMemory; 
    public int recordTareaMemoryNivelBajo; 
    public int recordTareaMemoryNivelMedio; 
    public int recordTareaMemoryNivelDificil; 

    public void Configurar(PacienteScriptable paciente)
    {
        if(paciente == null)
        {
            Debug.LogError("Paciente nulo");
            return; 
        }

        // datos
        codigo = paciente.codigo; 
        nombre = paciente.nombre; 
      

        // progreso
        nivelActualTareaTopos = paciente.nivelActualTareaTopos; 
        nivelActualTareaMemory = paciente.nivelActualTareaMemory;
        partidasDeMemoryGanadas = paciente.partidasDeMemoryGanadas;

        // puntuaciones 
        puntuacionTareaTopos = paciente.puntuacionTareaTopos;
        puntuacionTareaMemory = paciente.puntuacionTareaMemory; 

        // partidas de bonus
        partidasDeMemoryGanadas = paciente.partidasDeMemoryGanadas;

        // records
        recordTareaMemoryNivelBajo = paciente.recordTareaMemoryNivelBajo; 
        recordTareaMemoryNivelMedio = paciente.recordTareaMemoryNivelMedio;
        recordTareaMemoryNivelDificil = paciente.recordTareaMemoryNivelDificil;
    }


    
}
