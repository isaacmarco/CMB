using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaMemory : MonoBehaviour
{

    [SerializeField] private TextMeshPro nivelActual; 
    [SerializeField] private Transform jerarquiaNiveles; 

    private int nivelSeleccionado; 
    
    public void Seleccionar(int nivel)
    {
        nivelSeleccionado = nivel; 
        FindObjectOfType<Menu>().configuracion.pacienteActual.nivelActualTareaMemory = nivel; 
        //FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaMemory = nivel;         
        CambiarNivelSeleccionado();
    }

    public void Actualizar()
    {
        // obtner el paciente actual 
        PacienteScriptable paciente = FindObjectOfType<Menu>().configuracion.pacienteActual; 
        // mostrar el nivel actual, al abrir este menu corresponde
        // al ultimo nivel desbloqueado
        int nivelActual = paciente.ultimoNivelDesbloqueadoTareaMemory;
        paciente.nivelActualTareaMemory = nivelActual; 
        nivelSeleccionado = nivelActual; 

        // nivel actual seleccionado es el ultimo desbloqueado 
        this.nivelActual.text = (nivelActual+1).ToString();
        
        // bloquear niveles no superados ocultandolos 
        int numeroHijos = jerarquiaNiveles.childCount;
        for(int i=0; i<numeroHijos; i++)
        {
            Transform nivel = jerarquiaNiveles.GetChild(i);
            if (i <= nivelActual)
            {
                // desbloquer el nivel y cargar el tiempo record
                nivel.gameObject.SetActive(true); 
                // recuperamos el record y lo mostramos
                int record = paciente.tiemposRecordPorNivelTareaMemory[i];                
                nivel.gameObject.GetComponent<SeleccionarAlMirarUI>().MostrarTiempoRecord(record);

            } else {
                // bloquear el nivel 
                nivel.gameObject.SetActive(false);
            }
        }
        
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
