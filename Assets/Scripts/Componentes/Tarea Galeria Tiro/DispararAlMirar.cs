using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class DispararAlMirar : MonoBehaviour
{
    
    // componente tobii
	private GazeAware gazeAware;	
    // referencia a la tarea
    private Tarea tarea;     
    private DianaEntrenamiento estimulo; 
    private bool estimuloMirado;
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	private InterfazFijacion interfazFijacion; 

    void Awake()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<Tarea>();
		gazeAware = GetComponent<GazeAware>();	        
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();	
        estimulo = GetComponent<DianaEntrenamiento>(); 
        // reiniciarmos la interfaz de fijacion
        DetenerFijacion();	
	}


	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoEstimulo()
	{	
		Ray ray;
     	RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// comprobamos tanto el tobii como el mouse 	
		return gazeAware.HasGazeFocus || 
			(
				tarea.Configuracion.utilizarRatonAdicionalmente && 		
				Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject
			);
	}
    
	 
    
	void Update()
	{
		// si el estimulo no esta en uso
		if(!estimulo.EnUso)
		{
			DetenerFijacion();
			return; 			
		}
		// si la tarea esta bloqueada no deberiamos usar
		// esta interfaz o si el estimulo no esta en uso		
		if(tarea.TareaBloqueada) //  || !estimulo.Visible)
		{
			
			DetenerFijacion();
			return; 
		}

		// si la diana no es completamente visible escapamos
		if(!estimulo.EsVisiblePlenamente)
		{
			DetenerFijacion();
			return;
		}

		// no disparar si estamos recargando
		if(FindObjectOfType<TareaGaleriaTiro>().Recargando)
		{
			DetenerFijacion();
			return;
		}

		// si no hay municion no disparamos
		if(!FindObjectOfType<JugadorTareaGaleriaTiro>().HayMunicion())
		{
			DetenerFijacion();
			return; 
		}
		
		// comprobamos si tenemos la vista sobre este
		// estimulo 
		if(MirandoEstimulo() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!estimuloMirado)
			{
				if(!tarea.TareaBloqueada)
					ComenzarFijacion();
			} else {
				// ya estamos mirando el estimulo 
				ContinuarFijacion();
				Disparar();
			}
							
		} else {

			if(estimuloMirado)
			{
				// lo estabamos mirando y paramos 
				DetenerFijacion();
			} else {
					
				// nunca lo  hemos mirado 
			}
		}
       
	}    	


    
	
	private void ContinuarFijacion()
	{
		estimuloMirado = true; 
		// comprobar el tiempo
		float tiempoFijacionTranscurrido = Time.unscaledTime - tiempoInicioFijacion;
		// actualizar barra de tiempo
		float tiempoNecesario = tarea.Configuracion.tiempoParaSeleccion;
		float tiempoNormalizado = tiempoFijacionTranscurrido / tiempoNecesario;		
		interfazFijacion.Actualizar(tiempoNormalizado);
		if(tiempoFijacionTranscurrido > tiempoNecesario)
		{
			// ya hemos terminado
			estimulo.Destruir();
			DetenerFijacion();
		} else {
			// debemos seguir mirando 
		}
	}

	private void ComenzarFijacion()
	{
		tiempoInicioFijacion = Time.unscaledTime;
		estimuloMirado = true; 
	}

	protected void DetenerFijacion()
	{		
		estimuloMirado = false; 
		interfazFijacion.Reiniciar();
	}
    
    private void Disparar()
    {
        estimulo.RecibirDisparo();

		FindObjectOfType<JugadorTareaGaleriaTiro>().Orientar(this.gameObject);
    }



}
