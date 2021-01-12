using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfazFijacionCamaraUI : InterfazFijacion
{
   
    [SerializeField] private Camera camaraInterfaz; 

    // converite las coordenadas desde el espacio 3D a coordenadas
    // de pantalla
    protected override void ConvertirCoordenadasYPosicionarInterfaz()
    {
        // transformacion de coordenadas de 2D a 3D
        Vector3 posicion3D = gameObject.transform.parent.parent.position;                  
        Vector2 posicionViewport = camaraInterfaz.WorldToViewportPoint(posicion3D);
        Vector2 posicion2D = new Vector2(
        ((posicionViewport.x * canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x * 0.5f)),
        ((posicionViewport.y * canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y * 0.5f)));
 
        // posicionar elemento
        GetComponent<RectTransform>().anchoredPosition = posicion2D;
    }

}
