using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class SeleccionaAlMirar : MonoBehaviour
{	
	// componente tobii
	private GazeAware gazeAware;	
	// referencia al estimulo 
    private EstimuloTareaTopo estimulo;
	// referecia a la tarea 
	private TareaTopos tarea; 	    
	// si estamos mirando el estimulo en este frame
	private bool estimuloMirado; 
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	private InterfazFijacion interfazFijacion; 

	void Awake()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<TareaTopos>();
		gazeAware = GetComponent<GazeAware>();	
        estimulo = GetComponent<EstimuloTareaTopo>();	
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();		
	}

    public void SeleccionarEstimulo()
    {	
        estimulo.Golpedo();       
    }


	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoEsimulo()
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
		// si el estimulo no esta en uso, esta escondido o ya ha
		// sido golpeado abortamos la funcion 
		if(!estimulo.EnUso || estimulo.Escondido || estimulo.Golpeado)
		{
			DetenerFijacion();
			return; 
		}

		// comprobamos si tenemos la vista sobre este
		// estimulo 
		if(MirandoEsimulo() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!estimuloMirado)
			{
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
		// TODO AQUI SE ESTA DANDO UN NULL Y NO SE LA RAZON
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
		//Debug.Log("Comenzado fijacion");
		tiempoInicioFijacion = Time.unscaledTime;
		estimuloMirado = true; 
	}
	
	
	private void DetenerFijacion()
	{
		//Debug.Log("Fijacion detenida");
		estimuloMirado = false; 
		interfazFijacion.Reiniciar();
	}

}
