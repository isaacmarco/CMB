using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class DispararAlMirar : MonoBehaviour
{
    
    // componente tobii
	private GazeAware gazeAware;	
    // referencia a la tarea
    private TareaNaves tarea;     
    private Mina mina; 
    private bool objetivoMirado;
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	private InterfazFijacion interfazFijacion; 

    void Awake()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<TareaNaves>();
		gazeAware = GetComponent<GazeAware>();	        
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();	
        mina = gameObject.GetComponent<Mina>(); 
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
    
    void __Update()
    {
        if(MirandoObjetivo())
        {
            Disparar();
			ActualizarVidaEnLaInterfazFijacion();
        }
    }



	private void ActualizarVidaEnLaInterfazFijacion()
	{
		float porcentaje = mina.Vida / 1f; 
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
		
		interfazFijacion.Actualizar(mina.Vida / 100f);

		/*
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
			Disparar();
			DetenerFijacion();
		} else {
			// debemos seguir mirando 
		}*/
	}
    
    private void Disparar()
    {
        mina.RecibirImpacto();
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
