using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelGaleriaTiroScriptable : NivelScriptable
{  
    [Header("Configuracion general")]    
    public DificultadTareaGaleriaTiro dianas;    
    public int rutaPorLaCiudad; 
    public int duracionDiana = 3;    
    public float tiempoParaNuevaDiana = 0.5f;
    public int duracionDeCadaBloqueDeDianas = 6;
    [Range(0, 1)]
    public float probabilidadAparicionGema = 0.2f;
    [Range(0, 1)]
    public float probabilidadAparicionDianaErronea = 0.5f;
    [Range(0, 1)]
    public float probabilidadAparicionDianaMovil = 0.3f; 
    [Range(0, 1)]
    public float probabilidadAparicionBomba = 0.3f; 
    public float tiempoDetonacionBomba = 2f; 
    public bool esNecesarioRecargar = true; 
    [Range(100, 1000)]
    public float municionCargador = 300;
        
    [Header("Requisitos para la superacion")]
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;      
}
