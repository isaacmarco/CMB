using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelDisparoScriptable : NivelScriptable
{  
    [Header("Configuracion general")]
    public LocalizacionTareaDisparo localizacionDelNivel; 
    public int escenario; 
    [Header("Configuracion base de entrenamiento")]
    public EstimulosTareaDisparoEntrenamiento estimulos;
    public TipoTareaDisparoEntrenamiento tipoDeTareaDeEntrenamiento;
    public int duracionEstimuloEntrenamiento = 3;
    public int vidaEstimuloEntrenamiento = 100;
    public int tiempoParaNuevoEstimuloEntrenamiento = 2;
    [Range(0, 1)]
    public float probabilidadAparicionEstimuloErroneo = 0.5f;
    [Header("Configuracion planetas")]
    
    [Range(1, 100)]
    public int vidaMinas;
    [Range(1, 100)]
    public int potenciaLaserJugador; 
    public int escalaDianas; 
    
    [Header("Requisitos para la superacion")]
    public int t2; 
}
