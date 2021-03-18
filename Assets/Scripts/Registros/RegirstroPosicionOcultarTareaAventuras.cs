using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegirstroPosicionOcultarTareaAventuras : RegistroPosicionOcular
{
    public RegirstroPosicionOcultarTareaAventuras(
        float tiempo, int x, int y
    ) : base(tiempo, x, y)
    {
        //this.estimuloObjetivo = estimuloObjetivo; 
        //this.matrizTarea = matrizTarea; 
    }
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";";
    }
    

}
