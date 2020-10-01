using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcular
{
    // momento
    float tiempo;
    // punto al que se mira
    int x, y;
    // matriz para la tarae de topos
    // -1 no hay estimulo
    //  0 hay un estimulo
    //  1 esta el estimulo objetivo
    public int[] matrizTareaTopos = new int[9];

    public RegistroPosicionOcular(float tiempo, int x, int y)
    {
        this.tiempo = tiempo; 
        this.x = x; 
        this.y = y;         
    }
    
    public RegistroPosicionOcular(float tiempo, int x, int y, int[] matrizTareaTopos)
    {
        this.tiempo = tiempo; 
        this.x = x; 
        this.y = y;         
        this.matrizTareaTopos = matrizTareaTopos; 
    }

    public string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo + ";" + x + ";" + y;
    }

    public string RegistroFormateadoTareaTopos()
    {
        string matrizFormateada = string.Empty; 
        for(int i=0; i<matrizTareaTopos.Length; i++)
        {
            matrizFormateada += matrizTareaTopos[i] + ";";
            // TODO: QUITAR EL ; EN EL ULTIMO ELEMENTO
        }
        return tiempo + ";" + x + ";" + y + ";" + matrizFormateada; 
    }

}
