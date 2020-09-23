using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelToposScriptable : NivelScriptable
{
    

    [Header("Dificultad")]
    public Estimulos estimuloObjetivo; 
    public SimilitudEstimulos similitudEntreEstimulos;
    public float tiempoParaNuevoEstimulo; 
    public float tiempoPermanenciaDelEstimulo;   
    [Header("Requisitos para la superacion")]
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;      
    
}
