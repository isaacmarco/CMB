using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcultarTareaMemory : RegistroPosicionOcular
{
    private bool[] estadoTablero; 
    private bool[] tarjetasVistasPorJugador; 
  
    public RegistroPosicionOcultarTareaMemory(float tiempo, int x, int y, 
        bool[] tarjetasVistasPorJugador, 
        bool[] estadoTablero) : base(tiempo, x, y)
    {
        this.estadoTablero = estadoTablero; 
        this.tarjetasVistasPorJugador = tarjetasVistasPorJugador; 

    }

    
    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + 
        x + ";" + y + ";" + FormatearMatriz(tarjetasVistasPorJugador) + 
        ";" + FormatearMatriz(estadoTablero);
    }
    

    public string FormatearMatriz(bool[] matriz)
    {
        string matrizFormateada = string.Empty; 
        for(int i=0; i<matriz.Length; i++)
        {
            // cambiamos los valores true/false a 1 y 0
            string codificacion = matriz[i] ? "1" : "0";
            
            if(i < matriz.Length - 1)
            {                
                matrizFormateada += codificacion + ";";
            } else {
                matrizFormateada += codificacion;                 
            }
            
            
        }
        return matrizFormateada; 
    }
    
}
