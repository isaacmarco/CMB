using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegirstroPosicionOcultarTareaAventuras : RegistroPosicionOcular
{
    
    private string matrizItems; 
    private string matrizPeligros; 
    // solo para debug con el reproductor de logs
    public Vector2[] items, peligros; 
    
        
    public RegirstroPosicionOcultarTareaAventuras(
        float tiempo, int x, int y,
        string matrizItems, 
        string matrizPeligros
    ) : base(tiempo, x, y)
    {
        this.matrizItems = matrizItems; 
        this.matrizPeligros = matrizPeligros;
    }
    public override string RegistroFormateadoParaEscribirEnDisco()
    {       
        return tiempo.ToString("0.0000") + ";" + x + ";" + y + ";" + 
        matrizItems + ";" + matrizPeligros;
    }
    

}
