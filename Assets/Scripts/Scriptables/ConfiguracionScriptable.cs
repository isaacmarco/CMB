using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu]
public class ConfiguracionScriptable : ScriptableObject
{
    [Header("Datos actuales")]
    public PacienteScriptable pacienteActual;
    public TareaScriptable tareaActual;     
    public NivelScriptable nivelActual;

    
    [Header("Configuracion global")]
    public bool utilizarRatonAdicionalmente = true; 
    public float tiempoParaSeleccionEnMenus = 1f; 
    public bool registrarMovimientoOcularEnDiario = false; 
    public int duracionDeMensajes;
    public float intervaloRegistroOcularEnHZ = 60f;
    [Range(0, 1)]
    public float volumenDelFeedback = 1f; 


    [Header("Configuracion tarea de topos")]
    public float tiempoParaSeleccion = 1f; 
    [Range(0, 1)]
    public float probabilidadAparicionEstimuloObjetio = 0.5f;     

    [Header("Configuracion tarea de memoria")]
    public float tiempoParaOcultarPareja = 2f; 

    [Header("Lista de tareas")]
    public TareaScriptable[] tareas; 

    [Header("Lista de pacientes")]   
    public PacienteScriptable[] pacientes;
    [Header("Estado del programa")]
    public bool hayPacienteActivo; 

}
