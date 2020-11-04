using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazFijacion : MonoBehaviour
{
    // referencia al rect transform del canvas de esta interfaz
    private RectTransform canvasRect; 
    private Tarea tarea; 
  
    void Start()
    {
        // intentamos obtener una referencia a la tarea, 
        // si no la encontramos estamos en el menu 
        tarea = FindObjectOfType<Tarea>();                   
        
        // obtenemos el rect del canvas para calculos de coordenadas
        Canvas canvas = GetComponentInParent<Canvas>();
        // comprobar si la interfaz es de menu o de tarea
        if(tarea != null)
        {
            canvasRect = FindObjectOfType<Tarea>().CanvasRect; 
        } else {
            canvasRect = FindObjectOfType<Menu>().CanvasRect; 
        }
    }   

    // actualizar el valor de progreso de fijacion 
    public void Actualizar(float valorProgresoFijacion)
    {
        GetComponent<Image>().fillAmount  = valorProgresoFijacion; 
    }
    
    // establece el progreso de fijacion a cero 
    public void Reiniciar()
    {
        GetComponent<Image>().fillAmount  = 0f;
    }

    void Update()
    {
        // si estamos en una tarea, no debemos mostrar esta interfaz
        // de seleccion mientras la tarea este bloqueada
        if(tarea!=null)
        {
        }
        // coloca la interfaz de progreso sobre el estimulo 3D        
        ConvertirCoordenadasYPosicionarInterfaz();
    }

    // converite las coordenadas desde el espacio 3D a coordenadas
    // de pantalla
    private void ConvertirCoordenadasYPosicionarInterfaz()
    {
        // transformacion de coordenadas de 2D a 3D
        Vector3 posicion3D = gameObject.transform.parent.parent.position;                  
        Vector2 posicionViewport = Camera.main.WorldToViewportPoint(posicion3D);
        Vector2 posicion2D = new Vector2(
        ((posicionViewport.x * canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x * 0.5f)),
        ((posicionViewport.y * canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y * 0.5f)));
 
        // posicionar elemento
        GetComponent<RectTransform>().anchoredPosition = posicion2D;
    }

}
