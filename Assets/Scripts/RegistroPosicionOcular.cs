using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcular
{
    // momento
    float tiempo;
    // punto al que se mira
    int x, y;
    public RegistroPosicionOcular(int x, int y, float tiempo)
    {
        this.x = x; 
        this.y = y; 
        this.tiempo = tiempo; 
    }

    public string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo + ";" + x + ";" + "y";
    }

}
