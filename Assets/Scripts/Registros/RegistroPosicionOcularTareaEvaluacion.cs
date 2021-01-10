using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcularTareaEvaluacion : RegistroPosicionOcular
{
    public int objetivoX;
    public int objetivoY;

    public RegistroPosicionOcularTareaEvaluacion(float tiempo, int x, int y,
    int objetivoX, int objetivoY) : base(tiempo, x, y)
    {
        this.tiempo = tiempo; 
        this.x = x; 
        this.y = y;         
        this.objetivoX = objetivoX;
        this.objetivoY = objetivoY;
    }
    
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + 
        x + ";" + y + ";" + objetivoX + ";" + objetivoY;
    }
    
}
