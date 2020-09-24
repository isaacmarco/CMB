using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcular
{
    // momento
    int tiempo;
    // punto al que se mira
    int x, y;
    public RegistroPosicionOcular(int x, int y, int tiempo)
    {
        this.x = x; 
        this.y = y; 
        this.tiempo = tiempo; 
    }

}
