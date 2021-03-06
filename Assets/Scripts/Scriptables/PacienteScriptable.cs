﻿using System.Collections;
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
    // nivel por el que va el jugador
    public int ultimoNivelDesbloqueadoTareaTopos; 
    public int ultimoNivelDesbloqueadoTareaMemory;
    public int ultimoNivelDesbloqueadoTareaGaleriaTiro;
    public int ultimoNivelDesbloqueadoTareaAventuras;
    // el nivel actual seleccionado, ya que el jugador
    // puede elegri cualquier nivel desde el menu
    public int nivelActualTareaMemory; 
    // lleva la cuenta de niveles ganados para lanzar un nivel de bonus
    public int contadorNivelesGanadosParaBonusTareaMemory; 
    // indica que el nivel que se jugara es de bonus (en la tarea de memory)
    public bool jugandoNivelDeBonusTareaMemory = false; 
    public int nivelesBonusCompletosTareaMemory = 0; 
    
    [Header("Puntuaciones")]
    public int puntuacionTareaTopos;
    public int puntuacionTareaMemory; 
    public int puntuacionTareaGaleriaTiro;
    public int puntuacionTareaAventuras;
    
    [Header("Tiempos record para tarea memory")]
    public int[] tiemposRecordPorNivelTareaMemory; 
    public bool[] nivelesConRecordTareaMemory; 
    public int[] medallasTareaMemory; 
    [Header("Multiplicador de velocidad")]
    public float multiplicadorVelocidad;

    public void Reiniciar()
    {
        ultimoNivelDesbloqueadoTareaMemory = 0;
        ultimoNivelDesbloqueadoTareaTopos = 0; 
        ultimoNivelDesbloqueadoTareaGaleriaTiro = 0;
        ultimoNivelDesbloqueadoTareaAventuras = 0; 
        nivelActualTareaMemory = 0; 
        contadorNivelesGanadosParaBonusTareaMemory = 0;
        jugandoNivelDeBonusTareaMemory = false; 
        nivelesBonusCompletosTareaMemory = 0; 
        puntuacionTareaMemory = 0; 
        puntuacionTareaTopos = 0; 
        puntuacionTareaGaleriaTiro = 0;
        puntuacionTareaAventuras = 0; 
        multiplicadorVelocidad = 1; 

        // vector de tiempos records
        tiemposRecordPorNivelTareaMemory = new int[28];
        nivelesConRecordTareaMemory = new bool[28];
        medallasTareaMemory = new int[28];
        // iniciamos todo el vector al valor maximo de entero, de modo
        // que en la primera partida el record siempre se supere
        for(int i=0; i<tiemposRecordPorNivelTareaMemory.Length; i++)
        {
            tiemposRecordPorNivelTareaMemory[i] = int.MaxValue;
            nivelesConRecordTareaMemory[i] = false; 
            medallasTareaMemory[i] = 0;
        }
    }

    
    
}
