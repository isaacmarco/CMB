using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcular
{
    // momento
    protected float tiempo;
    // punto al que se mira
    protected int x, y;
    
    public int X{get {return x; }}
    public int Y{get { return y; }}
    
    public RegistroPosicionOcular(float tiempo, int x, int y)
    {
        this.tiempo = tiempo; 
        this.x = x; 
        this.y = y;         
    }
    
    public virtual string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + x + ";" + y;
    }
}
