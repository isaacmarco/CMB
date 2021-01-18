using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelGaleriaTiroScriptable : NivelScriptable
{  
    [Header("Configuracion general")]    
    public EstimulosTareaGaleriaTiro dianas;    
    public int duracionDiana = 3;    
    public int tiempoParaNuevaDiana = 2;
    public int duracionDeCadaBloqueDeDianas = 15;
    [Range(0, 1)]
    public float probabilidadAparicionGema = 0.2f;
    [Range(0, 1)]
    public float probabilidadAparicionDianaErronea = 0.5f;
    [Range(100, 1000)]
    public float municionCargador = 300;
        
    [Header("Requisitos para la superacion")]
    public int aciertosParaSuperarElNivel; 
    public int omisionesOErroresParaPerder;      
}
