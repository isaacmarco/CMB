using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelToposScriptable : NivelScriptable
{
    
       
    [Header("Dificultad")]
    public EstimulosTareaTopos estimuloObjetivo; 
    public SimilitudEstimulos similitudEntreEstimulos;
    public int aparicionesAntesDeCambiarEstimuloObjetivo; 
    public float tiempoParaNuevoEstimulo; 
    public float tiempoPermanenciaDelEstimulo;   
    [Header("Requisitos para la superacion")]
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;      
    
}
