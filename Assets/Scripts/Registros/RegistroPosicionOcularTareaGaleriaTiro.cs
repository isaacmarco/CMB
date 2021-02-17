using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcularTareaGaleriaTiro : RegistroPosicionOcular
{
    private bool recargando; 
    private int municion; 
    private EstimuloTareaGaleriaTiro estimuloA;
    private EstimuloTareaGaleriaTiro estimuloB;
    private EstimuloTareaGaleriaTiro estimuloC; 
    private int estimuloAX, estimuloAY;
    private int estimuloBX, estimuloBY;
    private int estimuloCX, estimuloCY; 

    public RegistroPosicionOcularTareaGaleriaTiro(
        float tiempo, int x, int y,
        EstimuloTareaGaleriaTiro estimuloA, 
        EstimuloTareaGaleriaTiro estimuloB, 
        EstimuloTareaGaleriaTiro estimuloC,
        int estimuloAX, int estimuloAY, 
        int estimuloBX, int estimuloBY, 
        int estimuloCX, int estimuloCY, 
        bool recargando, int municion
    ) : base(tiempo, x, y)
    {
        
        this.recargando = recargando; 
        this.municion = municion; 
        this.estimuloA = estimuloA; 
        this.estimuloB = estimuloB; 
        this.estimuloC = estimuloC; 
        this.estimuloAX = estimuloAX; 
        this.estimuloAY = estimuloAY;
        this.estimuloBX = estimuloBX; 
        this.estimuloBY = estimuloBY;
        this.estimuloCX = estimuloCX; 
        this.estimuloCY = estimuloCY; 

    }
    
    // tiempo; vision x ; vision y ; tipo estimulo A; tipo estimulo B; AX; AY; BX; BY; recargando?; municion";
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + x +";" + y + ";" +
        estimuloA + ";" + estimuloB + ";" + estimuloC + ";" + 
        estimuloAX + ";" + estimuloAY + ";" + 
        estimuloBX + ";" + estimuloBY + ";" +
        estimuloCX + ";" + estimuloCY + ";" + 
        recargando + ";" + municion;
    }
        
    
}
