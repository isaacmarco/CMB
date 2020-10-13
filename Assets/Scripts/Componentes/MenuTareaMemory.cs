using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaMemory : MonoBehaviour
{

    [SerializeField] private TextMeshPro nivelActual; 
    
    private int nivelSeleccionado; 
    
    public void Actualizar()
    {
        // mostrar el nivel actual 
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.nivelActualTareaMemory;
        this.nivelActual.text = nivelActual.ToString();
        nivelSeleccionado = nivelActual; 
    }

    private void CambiarNivelSeleccionado()
    {
        this.nivelActual.text = nivelSeleccionado.ToString();
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
