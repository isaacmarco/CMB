using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class PuntoVision : MonoBehaviour
{

    private Image imagenPunto; 
    private float alphaNormal = 0.4f; 
    private float alphaSeleccion = 1f; 
    private Vector2 puntoFiltrado = Vector2.zero;   
    [SerializeField] private bool noCambiarColorDelPunto = false; 
    [SerializeField] private RectTransform canvasRect; 

    void Awake()
    {
        // crear referencias
        imagenPunto = GetComponent<Image>();
    }

    void Update()
    {
        
        
        // actualizar alpha del punto dependiendo de si estamos
        // mirando a un objeto del juego 
        GameObject objetoFijado = TobiiAPI.GetFocusedObject();

        // esta tarea puede estar necesitando su propio color para el punto de vision
        if(!noCambiarColorDelPunto)
        {
            if(objetoFijado!=null)
            {
                imagenPunto.color = new Color(1f, 1f, 1f, alphaNormal);
            } else {
                imagenPunto.color = new Color(1f, 1f, 1f, alphaSeleccion);
            }
        }
      

        GazePoint gazePoint = TobiiAPI.GetGazePoint();

		if (gazePoint.IsValid)
		{
			Vector2 posicionGaze = gazePoint.Screen;	
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEntera = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );

              
            // posicionamos el punto. Debido al punto de pivote y configuracion
            // del canvas, podemos utilizar directamente las coordenadas en 
            // espacio de pantalla para dibujar en el canvas la UI del punto
            imagenPunto.GetComponent<RectTransform>().anchoredPosition = posicionEntera; // posicionEnElCanvas; 
			
		} 
      


    }
   
}
