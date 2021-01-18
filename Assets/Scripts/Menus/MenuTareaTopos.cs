using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaTopos : MonoBehaviour
{
    
    public TextMeshPro nivelActual; 
    public TextMeshPro puntuacion; 
    public GameObject mensajeTareaFainalizada; 
    public GameObject mensajeNivelAcutal; 

    public void Actualizar()
    {
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos;
        int puntos = FindObjectOfType<Menu>().Configuracion.pacienteActual.puntuacionTareaTopos;
        this.puntuacion.text = puntos.ToString();

        this.nivelActual.gameObject.SetActive(true);
        mensajeNivelAcutal.SetActive(true);
        mensajeTareaFainalizada.SetActive(false);
        

        int numeroNivelesEnTarea = 61;             

        if(FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos >= numeroNivelesEnTarea)
        {
            // tarea finalizada, mostramos solo el mensaje
            // y la puntuacion
            this.nivelActual.gameObject.SetActive(false);
            mensajeNivelAcutal.SetActive(false); 
            mensajeTareaFainalizada.SetActive(true);

        } else {
            // mostrar el nivel actual             
            this.nivelActual.text = (nivelActual + 1).ToString();            
            
        }
    }

}
