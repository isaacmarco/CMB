﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ConfiguracionScriptable : ScriptableObject
{
    [Header("Niveles de juego")]
    public NivelScriptable nivelActual;
    [Header("Configuracion general")]
    public bool utilizarRatonAdicionalmente = true; 
    public float tiempoParaSeleccionEnMenus = 1f; 
    public float intervaloRegistroOcularEnHZ = 60f;
    [Range(0, 1)]
    public float volumenDelFeedback = 1f; 
    [Header("Configuracion de la tarea de topos")]
    public float tiempoParaSeleccion = 1f; 
    [Range(0, 1)]
    public float probabilidadAparicionEstimuloObjetio = 0.5f; 
    [Header("Lista de tareas")]
    public TareaScriptable[] tareas; 
    [Header("Pacientes")]
    public PacienteScriptable pacienteActual;
    public PacienteScriptable[] pacientes;

}
