using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ConfiguracionScriptable : ScriptableObject
{
    
    public bool utilizarRatonAdicionalmente = true; 
    public float tiempoNecesarioParaSeleccion = 5f; 
    [Range(0, 1)]
    public float volumenDelFeedback = 1f; 


}
