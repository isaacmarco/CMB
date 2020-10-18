using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaMemory : MonoBehaviour
{

    [SerializeField] private TextMeshPro nivelActual; 
    
    private int nivelSeleccionado; 
    
    public void Seleccionar(int nivel)
    {
        nivelSeleccionado = nivel; 
        FindObjectOfType<Menu>().Configuracion.pacienteActual.nivelActualTareaMemory = nivel;         
        CambiarNivelSeleccionado();
    }

    public void Actualizar()
    {
        // mostrar el nivel actual 
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.nivelActualTareaMemory;
        this.nivelActual.text = (nivelActual+1).ToString();
        nivelSeleccionado = nivelActual; 
    }

    private void CambiarNivelSeleccionado()
    {
        // los niveles comienzan internamente en 0 no en 1
        this.nivelActual.text = (nivelSeleccionado + 1).ToString();
    }

    public void NivelSiguiente()    
    {
        Debug.Log("Siguiente nivel");
        if(nivelSeleccionado < 99)
        {
            nivelSeleccionado++; 
            CambiarNivelSeleccionado();
        }
    }

    public void NivelAnterior()
    {
        Debug.Log("Nivel anterior");
        if(nivelSeleccionado > 1)
        {
            nivelSeleccionado--; 
            CambiarNivelSeleccionado();
        }
    }

}
