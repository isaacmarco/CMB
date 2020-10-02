using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroPosicionOcultarTareaTopos : RegistroPosicionOcular
{
    // matriz para la tarae de topos    
    private EstimulosTareaTopos[] matrizTarea = new EstimulosTareaTopos[9];
    private EstimulosTareaTopos estimuloObjetivo; 

    public RegistroPosicionOcultarTareaTopos(float tiempo, int x, int y, EstimulosTareaTopos estimuloObjetivo, EstimulosTareaTopos[] matrizTarea) : base(tiempo, x, y)
    {
        this.estimuloObjetivo = estimuloObjetivo; 
        this.matrizTarea = matrizTarea; 
    }

    public override string RegistroFormateadoParaEscribirEnDisco()
    {
        return tiempo.ToString("0.0000") + ";" + 
        estimuloObjetivo + ";" + x + ";" + y + ";" + FormatearMatriz();
    }
    

    public string FormatearMatriz()
    {
        string matrizFormateada = string.Empty; 
        for(int i=0; i<matrizTarea.Length; i++)
        {
            if(i < matrizTarea.Length - 1)
            {
                matrizFormateada += matrizTarea[i] + ";";
                
            } else {
                matrizFormateada += matrizTarea[i];
            }
            
            
        }
        return matrizFormateada; 
    }
}
