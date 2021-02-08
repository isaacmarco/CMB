using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class JugadorTareaAventuras : MonoBehaviour
{
    
    // la referencia a la tara
    private TareaAventuras tarea; 
    // posicion 2D suavizada obtenida por el tobii
    private Vector2 puntoFiltrado = Vector2.zero;    
    // posicion a la que se esta movinedo el jugador
    private Vector3 posicionDestino; 
    [SerializeField] private GameObject modelo3D; 
    

    void Awake()
    {
        tarea = FindObjectOfType<TareaAventuras>();
    }

    void Update()
    {
        // obtenemos la posicion mirada
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        // si la informacion del dispositivo es valida
		if (gazePoint.IsValid)
		{
			Vector2 posicionGaze = gazePoint.Screen;	
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEntera = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );

            // usamos la posicion entera para realizar directamente
            // el ray picking como si fuesen las coordenadas del raton 
            Ray ray;
     	    RaycastHit hit;
		    ray = Camera.main.ScreenPointToRay(posicionEntera);
            if( Physics.Raycast(ray, out hit) )
            {
                GameObject objeto = hit.collider.gameObject; 
                // comprobar que estamos pulsando sobre este objeto 
                if(objeto.tag == "PlanoRaypicking")
                    Mover(hit.point);        
            }	

		}   else  {
            
            // debug usando el raton 
           
            Ray ray;
     	    RaycastHit hit;
		    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if( Physics.Raycast(ray, out hit) )
            {
                GameObject objeto = hit.collider.gameObject; 
                // comprobar que estamos pulsando sobre este objeto 
                if(objeto.tag == "PlanoRaypicking")
                    Mover(hit.point);
            }	
        }    

       

    }
  
    private void Mover(Vector3 destino)
    {
        // solo si la distancia es adecuada
        float umbral = 50f; 
        if( Vector3.Distance(destino, gameObject.transform.position) < umbral)
        {
            // orientamos al juador y lo impulsamos, primero calculamos
            // la direccion del jugador
            destino.y = 1; 
            Vector3 p = gameObject.transform.position; 
            p.y = 1; 
            gameObject.transform.position = p; 
            Vector3 direccion = destino - gameObject.transform.position;
            // normalizar la direccion
            direccion.Normalize();            
            // interpolar la velocidad en funcion de la distancia
            // al jugador
            float distancia = Vector3.Distance(destino, gameObject.transform.position);
            // obtener la distancia normalizada 
            float distanciaMinima = 0f; 
            float distanciaMaxima = 10f;
            float distanciaNormalizada = Mathf.InverseLerp(
                distanciaMaxima, distanciaMinima, distancia
            );
            // obtener la velocidad interpolada           
            float velocidadMaxima = 5f; 
            float velocidadMinima = 0f; 

            float velocidadInterpolada = Mathf.Lerp(
                velocidadMinima, velocidadMaxima, distanciaNormalizada
            );

          
            gameObject.transform.Translate( direccion * velocidadInterpolada * Time.deltaTime);
        }

    }
   
}
