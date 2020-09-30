using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class SeleccionarTarjetaAlMirar : MonoBehaviour
{
   
    // componente tobii
	private GazeAware gazeAware;	
    // referencia a la tarea
    private TareaMemory tarea; 
    // referencia a la tarjeta
    private TarjetaTareaMemory tarjeta;
    // si estamos mirando el estimulo en este frame
	private bool tarjetaMirada; 
	// momento en el que empezamos a mirar este estimulo	
	private float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	private InterfazFijacion interfazFijacion; 

    /*
    TODO: UTILIZAR UNA INTERFAZ O HERENCIA DE CLASES EN UN FUTURO
    PARA NO DUPLICAR VARIAS VECES ESTE CODIGO EN LAS TAREAS
    */

    /*
    TODO: TAL VEZ EN ESTA TAREA PODEMOS INTRODUCIR
    UN BREVE RETRASO ANTES DE QUE SE EMPIECE
    A LLENAR EL CIRCULO DE PROGRESO DE FIJACION, DE MODO
    QUE PUEDAS MOVER LA VISTA SOBRE LAS TARJETAS
    SIN QUE APAREZCA TODO EL RATO EL CÍRCULO
    */

    void Awake()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<TareaMemory>();
		gazeAware = GetComponent<GazeAware>();	
        tarjeta = GetComponent<TarjetaTareaMemory>();	
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();	
        // reiniciarmos la interfaz de fijacion
        DetenerFijacion();	

	}

    public void SeleccionarTarjeta()
    {	
        tarjeta.Voltear();
    }

    
	// devuelve verdadero si estamos fijando la vista sobre
	// el modelo 3D del estimulo 
	private bool MirandoTarjeta()
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
		
        // si la tarjeta esta volteada no podemos seguir
        // intentando seleccionarla
		if(tarjeta.Volteda)
		{
			DetenerFijacion();
			return; 
		}

		// comprobamos si tenemos la vista sobre este
		// estimulo 
		if(MirandoTarjeta() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!tarjetaMirada)
			{
				ComenzarFijacion();
			} else {
				// ya estamos mirando el estimulo 
				ContinuarFijacion();
			}
							
		} else {

			if(tarjetaMirada)
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
		tarjetaMirada = true; 
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
			SeleccionarTarjeta();
			DetenerFijacion();
		} else {
			// debemos seguir mirando 
		}
	}

	private void ComenzarFijacion()
	{
		tiempoInicioFijacion = Time.unscaledTime;
		tarjetaMirada = true; 
	}
	
	
	private void DetenerFijacion()
	{
		tarjetaMirada = false; 
		interfazFijacion.Reiniciar();
	}



}
