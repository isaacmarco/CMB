using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class SeleccionarAlMirarUI : MonoBehaviour
{
    
    // componente tobii
	private GazeAware gazeAware;	
    // refrencia a la tarea
    private Menu menu; 	    
    // si estamos mirando el estimulo en este frame
	private bool elementoUIMirado; 
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	private InterfazFijacion interfazFijacion; 
    // nombre del metodo que se ejecutara al mirar
    [SerializeField] private OpcionesSeleccionablesMenu opcion; 
	[SerializeField] private Tareas tarea;

    void Awake()
	{
		// creamos las referencias 
		menu = FindObjectOfType<Menu>();
		gazeAware = GetComponent<GazeAware>();	
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();		
		interfazFijacion.Reiniciar();		
	}


    
	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoElementoUI()
	{	
		Ray ray;
     	RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);		
		// comprobamos tanto el tobii como el mouse 	
		return gazeAware.HasGazeFocus || 
			(
				menu.Configuracion.utilizarRatonAdicionalmente && 		
				Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject
			);
	}

	void Update()
	{

		// comprobamos si tenemos la vista sobre este elemento de la UI
		if(MirandoElementoUI() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!elementoUIMirado)
			{
				ComenzarFijacion();
			} else {
				// ya estamos mirando el estimulo 
				ContinuarFijacion();
			}
							
		} else {

			if(elementoUIMirado)
			{
				// lo estabamos mirando y paramos 
				DetenerFijacion();
			} else {
					
				// nunca lo  hemos mirado 
			}
		}
       
	}    	

    private void Seleccionar()
    {
		FindObjectOfType<Menu>().EjecutarOpcionMenu(opcion, tarea);
	}

	private void ContinuarFijacion()
	{		
		elementoUIMirado = true; 
		// comprobar el tiempo
		float tiempoFijacionTranscurrido = Time.unscaledTime - tiempoInicioFijacion;
		// actualizar barra de tiempo
		float tiempoNecesario = menu.Configuracion.tiempoParaSeleccionEnMenus;
		float tiempoNormalizado = tiempoFijacionTranscurrido / tiempoNecesario;
		// actualizar la ui de fijacion 
		interfazFijacion.Actualizar(tiempoNormalizado);
		if(tiempoFijacionTranscurrido > tiempoNecesario)
		{			
			// ya hemos terminado
			Seleccionar();
			DetenerFijacion();
		} else {
			// debemos seguir mirando 
		}
	}

	private void ComenzarFijacion()
	{		
		tiempoInicioFijacion = Time.unscaledTime;
		elementoUIMirado = true;		
	}
	
	
	private void DetenerFijacion()
	{			
		elementoUIMirado = false; 
		interfazFijacion.Reiniciar();		
	}


}
