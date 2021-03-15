using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class UsarAlMirar2D : MonoBehaviour
{
    private ItemInventario itemInventario;
    protected bool itemMirado; 
    private Vector2 puntoFiltrado = Vector2.zero;    
    protected float tiempoInicioFijacion; 	
	protected InterfazFijacion interfazFijacion; 
    protected Tarea tarea; 	    
    [SerializeField] private RectTransform rect; 
    
    private bool PuntoDentroRect(Vector2 punto)
    {
        int x = (int) punto.x; 
        int y = (int) punto.y; 

        int bx = (int) rect.anchoredPosition.x; 
        int by = (int) rect.anchoredPosition.y; 
        int bw = (int) rect.sizeDelta.x; 
        int bh = (int) rect.sizeDelta.y; 

        return x > bx && x < bx + bw  && y > by && y < by + bh;        
    }

    private bool MirandoItem()
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
		if(MirandoItem() )
		{				
			// si no estabamos mirando el estimulo entonces
			// empezamos a mirarlo 
			if(!itemMirado)
			{
				if(!tarea.TareaBloqueada)
					ComenzarFijacion();
			} else {
				// ya estamos mirando el estimulo 
				ContinuarFijacion();
			}
							
		} else {

			if(itemMirado)
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
		itemMirado = true; 
		// comprobar el tiempo
		float tiempoFijacionTranscurrido = Time.unscaledTime - tiempoInicioFijacion;
		// actualizar barra de tiempo
		float tiempoNecesario = tarea.Configuracion.tiempoParaSeleccion;
		float tiempoNormalizado = tiempoFijacionTranscurrido / tiempoNecesario;		
		interfazFijacion.Actualizar(tiempoNormalizado);        
		if(tiempoFijacionTranscurrido > tiempoNecesario)
		{
			// ya hemos terminado
			SeleccionarItem();
			DetenerFijacion();
		} else {
			// debemos seguir mirando 
		}
	}

	private void ComenzarFijacion()
	{		
		tiempoInicioFijacion = Time.unscaledTime;
		itemMirado = true; 
	}
	
	
	protected void DetenerFijacion()
	{
		itemMirado = false; 
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
        itemInventario = GetComponent<ItemInventario>();        
        DetenerFijacion();
	}

    protected virtual void SeleccionarItem()
    {	 
        itemInventario.Usar();
    }


}
