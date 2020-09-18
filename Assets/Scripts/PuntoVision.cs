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
        if(objetoFijado!=null)
        {
            imagenPunto.color = new Color(1f, 1f, 1f, alphaNormal);
        } else {
            imagenPunto.color = new Color(1f, 1f, 1f, alphaSeleccion);
        }

        // obtener la posicion a la que se esta mirando
        // e interpolarla con la anterior (en coordenadas de pantalla)
        Vector2 punto = TobiiAPI.GetGazePoint().Screen;     
        // punto = Input.mousePosition;         
        puntoFiltrado = Vector2.Lerp(puntoFiltrado, punto, 0.5f);

        Debug.Log(puntoFiltrado);
        
        // el punto obtenido por Tobii esta en coordenadas de pantalla
        // (0,0)-(ancho, alto). Hay que convertir esas coordenadas al 
        // espacio del Canvas donde dibujamos el punto 
        /*
        Camera camara = Camera.main; 
        Vector2 posicionEnElCanvas = Vector2.zero; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            punto, 
            camara, 
            out posicionEnElCanvas
        );*/

        // posicionamos el punto. Debido al punto de pivote y configuracion
        // del canvas, podemos utilizar directamente las coordenadas en 
        // espacio de pantalla para dibujar en el canvas la UI del punto
        imagenPunto.GetComponent<RectTransform>().anchoredPosition = puntoFiltrado; // posicionEnElCanvas; 

    }
   
}
