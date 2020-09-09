using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelScriptable : ScriptableObject
{
    public int numeroDelNivel; 
    public int aciertosParaSuperarElNivel; 
    public int erroresParaPerder; 
    public Estimulos estimuloObjetivo; 
    public NivelDificultadScriptable nivelDeDificultad; 
    public bool nivelSuperado; 
}
