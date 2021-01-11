using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelDisparoScriptable : NivelScriptable
{  
    [Header("Configuracion general")]    
    public EstimulosTareaDisparoEntrenamiento estimulos;    
    public int duracionEstimuloEntrenamiento = 3;    
    public int tiempoParaNuevoEstimuloEntrenamiento = 2;
    public int duracionDeCadaBloqueDeDianas = 15;
    [Range(0, 1)]
    public float probabilidadAparicionEstimuloErroneo = 0.5f;
        
    [Header("Requisitos para la superacion")]
    public int t2; 
}
