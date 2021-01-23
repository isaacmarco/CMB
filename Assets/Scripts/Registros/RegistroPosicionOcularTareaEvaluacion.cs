using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcularTareaEvaluacion : RegistroPosicionOcular
{
    public int objetivoX;
    public int objetivoY;
    public int numeroBloqueEvaluacion;
    public bool mostrandoEstimuloFijacion; 

    public RegistroPosicionOcularTareaEvaluacion(float tiempo, int x, int y,
    int objetivoX, int objetivoY, int numeroBloqueEvaluacion, bool mostrandoEstimuloFijacion) : base(tiempo, x, y)
    {
        this.tiempo = tiempo; 
        this.x = x; 
        this.y = y;         
        this.objetivoX = objetivoX;
        this.objetivoY = objetivoY;
        this.numeroBloqueEvaluacion = numeroBloqueEvaluacion; 
        this.mostrandoEstimuloFijacion = mostrandoEstimuloFijacion; 
    }
    
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + mostrandoEstimuloFijacion + ";" + numeroBloqueEvaluacion + ";" + 
        x + ";" + y + ";" + objetivoX + ";" + objetivoY;
    }
    
}
