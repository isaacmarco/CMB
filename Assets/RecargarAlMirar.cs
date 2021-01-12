using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class RecargarAlMirar : MonoBehaviour
{
   
   // componente tobii
	protected GazeAware gazeAware;	   
	protected Tarea tarea; 	    
	// si estamos mirando el estimulo en este frame
	protected bool estimuloMirado; 
	// momento en el que empezamos a mirar este estimulo	
	protected float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	protected InterfazFijacion interfazFijacion; 

    public Camera camaraInterfaz; 

	void Awake()
	{		
		Inicio();
	}

	protected virtual void Inicio()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<Tarea>();
		gazeAware = GetComponent<GazeAware>();	       
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();		
        DetenerFijacion();
	}

    protected virtual void SeleccionarEstimulo()
    {	
        FindObjectOfType<TareaGaleriaTiro>().Recargar();
    }


	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoEsimulo()
	{	
		Ray ray;
     	RaycastHit hit;
		ray = camaraInterfaz.ScreenPointToRay(Input.mousePosition);
		// comprobamos tanto el tobii como el mouse 	
		return gazeAware.HasGazeFocus || 
			(
				tarea.Configuracion.utilizarRatonAdicionalmente && 		
				Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject
			);
	}

	void Update()
	{
		
		// comprobamos si tenemos la vista sobre este
		// estimulo 
		if(MirandoEsimulo() )
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
			SeleccionarEstimulo();
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
}
