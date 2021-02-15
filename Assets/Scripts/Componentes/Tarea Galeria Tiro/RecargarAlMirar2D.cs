using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class RecargarAlMirar2D : MonoBehaviour
{
    private float alphaNormal = 0.4f; 
    private float alphaSeleccion = 1f; 
    protected bool estimuloMirado; 
    private Vector2 puntoFiltrado = Vector2.zero;    
    protected float tiempoInicioFijacion; 
	// interfaz para el progreso de fijacion 
	protected InterfazFijacion interfazFijacion; 
    protected Tarea tarea; 	    
    [SerializeField] private RectTransform interfazRecargaRect; 
    
    private bool PuntoDentroRect(Vector2 punto)
    {
        int x = (int) punto.x; 
        int y = (int) punto.y; 

        int bx = (int) interfazRecargaRect.anchoredPosition.x; 
        int by = (int) interfazRecargaRect.anchoredPosition.y; 
        int bw = (int) interfazRecargaRect.sizeDelta.x; 
        int bh = (int) interfazRecargaRect.sizeDelta.y; 
      

        return x > bx && x < bx + bw  && y > by && y < by + bh;
        
    }

    private bool MirandoEsimulo()
    {
       
               
        // actualizar alpha del punto dependiendo de si estamos
        // mirando a un objeto del juego 
        GameObject objetoFijado = TobiiAPI.GetFocusedObject();
      
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

		if (gazePoint.IsValid)
		{
			Vector2 posicionGaze = gazePoint.Screen;	
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEnteraTobii = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );

              
            // posicionamos el punto. Debido al punto de pivote y configuracion
            // del canvas, podemos utilizar directamente las coordenadas en 
            // espacio de pantalla para dibujar en el canvas la UI del punto
            //imagenPunto.GetComponent<RectTransform>().anchoredPosition = posicionEntera; // posicionEnElCanvas; 
			
            // si estamos dentro del RECT de la interfaz, 
            // es que estamos mirando en esa parte de la pantalla
                       
            return PuntoDentroRect(posicionEnteraTobii);

		} else {
            
            // no hay informacion del tobii, comprobamos
            // si estamos usando el raton
            if( tarea.Configuracion.utilizarRatonAdicionalmente)           
                return PuntoDentroRect(Input.mousePosition);
                
        }

        return false; 


    }

    
	void Update()
	{
		if(tarea.TareaBloqueada)
			return;
		
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

    
	void Awake()
	{		
		Inicio();
	}

	protected virtual void Inicio()
	{
		// creamos las referencias 
		tarea = FindObjectOfType<Tarea>();     
		interfazFijacion = GetComponentInChildren<InterfazFijacion>();		        
        DetenerFijacion();
	}

    protected virtual void SeleccionarEstimulo()
    {	        
        FindObjectOfType<TareaGaleriaTiro>().Recargar();
    }

}
