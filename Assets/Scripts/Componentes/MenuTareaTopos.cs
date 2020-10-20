using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaTopos : MonoBehaviour
{
    
    public TextMeshPro nivelActual; 
    public TextMeshPro puntuacion; 
    
    public void Actualizar()
    {
        // mostrar el nivel actual 
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos;
        this.nivelActual.text = (nivelActual + 1).ToString();
        int puntos = FindObjectOfType<Menu>().Configuracion.pacienteActual.puntuacionTareaTopos;
        this.puntuacion.text = puntos.ToString();
    }

}
