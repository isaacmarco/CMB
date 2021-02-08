using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Bomba : MonoBehaviour
{
      
	// componente tobii
	protected GazeAware gazeAware;		
	// referecia a la tarea 
	protected TareaGaleriaTiro tarea; 	    
	// si estamos mirando el estimulo en este frame
	protected bool estimuloMirado; 
	// momento en el que empezamos a mirar este estimulo	
	protected float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	protected InterfazFijacion interfazFijacion; 

	void Awake()
	{		
		Inicio();
	}
  
    private IEnumerator CorrutinaBomba()
    {
        // inicio de vida de la bomba
        yield return new WaitForSeconds(tarea.Nivel.tiempoDetonacionBomba);
        // la bomba explota
        Detonar();
    }

    private void Detonar()
    {
        Debug.Log("La bomba ha detonado, la partida se pierde");
    }

	protected virtual void Inicio()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<TareaGaleriaTiro>();
		gazeAware = GetComponent<GazeAware>();	
        //estimulo = GetComponent<EstimuloTareaTopo>();	
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();		
        StartCoroutine(CorrutinaBomba());
	}

    protected virtual void SeleccionarEstimulo()
    {	
        
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
        /*
		if(!estimulo.EnUso || estimulo.Escondido || estimulo.Golpeado)
		{
			DetenerFijacion();
			return; 
		}*/

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
		//Debug.Log("Comenzado fijacion");
		tiempoInicioFijacion = Time.unscaledTime;
		estimuloMirado = true; 
	}
	
	
	protected void DetenerFijacion()
	{
		//Debug.Log("Fijacion detenida");
		estimuloMirado = false; 
		interfazFijacion.Reiniciar();
	}

}
