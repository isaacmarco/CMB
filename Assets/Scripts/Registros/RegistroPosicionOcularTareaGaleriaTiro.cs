using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcularTareaGaleriaTiro : RegistroPosicionOcular
{
    private bool recargando; 
    private int municion; 
    private EstimuloTareaGaleriaTiro estimuloA;
    private EstimuloTareaGaleriaTiro estimuloB;
    private int estimuloAX, estimuloAY;
    private int estimuloBX, estimuloBY;

    public RegistroPosicionOcularTareaGaleriaTiro(
        float tiempo, int x, int y,
        EstimuloTareaGaleriaTiro estimuloA, EstimuloTareaGaleriaTiro estimuloB, 
        int estimuloAX, int estimuloAY, int estimuloBX, int estimuloBY, bool recargando, int municion
    ) : base(tiempo, x, y)
    {
        
        this.recargando = recargando; 
        this.municion = municion; 
        this.estimuloA = estimuloA; 
        this.estimuloB = estimuloB; 
        this.estimuloAX = estimuloAX; 
        this.estimuloAY = estimuloAY;
        this.estimuloBX = estimuloBX; 
        this.estimuloBY = estimuloBY;

    }
    
    // tiempo; vision x ; vision y ; tipo estimulo A; tipo estimulo B; AX; AY; BX; BY; recargando?; municion";
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + x +";" + y + ";" +
        estimuloA + ";" + estimuloB + ";" + 
        estimuloAX + ";" + estimuloAY + ";" + 
        estimuloBX + ";" + estimuloBY + ";" +
        recargando + ";" + municion;
    }
        
    
}
