using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class PuntoVision : MonoBehaviour
{   
    // para la tarea de evaluacion 
    [SerializeField] private bool usandoJoystick; 

    private Image imagenPunto; 
    private float alphaNormal = 0.4f; 
    private float alphaSeleccion = 1f; 
    private Vector2 puntoFiltrado = Vector2.zero;   
    [SerializeField] private bool noCambiarColorDelPunto = false; 
    [SerializeField] private bool soloMoverEnHorizontal = false; 
    [SerializeField] private RectTransform canvasRect; 
    
    private Vector2 posicioEnPantalla; 
    public Vector2 PosicionEnPantalla{
        get {
             return this.posicioEnPantalla; 
        }
    }
    private Tarea tarea; 

    public void Mostrar()
    {        
        imagenPunto.gameObject.SetActive(true);
    }

    public void Ocultar()
    {
        imagenPunto.gameObject.SetActive(false);
    }

    void Awake()
    {
        // crear referencias
        imagenPunto = GetComponent<Image>();

        tarea = FindObjectOfType<Tarea>(); 

        // comprobar configuracion
        usandoJoystick = tarea.Configuracion.manejoTareaEvalucion == 
            ManejoTareaEvaluacion.Joystick;

        // si estamos usando joystick en la evaluacion
        // el estimulo tiene que aparecer centrado
        if(usandoJoystick)
        {
            Debug.Log ("La tarea usa joystick");
            Centrar();
        }

    }

    public void Centrar()
    {
        imagenPunto.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            960, 540
        );
    }

    void Update()
    {

        // estamos en la tarea de evaluacion y estamos utiliando
        // el joystick 

        if(usandoJoystick)
        {
                        
            // el vector esta normalizado 
            Vector2 axisJoystick = new Vector2(
                Input.GetAxis("Horizontal"),
                0
            );

            // calculamos la entrada del joystick 
            int centroVertical = 540;
            float sensibilidad = tarea.Configuracion.sensibilidadJoystick * 1000;
            Vector2 posicionPunto = imagenPunto.GetComponent<RectTransform>().anchoredPosition;             
            posicionPunto.y = centroVertical;
            posicionPunto.x += axisJoystick.x * Time.deltaTime * sensibilidad; 
            
            // movemos el estimulo
            imagenPunto.GetComponent<RectTransform>().anchoredPosition = posicionPunto; 

            // saltamos todo el codigo siguiente del update
            // para el control con la vista
            return; 
        }


        // utilizamos control ocular
        
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

            // si estamos en evaluacion, solo movemos en horizontal este punto
            if(soloMoverEnHorizontal)
            {
                // centramos en la pantalla horizontalmente
                posicionEntera.y = 540;
                
            }

            // posicionamos el punto. Debido al punto de pivote y configuracion
            // del canvas, podemos utilizar directamente las coordenadas en 
            // espacio de pantalla para dibujar en el canvas la UI del punto
            imagenPunto.GetComponent<RectTransform>().anchoredPosition = posicionEntera; // posicionEnElCanvas; 
            posicioEnPantalla = posicionEntera; 
			
		} 
      


    }
   
}
