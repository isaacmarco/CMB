using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelNavesScriptable : NivelScriptable
{  
    [Header("Dificultad")]
    [Range(1, 100)]
    public int vidaMinas;
    [Range(1, 100)]
    public int potenciaLaserJugador; 
    public int escalaDianas; 
    
    [Header("Requisitos para la superacion")]
    public int t2; 
}
