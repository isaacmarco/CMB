using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaGaleriaTiro : MonoBehaviour
{
    
    public TextMeshPro nivelActual; 
    public TextMeshPro puntuacion; 
    public GameObject mensajeTareaFainalizada; 
    public GameObject mensajeNivelAcutal; 
    public GameObject botonJugar; 

    public void Actualizar()
    {

        // mostrar el nivel actual 
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro;
        this.nivelActual.text = (nivelActual + 1).ToString();
        int puntos = FindObjectOfType<Menu>().Configuracion.pacienteActual.puntuacionTareaGaleriaTiro;
        this.puntuacion.text = puntos.ToString();

        
        
        this.nivelActual.gameObject.SetActive(true);
        mensajeNivelAcutal.SetActive(true);
        mensajeTareaFainalizada.SetActive(false);
        

        int numeroNivelesEnTarea = 15;             

        if(FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro >= numeroNivelesEnTarea)
        {
            // tarea finalizada, mostramos solo el mensaje
            // y la puntuacion
            this.nivelActual.gameObject.SetActive(false);
            mensajeNivelAcutal.SetActive(false); 
            mensajeTareaFainalizada.SetActive(true);
             botonJugar.SetActive(false);

        } else {
            // mostrar el nivel actual             
            this.nivelActual.text = (nivelActual + 1).ToString();            
             botonJugar.SetActive(true);
            
        }

    }

    
}
