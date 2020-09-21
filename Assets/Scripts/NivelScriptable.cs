using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelScriptable : ScriptableObject
{
    [Header("Configuracion General")]
    public int numeroDelNivel; 
    public bool nivelSuperado; 
    [Header("Dificultad")]
    public Estimulos estimuloObjetivo; 
    public SimilitudEstimulos similitudEntreEstimulos;
    public float tiempoParaNuevoEstimulo; 
    public float tiempoPermanenciaDelEstimulo;   
    [Header("Requisitos para la superacion")]
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;         
    
}
