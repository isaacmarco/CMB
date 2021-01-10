using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class DispararObjetivoAlMirar : MonoBehaviour
{

    // componente tobii
	private GazeAware gazeAware;	
    private bool objetivoMirado;
    private Tarea tarea;
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	private InterfazFijacion interfazFijacion; 
    private ObjetivoTareaDisparo objetivo;

    void Awake()
	{
		// creamos las referencias 		
		gazeAware = GetComponent<GazeAware>();	        
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();	
        tarea = GetComponent<Tarea>();      
        objetivo = GetComponent<ObjetivoTareaDisparo>();
        // reiniciarmos la interfaz de fijacion
        DetenerFijacion();	
	}

    
  
 
	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoObjetivo()
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

	private void ActualizarVidaEnLaInterfazFijacion()
	{
		float porcentaje = objetivo.vida / 1f; 
		interfazFijacion.Actualizar(porcentaje);		
	}
 
    
	void Update()
	{
		
		// si la tarea esta bloqueada no deberiamos usar
		// esta interfaz
		//if(tarea.TareaBloqueada)
		//	return; 			    

		// comprobamos si tenemos la vista sobre este
		// estimulo 
		if(MirandoObjetivo() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!objetivoMirado)
			{
				ComenzarFijacion();
			} else {
				// ya estamos mirando el estimulo 
				ContinuarFijacion();
				Disparar();
			}
							
		} else {

			if(objetivoMirado)
			{                
				// lo estabamos mirando y paramos 
				// DetenerFijacion();
			} else {
					
				// nunca lo  hemos mirado 
			}
		}
       
	}    	


	private void ContinuarFijacion()
	{
		objetivoMirado = true; 		
		interfazFijacion.Actualizar(objetivo.vida / 100f);
	}
    
    private void Disparar()
    {
        objetivo.RecibirDisparo();
    }

	private void ComenzarFijacion()
	{
		tiempoInicioFijacion = Time.unscaledTime;
		objetivoMirado = true; 
	}
	
	
	private void DetenerFijacion()
	{
		objetivoMirado = false; 
		interfazFijacion.Reiniciar();
	}
    
    
}
