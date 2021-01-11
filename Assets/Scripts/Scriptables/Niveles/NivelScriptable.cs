using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NivelScriptable : ScriptableObject
{
    [Header("Configuracion General")]
    public int numeroDelNivel; 
    public Dificultad dificultad; 

    [Header("Propósito del nivel")]
    public TipoNivel tipoDeNivel;  
    
       
    
}
