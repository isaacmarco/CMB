using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelScriptable : ScriptableObject
{
    public int numeroDelNivel; 
    public Estimulos estimuloObjetivo; 
    public NivelDificultadScriptable nivelDeDificultad; 
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;     
    
    public bool nivelSuperado; 
}
