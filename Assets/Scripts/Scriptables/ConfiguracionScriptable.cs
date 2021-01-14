using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu]
public class ConfiguracionScriptable : ScriptableObject
{
    [Header("Estado del programa")]
    public PacienteScriptable pacienteActual;    
    public NivelScriptable nivelActual;
    public Tareas tareaActual; 
    
    [Header("Configuracion global")]
    public bool utilizarRatonAdicionalmente = true; 
    public float tiempoParaSeleccionEnMenus = 1f; 
    public bool registrarMovimientoOcularEnDiario = false; 
    public int duracionDeMensajes;
    public float intervaloRegistroOcularEnHZ = 60f;
    [Range(0, 1)]
    public float volumenDelFeedback = 1f; 
    [Header("Sistema de puntuaciones")]
    public int puntuacionAciertoTopo = 100;
    public int penalizacionErrorTopo = -75;    
    public int penalizacionOmisionTopo = -25; 
    public int puntuacionAciertojaMemory = 100;
    public int penalizacionErrorMemory = -25;
    public int puntuacionNuevoRecod = 250; 
    public int puntuacionNivelBonus = 1000; 

    [Header("Configuracion tarea de topos")]
    public float tiempoParaSeleccion = 1f; 
    [Range(0, 1)]
    public float probabilidadAparicionEstimuloObjetio = 0.5f;     

    [Header("Configuracion tarea de memoria")]
    public float tiempoParaOcultarPareja = 2f; 
    public int numeroDeNivelesParaBonus = 2; 
    [Header("Configuracion tarea de naves")]
    [Range(0, 1)]
    public float velocidadDeLaNave = 1f; 
    [Header("Configuracion tarea de evaluacion")]
    public int numberoDeBloquesDeEvaluacion = 3;    
    public int duracionDelBloqueDeEvaluacion = 10;    
    public int duracionEstimuloFijacionEvaluacion = 1;    
    [Header("Configuracion tarea de galeria de tiro")]
    public int puntuacionAciertoGaleriaTiro = 100;
    public int puntuacionGemaGaleriaTiro = 500;
    public int penalizacionErrorGaleriaTiro = 50;
    public int penalizacionOmisionGaleriaTiro = 25;
    [Header("Lista de pacientes")]   
    public PacienteScriptable[] pacientes;
  
  

}
