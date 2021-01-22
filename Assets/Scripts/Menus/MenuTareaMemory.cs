using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaMemory : MonoBehaviour
{

    [SerializeField] private TextMeshPro nivelActual; 
    [SerializeField] private TextMeshPro puntuacion; 
    [SerializeField] private Transform jerarquiaNiveles; 
    [SerializeField] private GameObject prefabNivelMemory; 

    private int nivelSeleccionado; 
    
    public void Seleccionar(int nivel)
    {
        Debug.Log("Seleccionado nivel " + nivel);
        nivelSeleccionado = nivel; 
        FindObjectOfType<Menu>().configuracion.pacienteActual.nivelActualTareaMemory = nivel;         
        CambiarNivelSeleccionado();
    }

    private void InstanciarNiveles()
    {
        int contadorColumnas = 0; 
        int contadorFilas = 0; 

        for(int i=0; i<27; i++)
        {

            GameObject nuevoNivel = (GameObject) Instantiate(prefabNivelMemory, jerarquiaNiveles);
            nuevoNivel.transform.localPosition = Vector3.zero; 


            Vector3 posicion = new Vector3(
                contadorColumnas * 2, -contadorFilas * 2 , 0
            );

            nuevoNivel.transform.transform.localPosition = posicion; 

            nuevoNivel.name = "Nivel" + i;
            UINivelMemory ui = nuevoNivel.GetComponent<UINivelMemory>();
            ui.Configurar(i);

            // comprobar record
            PacienteScriptable paciente = FindObjectOfType<Aplicacion>().configuracion.pacienteActual; 

            //if(paciente.nivelesConRecordTareaMemory[i])
                //ui.MarcarComoRecord();

            // nuevo sistema de medallas 
            if(paciente.medallasTareaMemory!=null && paciente.medallasTareaMemory.Length != 0)           
            {
                int numeroMedallasDelNivel = paciente.medallasTareaMemory[i];
                ui.ConfigurarMedallas(numeroMedallasDelNivel);
                // == 0 no se ha jugado
                // == 1 bronce (primer record o primera partida completada)
                // == 2 plata (segundo record)
                // >= 3 oro (tercer record)
            } else {
                Debug.LogError("Los datos de este usuario son de una version anterior el programa");
            }
            
            
            
            // posicionamiento en la matriz
            contadorColumnas++;

            if(contadorColumnas > 6)
            {
                contadorFilas++;
                contadorColumnas = 0;
            }

        }
    }

    public void Actualizar()
    {
        
        InstanciarNiveles();

        // obtner el paciente actual 
        PacienteScriptable paciente = FindObjectOfType<Menu>().configuracion.pacienteActual; 

        // puntuacion 
        int puntos = paciente.puntuacionTareaMemory;
        this.puntuacion.text = puntos.ToString();



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
                //int record = paciente.tiemposRecordPorNivelTareaMemory[i];                
                //nivel.gameObject.GetComponent<UINivelMemory>().MarcarComoRecord();

                // actualizar numero nivel
                //string nombre = "Nivel " + (i+1).ToString();
                //nivel.gameObject.GetComponent<UINivelMemory>().FijarTexto( (i+1) );
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
            nivel.gameObject.GetComponent<UINivelMemory>().Desactivar();
            if( i == nivelSeleccionado)
                nivel.gameObject.GetComponent<UINivelMemory>().Activar();
        }

    }


}
