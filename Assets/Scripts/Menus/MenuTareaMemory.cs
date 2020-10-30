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
        Debug.Log("Seleccionado nivel " + nivel);
        nivelSeleccionado = nivel; 
        FindObjectOfType<Menu>().configuracion.pacienteActual.nivelActualTareaMemory = nivel;         
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
        this.nivelActual.text = "Nivel " + (nivelActual+1).ToString();
        CambiarNivelSeleccionado();
        
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

                // actualizar numero nivel
                string nombre = "Nivel " + (i+1).ToString();
                nivel.gameObject.GetComponent<SeleccionarAlMirarUI>().FijarTexto(nombre);
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

        int numeroHijos = jerarquiaNiveles.childCount;
        for(int i=0; i<numeroHijos; i++)
        {
            Transform nivel = jerarquiaNiveles.GetChild(i);
            nivel.gameObject.GetComponent<SeleccionarAlMirarUI>().Desactivar();
            if( i == nivelSeleccionado)
                nivel.gameObject.GetComponent<SeleccionarAlMirarUI>().Activar();
        }

    }

    public void NivelSiguiente()    
    {
        /*
        Debug.Log("Siguiente nivel");
        if(nivelSeleccionado < 99)
        {
            nivelSeleccionado++; 
            CambiarNivelSeleccionado();
        }*/
    }

    public void NivelAnterior()
    {
        /*
        Debug.Log("Nivel anterior");
        if(nivelSeleccionado > 1)
        {
            nivelSeleccionado--; 
            CambiarNivelSeleccionado();
        }*/
    }

}
